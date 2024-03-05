using Dfe.ManageFreeSchoolProjects.API.Contracts.Common;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Data;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.StatutoryConsultation;

    internal class GetStatutoryConsultationTaskService : IGetTaskService
    {
        private readonly MfspContext _context;

        public GetStatutoryConsultationTaskService(MfspContext context)
        {
            _context = context;

        }
        public async Task<GetProjectByTaskResponse> Get(GetTaskServiceParameters parameters)
        {
            var result = await(from kpi in parameters.BaseQuery
                join milestones in _context.Milestones on kpi.Rid equals milestones.Rid into joinedMilestones
                from milestones in joinedMilestones.DefaultIfEmpty()
                select new GetProjectByTaskResponse()
                {
                    StatutoryConsultation = new()
                    {
                        ExpectedDateForReceivingFindingsFromTrust = milestones.FsgPreOpeningMilestonesScrForecastDate,
                        DateReceived = milestones.FsgPreOpeningMilestonesScrActualDateOfCompletion,
                        Comments = milestones.FsgPreOpeningMilestonesMi80CommentsOnDecisionToApproveIfApplicable,
                        ReceivedConsultationFindingsFromTrust = milestones.FsgPreOpeningMilestonesScrReceived,
                        ConsultationFulfilsTrustSection10StatutoryDuty = milestones.FsgPreOpeningMilestonesScrFulfilsSection10StatutoryDuty,
                        SavedFindingsInWorkplacesFolder = milestones.FsgPreOpeningMilestonesScrSavedFindingsInWorkplacesFolder,
                    }
                }).FirstOrDefaultAsync();

            return result ?? new GetProjectByTaskResponse() { StatutoryConsultation = new () };
        }
    }