using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Extensions;
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
    /// <summary>
    /// The "another local authority" and "joint proposal" journeys both pick a local authority from
    /// the region chosen on the previous page. The pages differ only in which cache fields they read
    /// and write and where they link back to, so the selection itself lives here.
    /// </summary>
    public abstract class ProposalLocalAuthorityModel(
        ICreateProposalCache createProposalCache,
        IGetLocalAuthoritiesService getLocalAuthoritiesService,
        ILogger logger,
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

        /// <summary>The page the user came from to reach this one.</summary>
        protected abstract string BackLinkRoute { get; }

        /// <summary>The region whose authorities this page lists.</summary>
        protected abstract ProjectRegion? GetRegion(CreateProposalCacheItem cache);

        /// <summary>The authority already chosen on this journey, if the user is returning.</summary>
        protected abstract string GetSelectedLocalAuthority(CreateProposalCacheItem cache);

        /// <summary>Stores the chosen authority and its LA code against this journey.</summary>
        protected abstract void StoreLocalAuthority(CreateProposalCacheItem cache, string name, string code);

        public async Task<ActionResult> OnGet()
        {
            LogEntered(nameof(OnGet));

            SetBackLink();

            var cache = CreateProposalCache.Get();

            LocalAuthority = GetSelectedLocalAuthority(cache);

            await SetLocalAuthorities(GetRegion(cache));

            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            LogEntered(nameof(OnPost));

            SetBackLink();

            var cache = CreateProposalCache.Get();
            await SetLocalAuthorities(GetRegion(cache));

            if (!ModelState.IsValid)
            {
                errorService.AddErrors(ModelState.Keys, ModelState);

                return Page();
            }

            var code = LocalAuthorities.FirstOrDefault(x => x.Value == LocalAuthority).Key;

            StoreLocalAuthority(cache, LocalAuthority, code);

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
            BackLink = string.Format(BackLinkRoute, ProjectId);
        }

        // The logger is injected as ILogger<TheConcretePage>, so the category still names the page
        // the user is on. LogMethodEntered needs the generic interface, which this base does not take.
        private void LogEntered(string method)
        {
            logger.LogInformation("{Page}::{Method} entered", GetType().Name, method);
        }
    }
}
