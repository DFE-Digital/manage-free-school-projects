using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class ProposedFaithStatusModel(
        IGetProposalService getProposalService,
        IUpdateProposalService updateProposalService,
        ILogger<ProposedFaithStatusModel> logger,
        ErrorService errorService
    ) : UpdateProposalBaseModel
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(SupportsGet = true, Name = "rid")]
        public string Rid { get; set; }

        public ProposalResponse Proposal { get; set; }

        [BindProperty(Name = "faith-status")]
        [Required(ErrorMessage = "Select the faith status")]
        public FaithStatus Status { get; set; }

        public async Task<IActionResult> OnGet()
        {
            logger.LogMethodEntered();

            var proposal = await getProposalService.ExecuteSingle(Rid);

            if (proposal == null || proposal.Data == null)
            {
                return NotFound();
            }

            Proposal = proposal.Data;

            Status = Proposal.ProposedFaithStatus;

            SetBackLink();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            logger.LogMethodEntered();

            SetBackLink();

            var proposal = await getProposalService.ExecuteSingle(Rid);
            Proposal = proposal.Data;

            var updateRequest = new UpdateProposalRequest
            {
                Rid = Rid,
                Proposer = Proposal.Proposer,
                ProposedFaithStatus = Status
            };

            await updateProposalService.Execute(updateRequest);

            return Redirect(string.Format(RouteConstants.Proposals_Details, ProjectId, Rid));
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals_Details, ProjectId, Rid);
        }
    }
}
