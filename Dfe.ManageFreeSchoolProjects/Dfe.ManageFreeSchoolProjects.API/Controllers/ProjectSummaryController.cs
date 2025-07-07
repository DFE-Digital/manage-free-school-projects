using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Summary;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Summary;
using Dfe.ManageFreeSchoolProjects.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageFreeSchoolProjects.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/summary/project")]
    [ApiController]
    public class ProjectSummaryController : ControllerBase
    {
        private readonly IGetProjectSummaryByUserService _getProjectSummary;
        private readonly ILogger<ProjectSummaryController> _logger;

        public ProjectSummaryController(
            IGetProjectSummaryByUserService getProjectSummaryAll,
            ILogger<ProjectSummaryController> logger)
        {
            _getProjectSummary = getProjectSummaryAll;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseV2<GetProjectSummaryResponse>>> GetAllProjectSummaries(
            string projectManagedByEmail)
        {
            _logger.LogMethodEntered();

            var parameters = new GetProjectSummaryByUserParameters()
            {
                ProjectManagedByEmail = projectManagedByEmail
            };

            var (projects, recordCount) = await _getProjectSummary.Execute(parameters);

            var pagingResponse = BuildPaginationResponse(recordCount, null, null);

            var response = new ApiResponseV2<GetProjectSummaryResponse>(projects, pagingResponse);

            _logger.LogInformation("Found {ProjectCount} projects", projects.Count);

            return Ok(response);
        }

        private PagingResponse BuildPaginationResponse(int recordCount, int? page = null, int? count = null)
        {
            PagingResponse result = null;

            if (page.HasValue && count.HasValue)
                result = PagingResponseFactory.Create(page.Value, count.Value, recordCount, Request);

            return result;
        }
    }
}
