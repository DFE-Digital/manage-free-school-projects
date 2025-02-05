using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Sites;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites;

namespace Dfe.ManageFreeSchoolProjects.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/client/projects/{projectId}/sites")]
    [ApiController]
    public class ProjectSiteController
    {
        private readonly IGetProjectSitesCentralService _getProjectSitesCentralService;
        private readonly IGetProjectSitesPresumptionService _getProjectSitesPresumptionService;
        private readonly IUpdateProjectSitePresumptionService _updateProjectSitePresumptionService;
        private readonly ILogger<ProjectSiteController> _logger;

        public ProjectSiteController(
            IGetProjectSitesCentralService getProjectSitesCentralService,
            IGetProjectSitesPresumptionService getProjectSitesPresumptionService,
            IUpdateProjectSitePresumptionService updateProjectSitePresumptionService,
            ILogger<ProjectSiteController> logger)
        {
            _getProjectSitesCentralService = getProjectSitesCentralService;
            _getProjectSitesPresumptionService = getProjectSitesPresumptionService;
            _updateProjectSitePresumptionService = updateProjectSitePresumptionService;
            _logger = logger;
        }

        [HttpGet("central")]
        public async Task<ActionResult<ApiSingleResponseV2<GetProjectSitesCentralResponse>>> GetProjectSitesCentral(string projectId)
        {
            _logger.LogMethodEntered();

            var response = await _getProjectSitesCentralService.Execute(projectId);

            if (response == null)
            {
                response = new GetProjectSitesCentralResponse();
            }

            return new ObjectResult(new ApiSingleResponseV2<GetProjectSitesCentralResponse>(response))
            { StatusCode = StatusCodes.Status200OK };
        }

        [HttpGet("presumption")]
        public async Task<ActionResult<ApiSingleResponseV2<GetProjectSitesPresumptionResponse>>> GetProjectSitesPresumption(string projectId)
        {
            _logger.LogMethodEntered();

            var response = await _getProjectSitesPresumptionService.Execute(projectId);

            if (response == null)
            {
                response = new GetProjectSitesPresumptionResponse();
            }

            return new ObjectResult(new ApiSingleResponseV2<GetProjectSitesPresumptionResponse>(response))
            { StatusCode = StatusCodes.Status200OK };
        }

        [HttpPatch("presumption/{siteType}")]
        public async Task<ActionResult<ApiSingleResponseV2<object>>> PatchProjectSitePresumption(string projectId, ProjectSiteType siteType, UpdateProjectSitePresumptionRequest request)
        {
            _logger.LogMethodEntered();

            await _updateProjectSitePresumptionService.Execute(projectId, request, siteType);

            return new ObjectResult(new ApiSingleResponseV2<object>(new object()))
            { StatusCode = StatusCodes.Status200OK };
        }
    }
}
