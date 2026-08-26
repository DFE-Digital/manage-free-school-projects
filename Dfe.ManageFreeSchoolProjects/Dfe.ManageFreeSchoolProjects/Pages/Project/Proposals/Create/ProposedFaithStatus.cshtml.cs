using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create
{
    public class ProposedFaithStatusModel(
        ICreateProposalCache createProposalCache,
        ILogger<ProposedFaithStatusModel> logger,
        ErrorService errorService
    ) : CreateProposalBaseModel(createProposalCache)
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(Name = "faith-status")]
        [Required(ErrorMessage = "Select the faith status")]
        public FaithStatus Status { get; set; }

        public IActionResult OnGet()
        {
            logger.LogMethodEntered();

            SetBackLink();

            Status = CreateProposalCache.Get().ProposedFaithStatus;

            return Page();
        }

        public IActionResult OnPost()
        {
            logger.LogMethodEntered();

            SetBackLink();

            if (!ModelState.IsValid)
            {
                errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            // update cache
            var cache = CreateProposalCache.Get();

            cache.ProposedFaithStatus = Status;
            CreateProposalCache.Update(cache);

            if (Status == FaithStatus.None)
            {
                return Redirect(string.Format(RouteConstants.Proposals_Create_Check_Answers, ProjectId));
            }
            else
            {
                return Redirect(string.Format(RouteConstants.Proposals_Create_Proposed_Faith_Type, ProjectId));
            }
        }

        private void SetBackLink()
        {
            var cache = CreateProposalCache.Get();

            if (cache.Trust != null) // we came from Confirm the trust screen
            {
                BackLink = string.Format(RouteConstants.Proposals_Create_Confirm_Trust, ProjectId);
            }
            else if (cache.FaithOfDiocese != null) // we came from the faith of the Diocese screen
            {
                BackLink = string.Format(RouteConstants.Proposals_Create_Faith_Of_Diocese, ProjectId);
            }
            else if (cache.FaithTypeOfOtherReligiousOrganisation != null) // we came from faith type of the other religious organisation screen
            {
                BackLink = string.Format(RouteConstants.Proposals_Create_Faith_Of_Other_Religious_Organisation, ProjectId);
            }
            else if (!string.IsNullOrWhiteSpace(cache.OtherLocalAuthority)) // we came from the other Local authority screen
            {
                BackLink = string.Format(RouteConstants.Proposals_Create_Other_Local_Authority, ProjectId);
            }
            else if (!string.IsNullOrWhiteSpace(cache.JointProposalLocalAuthority)) // we came from the joint Local authority screen
            {
                BackLink = string.Format(RouteConstants.Proposals_Create_Joint_Proposal_Local_Authority, ProjectId);
            }
            else
            {
                BackLink = string.Format(RouteConstants.Proposals_Create_Proposer, ProjectId);
            }
        }
    }
}
