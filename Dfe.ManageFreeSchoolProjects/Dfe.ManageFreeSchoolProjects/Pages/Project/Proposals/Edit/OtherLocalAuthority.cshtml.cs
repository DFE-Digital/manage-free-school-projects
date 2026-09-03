using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Extensions;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Dashboard;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class OtherLocalAuthorityModel(
        IGetProposalService getProposalService,
        IGetLocalAuthoritiesService getLocalAuthoritiesService,
        IUpdateProposalService updateProposalService,
        ILogger<FaithOfOtherReligiousOrganisationModel> logger,
        ErrorService errorService
    ) : UpdateProposalBaseModel
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(SupportsGet = true, Name = "rid")]
        public string Rid { get; set; }

        public ProposalResponse Proposal { get; set; }

        [BindProperty(SupportsGet = true, Name = "region")]
        public ProjectRegion Region { get; set; }

        [BindProperty(Name = "local-authority")]
        [Display(Name = "Local Authority")]
        [Required(ErrorMessage = "Select the local authority")]
        public string LocalAuthority { get; set; }

        [BindProperty(Name = "local-authorities")]
        public Dictionary<string, string> LocalAuthorities { get; set; }

        public async Task<IActionResult> OnGet()
        {
            logger.LogMethodEntered();

            SetBackLink();

            var proposal = await getProposalService.ExecuteSingle(Rid);

            if (proposal == null || proposal.Data == null)
            {
                return NotFound();
            }

            Proposal = proposal.Data;

            LocalAuthorities = await GetLocalAuthoritiesByRegion(Region);

            LocalAuthority = Proposal.OtherLocalAuthority;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            logger.LogMethodEntered();

            SetBackLink();

            if (!ModelState.IsValid)
            {
                LocalAuthorities = await GetLocalAuthoritiesByRegion(Region);

                errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            var proposal = await getProposalService.ExecuteSingle(Rid);
            Proposal = proposal.Data;

            var updateRequest = new UpdateProposalRequest
            {
                Rid = Rid,
                Proposer = Proposal.Proposer,
                OtherLocalAuthorityRegion = Region,
                OtherLocalAuthority = LocalAuthority
            };

            await updateProposalService.Execute(updateRequest);

            return Redirect(string.Format(RouteConstants.Proposals_Details, ProjectId, Rid));
        }

        private async Task<Dictionary<string, string>> GetLocalAuthoritiesByRegion(ProjectRegion? region)
        {
            var response = await getLocalAuthoritiesService.Execute([region.ToDescription()]);

            var authorities = new Dictionary<string, string>();

            response.Regions.ForEach(r =>
            {
                r.LocalAuthorities.ForEach(authority =>
                {
                    authorities.Add(authority.LACode, authority.Name);
                });
            });

            return authorities;
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals_Edit_Other_Local_Authority_Region, ProjectId, Rid);
        }
    }
}
