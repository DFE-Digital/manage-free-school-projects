﻿using Azure;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Dashboard;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Projects;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project;
using Dfe.ManageFreeSchoolProjects.Logging;
using Microsoft.AspNetCore.Mvc;
namespace Dfe.ManageFreeSchoolProjects.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/client/project")]
    [ApiController]
    public class ProjectController : ControllerBase
	{

        private readonly ICreateProjectService _createProject;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(
            ICreateProjectService createProject,
            ILogger<ProjectController> logger)
		{
            _createProject = createProject;
            _logger = logger;
		}

        [HttpPost]
        [Route("create/individual")]
        public async Task<ActionResult<ApiResponseV2<ProjectResponse>>> CreateProject(CreateProjectRequest createProjectRequest)
        {
            _logger.LogMethodEntered();

            var project = await _createProject.Execute(createProjectRequest);

            var response = new ApiResponseV2<ProjectResponse>(project);

            return Ok(response);
        }

    }
}

