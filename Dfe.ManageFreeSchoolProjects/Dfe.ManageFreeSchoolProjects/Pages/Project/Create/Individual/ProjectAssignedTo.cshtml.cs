using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual;

public class ProjectAssignedTo : CreateProjectBaseModel
{
    private readonly ErrorService _errorService;

    [Required(ErrorMessage = "Enter the email address")]
    [BindProperty(Name = "email")]
    public string Email { get; set; }
    
    public ProjectAssignedTo(ErrorService errorService, ICreateProjectCache createProjectCache)
        :base(createProjectCache)
    {
        _errorService = errorService;
    }

    public IActionResult OnGet()
    {
        if (!IsUserAuthorised())
        {
            return new UnauthorizedResult();
        }

        var projectCache = CreateProjectCache.Get();

        if (projectCache.ProjectType == ProjectType.NewSchool)
        {
            if (projectCache.FaithStatus == FaithStatus.None)
            {
                BackLink = RouteConstants.CreateFaithStatus;
            }
            else
            {
                BackLink = RouteConstants.CreateFaithType;
            }
        }
        else
        {
            BackLink = GetPreviousPage(CreateProjectPageName.ProjectAssignedTo);
        } 

        Email = projectCache.ProjectAssignedToEmail;

        return Page();
    }


    public IActionResult OnPost()
    {
        var projectCache = CreateProjectCache.Get();
        BackLink = GetPreviousPage(CreateProjectPageName.ProjectAssignedTo);

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(ModelState.Keys, ModelState);
            return Page();
        }

        if (!IsEducationEmailValid(Email))
        {
            ModelState.AddModelError("email", "Email address must be in the format firstname.surname@education.gov.uk");
            _errorService.AddErrors(ModelState.Keys, ModelState);
            return Page();
        }

        if (Email?.Length > 100)
        {
            ModelState.AddModelError("email", "Email address must be 100 characters or less");
        }

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(ModelState.Keys, ModelState);
            return Page();
        }

        projectCache.ProjectAssignedToEmail = Email;
        CreateProjectCache.Update(projectCache);
        
        return Redirect(RouteConstants.CreateProjectCheckYourAnswers);
    }

    private static bool IsNamePopulated(string name)
    {
        return name != null && name.Contains(' ');
    }

    private static bool IsEducationEmailValid(string email)
    {
        return email != null && email.Contains("@education.gov.uk") && new EmailAddressAttribute().IsValid(email);
    }

}