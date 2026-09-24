using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Dfe.ManageFreeSchoolProjects.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/client/proposals")]
    [ApiController]
    [SuppressMessage(
        "Major Code Smell",
        "S6960:Controllers should not have mixed responsibilities",
        Justification =
            "Creating and listing both act on the same proposals resource, so they belong on one " +
            "controller. Splitting them would fragment the API for no benefit.")]
    public class ProposalController : ControllerBase
    {
        private readonly ICreateProposalService _createProposalService;
        private readonly IGetProposalService _getProposalService;
        private readonly IUpdateProposalService _updateProposalService;
        private readonly UpdateProposalRequestValidator _updateProposalRequestValidator;
        private readonly CreateProposalRequestValidator _createProposalRequestValidator;
        private readonly ILogger<ProposalController> _logger;

        public ProposalController(
            ICreateProposalService createProposalService,
            IGetProposalService getProposalService,
            IUpdateProposalService updateProposalService,
            UpdateProposalRequestValidator updateProposalRequestValidator,
            CreateProposalRequestValidator createProposalRequestValidator,
            ILogger<ProposalController> logger)
        {
            _createProposalService = createProposalService;
            _getProposalService = getProposalService;
            _updateProposalService = updateProposalService;
            _updateProposalRequestValidator = updateProposalRequestValidator;
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

            var response = new ApiSingleResponseV2<ProposalResponse>(createResult);

            return new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status201Created
            };
        }

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<ApiSingleResponseV2<List<GetProposalSummaryResponse>>>> GetProposals(string projectId)
        {
            _logger.LogMethodEntered();

            var result = await _getProposalService.ExecuteList(projectId);

            var response = new ApiSingleResponseV2<List<GetProposalSummaryResponse>>(result);

            return new ObjectResult(response) { StatusCode = StatusCodes.Status200OK };
        }

        [HttpGet]
        [Route("{rid}")]
        public async Task<ActionResult<ApiSingleResponseV2<ProposalResponse>>> GetProposalByRid(string rid)
        {
            _logger.LogMethodEntered();

            var result = await _getProposalService.ExecuteSingle(rid);

            var response = new ApiSingleResponseV2<ProposalResponse>(result);

            return new ObjectResult(response) { StatusCode = StatusCodes.Status200OK };
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<ApiSingleResponseV2<ProposalResponse>>> UpdateProposal(UpdateProposalRequest updateProposalRequest)
        {
            _logger.LogMethodEntered();
            
            var validationResult = await _updateProposalRequestValidator.ValidateAsync(updateProposalRequest);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var result = await _updateProposalService.Execute(updateProposalRequest);

            var response = new ApiSingleResponseV2<ProposalResponse>(result);

            return new ObjectResult(response) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
