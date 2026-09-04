using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals
{
    public class ProposalListModel(
        IGetProjectOverviewService getProjectOverviewService,
        IGetProposalService getProposalService,
        ILogger<ProposalListModel> logger
    ) : PageModel
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        public ProjectOverviewResponse Project { get; set; }

        public List<GetProposalSummaryResponse> Proposals { get; set; }

        public async Task<IActionResult> OnGet()
        {
            logger.LogMethodEntered();

            try
            {
                var projectId = RouteData.Values["projectId"] as string;
                Project = await getProjectOverviewService.Execute(projectId);

                var response = await getProposalService.ExecuteList(projectId);
                Proposals = response.Data;
            }
            catch (Exception ex)
            {
                logger.LogErrorMsg(ex);
            }

            return Page();
        }
    }
}