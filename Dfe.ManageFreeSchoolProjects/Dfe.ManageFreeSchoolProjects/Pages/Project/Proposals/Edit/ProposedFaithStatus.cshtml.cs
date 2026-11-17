using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class ProposedFaithStatusModel(
        IGetProposalService getProposalService,
        IUpdateProposalService updateProposalService,
        ILogger<ProposedFaithStatusModel> logger,
        ErrorService errorService)
        : UpdateProposalEditModel(getProposalService, updateProposalService, logger, errorService)
    {
        [BindProperty(Name = "faith-status")]
        [Required(ErrorMessage = "Select the faith status")]
        public FaithStatus Status { get; set; }

        protected override void PopulateForm()
        {
            Status = Proposal.ProposedFaithStatus;
        }

        protected override void ApplyChanges(UpdateProposalRequest request)
        {
            request.ProposedFaithStatus = Status;
        }
    }
}
