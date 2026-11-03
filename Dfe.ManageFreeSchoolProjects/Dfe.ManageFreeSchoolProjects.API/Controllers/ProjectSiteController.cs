using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Sites;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Sites.NewSchool;
using Dfe.ManageFreeSchoolProjects.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageFreeSchoolProjects.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/client/projects/{projectId}/sites")]
    [ApiController]
    public class ProjectSiteController
    {
        private readonly IGetProjectSitesCentralService _getProjectSitesCentralService;
        private readonly IGetProjectSitesPresumptionService _getProjectSitesPresumptionService;
        private readonly IGetProjectSitesNewSchoolService _getProjectSitesNewSchoolService;
        private readonly IUpdateProjectSiteService _updateProjectSiteService;
        private readonly ILogger<ProjectSiteController> _logger;

        public ProjectSiteController(
            IGetProjectSitesCentralService getProjectSitesCentralService,
            IGetProjectSitesPresumptionService getProjectSitesPresumptionService,
            IGetProjectSitesNewSchoolService getProjectSitesNewSchoolService,
            IUpdateProjectSiteService updateProjectSiteService,
            ILogger<ProjectSiteController> logger)
        {
            _getProjectSitesCentralService = getProjectSitesCentralService;
            _getProjectSitesPresumptionService = getProjectSitesPresumptionService;
            _updateProjectSiteService = updateProjectSiteService;
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
        public async Task<ActionResult<ApiSingleResponseV2<GetProjectSitesResponse>>> GetProjectSitesPresumption(string projectId)
        {
            _logger.LogMethodEntered();

            var response = await _getProjectSitesPresumptionService.Execute(projectId);

            if (response == null)
            {
                response = new GetProjectSitesResponse();
            }

            return new ObjectResult(new ApiSingleResponseV2<GetProjectSitesResponse>(response))
            { StatusCode = StatusCodes.Status200OK };
        }

        [HttpGet("new-school")]
        public async Task<ActionResult<ApiSingleResponseV2<GetProjectSitesResponse>>> GetProjectSitesNewSchool(string projectId)
        {
            _logger.LogMethodEntered();

            var response = await _getProjectSitesNewSchoolService.Execute(projectId);

            if (response == null)
            {
                response = new GetProjectSitesResponse();
            }

            return new ObjectResult(new ApiSingleResponseV2<GetProjectSitesResponse>(response))
            { StatusCode = StatusCodes.Status200OK };
        }

        [HttpPatch("presumption/{siteType}")]
        [HttpPatch("new-school/{siteType}")]
        public async Task<ActionResult<ApiSingleResponseV2<object>>> PatchProjectSite(string projectId, ProjectSiteType siteType, UpdateProjectSiteRequest request)
        {
            _logger.LogMethodEntered();

            await _updateProjectSiteService.Execute(projectId, request, siteType);

            return new ObjectResult(new ApiSingleResponseV2<object>(new object()))
            { StatusCode = StatusCodes.Status200OK };
        }
    }
}
