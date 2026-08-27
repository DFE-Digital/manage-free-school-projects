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
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
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

        public List<GetProposalResponse> Proposals { get; set; }

        public async Task<IActionResult> OnGet()
        {
            logger.LogMethodEntered();

            try
            {
                var projectId = RouteData.Values["projectId"] as string;
                Project = await getProjectOverviewService.Execute(projectId);

                LoadTestProposals();
                //Proposals = await getProposalService.ExecuteList(projectId);
            }
            catch (Exception ex)
            {
                logger.LogErrorMsg(ex);
            }

            return Page();
        }

        private void LoadTestProposals()
        {
            Proposals =
            [
                new GetProposalResponse
                {
                    Rid = "qieu5sdlk",
                    ProjectId = ProjectId,
                    Proposer = ProposalProposer.AnotherLocalAuthority,
                    Name = "Bristol City Council",
                    ReligiousCharacterOrEthos = "None",
                    ProposedFaithType = FaithType.Hindu,
                    Status = ProposalStatus.Active
                }
            ];
        }
    }
}