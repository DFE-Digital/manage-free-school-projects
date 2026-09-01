using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual
{
    public class ConfirmationModel : PageModel
    {
        private readonly ICreateProjectCache _createProjectCache;

        public string PageTitle { get; set; }

        public string ProjectID { get; set; }

        public string EmailToNotify { get; set; }

        public ConfirmationModel(ICreateProjectCache createProjectCache)
        {
            _createProjectCache = createProjectCache;
        }

        public void OnGet()
        {
            var project = _createProjectCache.Get();

            PageTitle = project.ProjectType == API.Contracts.Project.ProjectType.NewSchool ? "Local autority project created" : "Free school project created";
            ProjectID = project.ProjectId;
            EmailToNotify = project.ProjectAssignedToEmail;
        }
    }
}
