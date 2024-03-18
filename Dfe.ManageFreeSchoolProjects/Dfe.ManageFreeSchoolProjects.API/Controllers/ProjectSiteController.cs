﻿using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Sites;

namespace Dfe.ManageFreeSchoolProjects.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/client/projects/{projectId}/sites")]
    [ApiController]
    public class ProjectSiteController
    {
        private readonly IGetProjectSitesService _getProjectSitesService;
        private readonly IUpdateProjectSitesService _updateProjectSitesService;
        private readonly ILogger<ProjectSiteController> _logger;

        public ProjectSiteController(
            IGetProjectSitesService getProjectSitesService,
            IUpdateProjectSitesService updateProjectSitesService,
            ILogger<ProjectSiteController> logger)
        {
            _getProjectSitesService = getProjectSitesService;
            _updateProjectSitesService = updateProjectSitesService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiSingleResponseV2<GetProjectSitesResponse>>> GetProjectSites(string projectId)
        {
            _logger.LogMethodEntered();

            var response = await _getProjectSitesService.Execute(projectId);

            if (response == null)
            {
                response = new GetProjectSitesResponse();
            }

            return new ObjectResult(new ApiSingleResponseV2<GetProjectSitesResponse>(response))
            { StatusCode = StatusCodes.Status200OK };
        }

        [HttpPatch]
        public async Task<ActionResult<ApiSingleResponseV2<object>>> PatchProjectSites(string projectId, UpdateProjectSitesRequest request)
        {
            _logger.LogMethodEntered();

            await _updateProjectSitesService.Execute(projectId, request);

            return new ObjectResult(new ApiSingleResponseV2<object>(new object()))
            { StatusCode = StatusCodes.Status200OK };
        }
    }
}
