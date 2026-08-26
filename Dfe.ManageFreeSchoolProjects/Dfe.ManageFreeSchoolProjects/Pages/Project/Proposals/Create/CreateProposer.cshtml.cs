using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals.Enums;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create
{
    public class CreateProposerModel(
        ICreateProposalCache createProposalCache,
        ILogger<CreateProposerModel> logger,
        ErrorService errorService
    ) : CreateProposalBaseModel(createProposalCache)
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(Name = "proposer")]
        [Display(Name = "proposer")]
        [Required(ErrorMessage = "Select the proposer")]
        public ProposalProposer? Proposer { get; set; }

        [FromQuery(Name = "newProposal")]
        public bool? IsNewProposal { get; set; }

        public IActionResult OnGet()
        {
            logger.LogMethodEntered();

            SetBackLink();

            if (IsNewProposal != null && (bool)IsNewProposal)
            {
                CreateProposalCache.Delete();
            }
            else
            {
                var cache = CreateProposalCache.Get();

                Proposer = cache.Proposer;

                var previous = cache.PreviousProposer;

                if (Proposer != previous)
                {
                    cache.PreviousProposer = Proposer;
                    CreateProposalCache.Update(cache);
                }
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            logger.LogMethodEntered();

            if (!ModelState.IsValid)
            {
                errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            ClearOtherJourneysSessionProperties();

            // update cache
            var cache = CreateProposalCache.Get();

            cache.Proposer = Proposer;
            CreateProposalCache.Update(cache);

            // redirect to next page
            if (Proposer == ProposalProposer.AcademyTrust)
            {
                return Redirect(string.Format(RouteConstants.Proposals_Create_SearchTrustByTRN, ProjectId));
            }
            else if (Proposer == ProposalProposer.Diocese)
            {
                return Redirect(string.Format(RouteConstants.Proposals_Create_Name_Of_Diocese, ProjectId));
            }
            else if (Proposer == ProposalProposer.AnotherReligiousOrganisation)
            {
                return Redirect(string.Format(RouteConstants.Proposals_Create_Name_Of_Other_Religious_Organisation, ProjectId));
            }
            else if (Proposer == ProposalProposer.LocalAuthorityThatPushedSpecification)
            {
                return Redirect(string.Format(RouteConstants.Proposals_Create_Proposed_Faith_Status, ProjectId));
            }
            else if (Proposer == ProposalProposer.AnotherLocalAuthority)
            {
                return Redirect(string.Format(RouteConstants.Proposals_Create_Other_Local_Authority_Region, ProjectId));
            }
            else if (Proposer == ProposalProposer.JointProposal)
            {
                return Redirect(string.Format(RouteConstants.Proposals_Create_Joint_Proposal_Region, ProjectId));
            }

            return Page();
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals, ProjectId);
        }

        private void ClearOtherJourneysSessionProperties()
        {
            var cache = CreateProposalCache.Get();

            if (Proposer == cache.PreviousProposer) // the user has not changed the journey, so no need to clear any session data
            {
                return;
            }

            CreateProposalCache.Delete();
        }
    }
}
