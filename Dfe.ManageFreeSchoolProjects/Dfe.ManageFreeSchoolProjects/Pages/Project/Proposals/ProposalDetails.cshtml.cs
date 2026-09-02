using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals
{
    public class ProposalDetailsModel(
        IGetProposalService getProposalService,
        IGetProjectOverviewService getProjectOverviewService,
        ILogger<ProposalDetailsModel> logger
    ) : PageModel
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        public ProjectOverviewResponse Project { get; set; }

        [BindProperty(SupportsGet = true, Name = "rid")]
        public string Rid { get; set; }

        public string BackLink { get; set; }

        public ProposalResponse Proposal { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            logger.LogMethodEntered();

            var result = await getProposalService.ExecuteSingle(Rid);

            if (result == null || result.Data == null)
            {
                return NotFound();
            }

            Proposal = result.Data;

            Project = await getProjectOverviewService.Execute(ProjectId);

            return Page();
        }
    }
}
