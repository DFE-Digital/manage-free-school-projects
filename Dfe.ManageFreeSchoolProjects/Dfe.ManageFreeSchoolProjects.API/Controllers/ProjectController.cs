﻿using ConcernsCaseWork.API.UseCases.Project;
using Microsoft.AspNetCore.Mvc;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Projects;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels.Project;
using Dfe.ManageFreeSchoolProjects.API.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.UseCases;

namespace ConcernsCaseWork.API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ProjectController : ControllerBase
	{
		private readonly ILogger<ProjectController> _logger;
		private readonly IUseCase<CreateProjectRequest, ProjectResponse> _createProjectUseCase;
        private readonly IUseCase<GetAllProjectsRequest, ProjectResponse[]> _getProjectUseCase;

        public ProjectController(ILogger<ProjectController> logger, 
								 IUseCase<CreateProjectRequest, ProjectResponse> createProjectUseCase, 
								 IUseCase<GetAllProjectsRequest, ProjectResponse[]> getProjectUseCase)
		{
			_logger = logger;
			_createProjectUseCase = createProjectUseCase;
			_getProjectUseCase = getProjectUseCase;
		}

		[HttpGet]
		[MapToApiVersion("1.0")]
		public async Task<ActionResult<ApiResponseV2<ProjectResponse[]>>> GetProject(string user, CancellationToken cancellationToken = default)
		{
			var projects = _getProjectUseCase.Execute(new GetAllProjectsRequest() { User = user});

			var pagingResponse = PagingResponseFactory.Create(1, projects.Count(), projects.Count(), Request);
			var response = new ApiResponseV2<ProjectResponse>(projects, pagingResponse);

            return new ObjectResult(response) { StatusCode = StatusCodes.Status200OK };
		}

		[HttpPost]
		[MapToApiVersion("1.0")]
		public async Task<ActionResult<ApiSingleResponseV2<ProjectResponse>>> Create(CreateProjectRequest request, CancellationToken cancellationToken = default)
		{
			var createdProject = _createProjectUseCase.Execute(request);
			var response = new ApiSingleResponseV2<ProjectResponse>(createdProject);

			return new ObjectResult(response) { StatusCode = StatusCodes.Status201Created };
		}
	}
}
