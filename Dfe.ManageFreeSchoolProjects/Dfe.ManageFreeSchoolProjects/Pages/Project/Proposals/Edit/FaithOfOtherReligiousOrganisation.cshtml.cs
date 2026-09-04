using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class FaithOfOtherReligiousOrganisationModel(
        IGetProposalService getProposalService,
        IUpdateProposalService updateProposalService,
        ILogger<FaithOfOtherReligiousOrganisationModel> logger,
        ErrorService errorService)
        : UpdateProposalEditModel(getProposalService, updateProposalService, logger, errorService)
    {
        [BindProperty(Name = "faith-type")]
        [Display(Name = "Faith of the other religious organisation")]
        [Required(ErrorMessage = "Select the faith of the other religious organisation")]
        public FaithType? FaithTypeOfOtherReligiousOrganisation { get; set; }

        [BindProperty(Name = "other-faith-type")]
        [Display(Name = "Other faith of the other religious organisation")]
        public string OtherFaithType { get; set; }

        protected override void PopulateForm()
        {
            FaithTypeOfOtherReligiousOrganisation = Proposal.FaithTypeOfOtherReligiousOrganisation;
            OtherFaithType = Proposal.OtherFaithTypeOfOtherReligiousOrganisation;
        }

        protected override void ApplyChanges(UpdateProposalRequest request)
        {
            request.FaithTypeOfOtherReligiousOrganisation = FaithTypeOfOtherReligiousOrganisation;
            request.OtherFaithTypeOfOtherReligiousOrganisation =
                FaithTypeOfOtherReligiousOrganisation == FaithType.Other ? OtherFaithType : null;
        }
    }
}
