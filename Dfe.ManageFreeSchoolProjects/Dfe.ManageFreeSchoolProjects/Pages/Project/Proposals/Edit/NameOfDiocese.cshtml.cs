using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class NameOfDioceseModel(
        IGetProposalService getProposalService,
        IUpdateProposalService updateProposalService,
        ILogger<NameOfDioceseModel> logger,
        ErrorService errorService)
        : UpdateProposalEditModel(getProposalService, updateProposalService, logger, errorService)
    {
        [BindProperty(Name = "name-of-diocese")]
        [Display(Name = "Name of diocese")]
        [Required(ErrorMessage = "Enter the name of the Diocese")]
        public string NameOfDiocese { get; set; }

        protected override void PopulateForm()
        {
            NameOfDiocese = Proposal.NameOfDiocese;
        }

        protected override void ApplyChanges(UpdateProposalRequest request)
        {
            request.NameOfDiocese = NameOfDiocese;
        }
    }
}
