using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class NameOfOtherReligiousOrganisationModel(
        IGetProposalService getProposalService,
        IUpdateProposalService updateProposalService,
        ILogger<NameOfOtherReligiousOrganisationModel> logger,
        ErrorService errorService)
        : UpdateProposalEditModel(getProposalService, updateProposalService, logger, errorService)
    {
        [BindProperty(Name = "name-of-other-religious-organisation")]
        [Display(Name = "Name of the other religious organisation")]
        [Required(ErrorMessage = "Enter the name of the other religious organisation")]
        public string NameOfOtherReligiousOrganisation { get; set; }

        protected override void PopulateForm()
        {
            NameOfOtherReligiousOrganisation = Proposal.NameOfOtherReligiousOrganisation;
        }

        protected override void ApplyChanges(UpdateProposalRequest request)
        {
            request.NameOfOtherReligiousOrganisation = NameOfOtherReligiousOrganisation;
        }
    }
}
