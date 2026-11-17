using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class FaithOfDioceseModel(
        IGetProposalService getProposalService,
        IUpdateProposalService updateProposalService,
        ILogger<FaithOfDioceseModel> logger,
        ErrorService errorService)
        : UpdateProposalEditModel(getProposalService, updateProposalService, logger, errorService)
    {
        [BindProperty(Name = "faith-of-diocese")]
        [Display(Name = "faith-of-diocese")]
        [Required(ErrorMessage = "Select the faith of the diocese")]
        public FaithOfDiocese? FaithOfDiocese { get; set; }

        protected override void PopulateForm()
        {
            FaithOfDiocese = Proposal.FaithOfDiocese;
        }

        protected override void ApplyChanges(UpdateProposalRequest request)
        {
            request.FaithOfDiocese = FaithOfDiocese;
        }
    }
}
