using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.Extensions;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.LocalAuthority.Decision
{
    public class ConfirmChosenProposalModel : PageModel
    {
        private readonly IGetProjectByTaskService _getProjectService;
        private readonly IGetProposalService _getProposalService;

        private readonly ILogger<ConfirmChosenProposalModel> _logger;
        private readonly ErrorService _errorService;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(SupportsGet = true, Name = "proposalId")]
        public string ProposalId { get; set; }
        public string ProposalName { get; set; } = string.Empty;

        public string CurrentFreeSchoolName { get; set; }

        [BindProperty(Name = "is-right-proposal")]
        [Required(ErrorMessage = "Select yes if this is the right proposal")]
        public bool? IsRightProposal { get; set; }

        public ConfirmChosenProposalModel(
            IGetProjectByTaskService getProjectService,
            IGetProposalService getProposalService,
            ILogger<ConfirmChosenProposalModel> logger,
            ErrorService errorService)
        {
            _getProjectService = getProjectService;
            _getProposalService = getProposalService;
            _logger = logger;
            _errorService = errorService;
        }

        public async Task<IActionResult> OnGet()
        {
            _logger.LogMethodEntered();

            await LoadPage();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            _logger.LogMethodEntered();

            _errorService.AddErrors(ModelState.Keys, ModelState);

            if (!ModelState.IsValid)
            {
                await LoadPage();

                return Page();
            }

            if (IsRightProposal == false)
            {
                return Redirect(string.Format(RouteConstants.NewSchoolWhichProposalChosen, ProjectId));
            }

            return Redirect(string.Format(RouteConstants.NewSchoolDecision, ProjectId, ProposalId));
        }

        private async Task LoadPage()
        {
            try
            {
                var project = await _getProjectService.Execute(ProjectId, TaskName.NewSchoolDecision);
                CurrentFreeSchoolName = project.SchoolName;

                var response = await _getProposalService.ExecuteList(ProjectId);
                var proposal = response.Data.Find(p => p.Rid == ProposalId);

                if (proposal?.Proposer is not null)
                {
                    ProposalName = proposal.Proposer.ToDescription();
                }
            }
            catch (Exception ex)
            {
                _logger.LogErrorMsg(ex);
            }
        }
    }
}
