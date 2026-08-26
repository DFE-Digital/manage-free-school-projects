using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Extensions;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create
{
    public class JointProposalLocalAuthorityModel(
        ICreateProposalCache createProposalCache,
        IGetLocalAuthoritiesService getLocalAuthoritiesService,
        ILogger<JointProposalLocalAuthorityModel> logger,
        ErrorService errorService
    ) : CreateProposalBaseModel(createProposalCache)
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(Name = "local-authority")]
        [Display(Name = "Local Authority")]
        [Required(ErrorMessage = "Select the local authority")]
        public string LocalAuthority { get; set; }

        [BindProperty(Name = "local-authorities")]
        public Dictionary<string, string> LocalAuthorities { get; set; }

        public async Task<ActionResult> OnGet()
        {
            logger.LogMethodEntered();

            SetBackLink();

            var cache = CreateProposalCache.Get();

            LocalAuthority = cache.JointProposalLocalAuthority;

            await SetLocalAuthorities(cache.JointProposalLocalAuthorityRegion);

            if (!string.IsNullOrEmpty(cache.JointProposalLocalAuthority))
                LocalAuthority = cache.JointProposalLocalAuthority;

            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            logger.LogMethodEntered();

            SetBackLink();

            var cache = CreateProposalCache.Get();
            await SetLocalAuthorities(cache.JointProposalLocalAuthorityRegion);

            if (!ModelState.IsValid)
            {
                errorService.AddErrors(ModelState.Keys, ModelState);

                return Page();
            }

            // update cache
            cache.JointProposalLocalAuthority = LocalAuthority;
            cache.JointProposalLocalAuthorityCode = LocalAuthorities.FirstOrDefault(x => x.Value == LocalAuthority).Key;

            CreateProposalCache.Update(cache);

            return Redirect(string.Format(RouteConstants.Proposals_Create_Proposed_Faith_Status, ProjectId));
        }

        private async Task SetLocalAuthorities(ProjectRegion? region)
        {
            var response = await GetLocalAuthoritiesByRegion(region);
            LocalAuthorities = response.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        private async Task<Dictionary<string, string>> GetLocalAuthoritiesByRegion(ProjectRegion? region)
        {
            var response = await getLocalAuthoritiesService.Execute(new List<string> { region.ToDescription() });

            var authorities = new Dictionary<string, string>();

            response.Regions.ForEach(region =>
            {
                region.LocalAuthorities.ForEach(authority =>
                {
                    authorities.Add(authority.LACode, authority.Name);
                });
            });

            return authorities;
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals_Create_Joint_Proposal_Region, ProjectId);
        }
    }
}
