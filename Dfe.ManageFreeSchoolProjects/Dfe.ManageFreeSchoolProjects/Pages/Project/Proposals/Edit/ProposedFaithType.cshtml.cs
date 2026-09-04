using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class ProposedFaithTypeModel(
        IGetProposalService getProposalService,
        IUpdateProposalService updateProposalService,
        ILogger<ProposedFaithTypeModel> logger,
        ErrorService errorService)
        : UpdateProposalEditModel(getProposalService, updateProposalService, logger, errorService)
    {
        [BindProperty(Name = "faith-type")]
        [Required(ErrorMessage = "Select the proposed faith type")]
        public FaithType FaithType { get; set; }

        [BindProperty(Name = "other-faith-type")]
        [Display(Name = "Other faith type")]
        public string OtherFaithType { get; set; }

        protected override void PopulateForm()
        {
            FaithType = Proposal.ProposedFaithType;
            OtherFaithType = Proposal.OtherFaithType;
        }

        protected override void ApplyChanges(UpdateProposalRequest request)
        {
            request.ProposedFaithType = FaithType;
            request.OtherFaithType = FaithType == FaithType.Other ? OtherFaithType : string.Empty;
        }
    }
}
