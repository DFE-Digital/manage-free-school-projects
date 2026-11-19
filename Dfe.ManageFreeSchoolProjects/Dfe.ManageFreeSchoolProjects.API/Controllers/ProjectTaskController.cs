using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Tasks;
using Dfe.ManageFreeSchoolProjects.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageFreeSchoolProjects.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/client/projects/{projectId}/tasks")]
    [ApiController]
    public class ProjectTaskController(
        IUpdateProjectByTaskService updateProjectTaskService,
        IGetProjectByTaskService getProjectByTaskService,
        IGetTasksService getTasksService,
        ILogger<ProjectTaskController> logger)
        : ControllerBase
    {
        [HttpPatch]
        public async Task<ActionResult> PatchTask(string projectId, UpdateProjectByTaskRequest request)
        {
            logger.LogMethodEntered();

            await updateProjectTaskService.Execute(projectId, request);

            return new OkResult();
        }

        [HttpGet]
        [Route("{taskName}")]
        public async Task<ActionResult<ApiSingleResponseV2<GetProjectByTaskResponse>>> GetProjectByTask(string projectId, TaskName taskName)
        {
            logger.LogMethodEntered();

            var projectByTask = await getProjectByTaskService.Execute(projectId, taskName);

            if (projectByTask == null)
            {
                logger.LogInformation("No project could be found for the given project id {projectId}", projectId);
                return new NotFoundResult();
            }

            var result = new ApiSingleResponseV2<GetProjectByTaskResponse>(projectByTask);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status200OK };
        }

        [HttpGet]
        [Route("summary")]
        public async Task<ActionResult<ApiSingleResponseV2<ProjectByTaskSummaryResponse>>> GetProjectTaskListSummary(
            string projectId)
        {
            logger.LogMethodEntered();

            var summary = await getTasksService.Execute(projectId);
           
            var response = new ApiSingleResponseV2<ProjectByTaskSummaryResponse>(summary);

            return new ObjectResult(response) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
