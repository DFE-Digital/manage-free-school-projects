using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Exceptions;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks
{
    public interface IUpdateProjectByTaskService
    {
        public Task Execute(string projectId, UpdateProjectByTaskRequest request);
    }

    public class UpdateProjectByTaskService(MfspContext context, IEnumerable<IUpdateTaskService> updateTaskServices) : IUpdateProjectByTaskService
    {
        public async Task Execute(string projectId, UpdateProjectByTaskRequest request)
        {
            var dbKpi = await context.Kpi.FirstOrDefaultAsync(p => p.ProjectStatusProjectId == projectId);

            if (dbKpi == null)
            {
                throw new NotFoundException($"Project {projectId} not found");
            }

            var updateParameters = new UpdateTaskServiceParameters
            {
                Kpi = dbKpi,
                ProjectId = projectId,
                Request = request
            };

            foreach (var updateTaskService in updateTaskServices)
            {
                await updateTaskService.Update(updateParameters);
            }

            await UpdateTaskStatus(dbKpi.Rid, Status.InProgress, request);

            await SetProposedDecision(request.NewSchoolDecision);

            await context.SaveChangesAsync();
        }

        private async Task UpdateTaskStatus(string taskRid, Status updatedStatus,
            UpdateProjectByTaskRequest updateProjectByTaskRequest)
        {
            var taskNameToUpdate = Enum.Parse<TaskName>(updateProjectByTaskRequest.TaskToUpdate);

            var task = await context.Tasks.FirstOrDefaultAsync(x => x.Rid == taskRid && x.TaskName == taskNameToUpdate);
            if (task is null)
                return;

            task.Status = updatedStatus;
        }

        private async Task SetProposedDecision(NewSchoolDecisionTask decisionTask)
        {
            if (decisionTask is null)
                return;

            var proposal = await context.Proposals.FirstOrDefaultAsync(x => x.Rid == decisionTask.ProposalId);
            if (proposal is null)
                return;

            proposal.Status = "Successful";

            var others = await context.Proposals.Where(x => x.ProjectId == proposal.ProjectId && x.Rid != proposal.Rid).ToListAsync();

            foreach (var other in others)
            {
                other.Status = "Unsuccessful";
            }
        }
    }
}