using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Extensions;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.LocalAuthority.Decision
{
    public class WhichProposalChosenModel : PageModel
    {
        private readonly IGetProjectByTaskService _getProjectService;
        private readonly IGetProposalService _getProposalService;

        private readonly ILogger<WhichProposalChosenModel> _logger;
        private readonly ErrorService _errorService;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        public string CurrentFreeSchoolName { get; set; }

        public List<KeyValuePair<string, string>> Proposals { get; set; } = [];

        [BindProperty(Name = "chosen-proposal")]
        [Required(ErrorMessage = "Select the proposal that has been chosen")]
        public string ChosenProposalId { get; set; }

        public WhichProposalChosenModel(
            IGetProjectByTaskService getProjectService,
            IGetProposalService getProposalService,
            ILogger<WhichProposalChosenModel> logger,
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

        public async Task<ActionResult> OnPost()
        {
            _logger.LogMethodEntered();

            _errorService.AddErrors(ModelState.Keys, ModelState);

            if (!ModelState.IsValid)
            {
                await LoadPage();

                return Page();
            }

            return Redirect(string.Format(RouteConstants.NewSchoolConfirmProposalChosen, ProjectId, ChosenProposalId));
        }

        private async Task LoadPage()
        {
            try
            {
                var project = await _getProjectService.Execute(ProjectId, TaskName.NewSchoolDecision);
                CurrentFreeSchoolName = project.SchoolName;

                var response = await _getProposalService.ExecuteList(ProjectId);
                Proposals = [.. response.Data.Select(p => new KeyValuePair<string, string>(p.Rid, p.Proposer.ToDescription()))];
            }
            catch (Exception ex)
            {
                _logger.LogErrorMsg(ex);
            }
        }
    }
}
