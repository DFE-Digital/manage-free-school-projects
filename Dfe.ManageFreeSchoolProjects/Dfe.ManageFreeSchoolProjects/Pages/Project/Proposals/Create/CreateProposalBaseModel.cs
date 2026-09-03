using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create
{
    public class CreateProposalBaseModel(ICreateProposalCache createProposalCache) : PageModel
    {
        protected internal string BackLink { get; set; }
        protected readonly ICreateProposalCache CreateProposalCache = createProposalCache;
    }
}
