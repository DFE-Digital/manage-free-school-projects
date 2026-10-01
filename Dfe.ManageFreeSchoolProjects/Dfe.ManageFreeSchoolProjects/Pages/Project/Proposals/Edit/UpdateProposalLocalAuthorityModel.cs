using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
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
    public abstract class UpdateProposalLocalAuthorityModel(
        IGetProposalService getProposalService,
        IGetLocalAuthoritiesService getLocalAuthoritiesService,
        IUpdateProposalService updateProposalService,
        ILogger logger,
        ErrorService errorService)
        : UpdateProposalEditModel(getProposalService, updateProposalService, logger, errorService)
    {
        [BindProperty(SupportsGet = true, Name = "region")]
        public ProjectRegion Region { get; set; }

        [BindProperty(Name = "local-authority")]
        [Display(Name = "Local Authority")]
        [Required(ErrorMessage = "Select the local authority")]
        public string LocalAuthority { get; set; }

        [BindProperty(Name = "local-authorities")]
        public Dictionary<string, string> LocalAuthorities { get; set; }

        protected override async Task PrepareView()
        {
            LocalAuthorities = await getLocalAuthoritiesService.GetByRegion(Region);
        }
    }
}
