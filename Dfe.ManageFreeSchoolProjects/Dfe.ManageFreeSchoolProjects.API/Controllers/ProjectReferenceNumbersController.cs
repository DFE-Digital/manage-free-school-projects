using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ReferenceNumbers;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.ReferenceNumbers;

namespace Dfe.ManageFreeSchoolProjects.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/client/projects/{projectId}/referencenumbers")]
    [ApiController]
    public class ProjectReferenceNumbersController
    {
        private readonly IGetProjectReferenceNumbersService _getProjectReferenceNumbersService;
        private readonly IUpdateProjectReferenceNumbersService _updateProjectReferenceNumbersService;
        private readonly ILogger<ProjectReferenceNumbersController> _logger;

        public ProjectReferenceNumbersController(
            IGetProjectReferenceNumbersService getProjectReferenceNumbersService,
            IUpdateProjectReferenceNumbersService updateProjectReferenceNumbersService,
            ILogger<ProjectReferenceNumbersController> logger)
        {
            _getProjectReferenceNumbersService = getProjectReferenceNumbersService;
            _updateProjectReferenceNumbersService = updateProjectReferenceNumbersService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiSingleResponseV2<GetProjectReferenceNumbersResponse>>> GetProjectReferenceNumbers(string projectId)
        {
            _logger.LogMethodEntered();

            var response = await _getProjectReferenceNumbersService.Execute(projectId);

            if (response == null)
            {
                response = new GetProjectReferenceNumbersResponse();
            }

            return new ObjectResult(new ApiSingleResponseV2<GetProjectReferenceNumbersResponse>(response))
            { StatusCode = StatusCodes.Status200OK };
        }

        [HttpPatch]
        public async Task<ActionResult<ApiSingleResponseV2<object>>> PatchProjectReferenceNumbers(string projectId, UpdateProjectReferenceNumbersRequest request)
        {
            _logger.LogMethodEntered();

            await _updateProjectReferenceNumbersService.Execute(projectId, request);

            return new ObjectResult(new ApiSingleResponseV2<object>(new object()))
            { StatusCode = StatusCodes.Status200OK };
        }
    }
}
