using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public abstract class UpdateProposalBaseModel(
        IGetProposalService getProposalService,
        ILogger logger) : PageModel
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(SupportsGet = true, Name = "rid")]
        public string Rid { get; set; }

        public ProposalResponse Proposal { get; set; }

        protected internal string BackLink { get; set; }

        protected virtual string BackLinkRoute => RouteConstants.Proposals_Details;

        protected string ProposalDetailsUrl => string.Format(RouteConstants.Proposals_Details, ProjectId, Rid);

        protected void SetBackLink()
        {
            BackLink = string.Format(BackLinkRoute, ProjectId, Rid);
        }

        protected async Task<ProposalResponse> LoadProposal()
        {
            var response = await getProposalService.ExecuteSingle(Rid);

            Proposal = response?.Data;

            return Proposal;
        }

        protected void LogPageEntered([CallerMemberName] string memberName = "")
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("{Page}::{Method} entered", GetType().Name, memberName);
            }
        }
    }
}
