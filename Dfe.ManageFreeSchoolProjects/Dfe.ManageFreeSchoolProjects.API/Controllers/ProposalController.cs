using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Projects;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageFreeSchoolProjects.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/client/proposals")]
    [ApiController]
    public class ProposalController : ControllerBase
    {
        private readonly ICreateProposalService _createProposalService;
        private readonly CreateProposalRequestValidator _createProposalRequestValidator;
        private readonly ILogger<ProposalController> _logger;

        public ProposalController(
            ICreateProposalService createProposalService,
            CreateProposalRequestValidator createProposalRequestValidator,
            ILogger<ProposalController> logger)
        {
            _createProposalService = createProposalService;
            _createProposalRequestValidator = createProposalRequestValidator;
            _logger = logger;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateProposal(CreateProposalRequest createProposalRequest)
        {
            _logger.LogMethodEntered();

            var validationResult = await _createProposalRequestValidator.ValidateAsync(createProposalRequest);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var createResult = await _createProposalService.Execute(createProposalRequest);

            var response = new ApiSingleResponseV2<CreateProposalResponse>(createResult);

            return new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
    }
}
