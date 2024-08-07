using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Contacts;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Contacts;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Contacts;

public class EditOfstedContactModel : PageModel
{
    private readonly IGetContactsService _getContactsService;
    
    private readonly IAddContactsService _addContactsService;
    
    private readonly IGetProjectOverviewService _getProjectOverviewService;
    
    private readonly ILogger<EditOfstedContactModel> _logger;
    
    private readonly ErrorService _errorService;
    
    [BindProperty(Name = "projectId")]
    public string ProjectId { get; set; }
    

    [BindProperty(Name = "ofsted-contact-name")]
    [ValidText(100)]
    [DisplayName("Ofsted contact name")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string OfstedContactName { get; set; }

    [BindProperty(Name = "ofsted-contact-email")]
    [DisplayName("Ofsted contact email")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string OfstedContactEmail { get; set; }

    [BindProperty(Name = "ofsted-contact-phone-number")]
    [ValidPhoneNumber]
    [DisplayName("Ofsted contact phone number")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string OfstedContactPhoneNumber { get; set; }

    [BindProperty(Name = "ofsted-contact-role")]
    [ValidText(100)]
    [DisplayName("Ofsted contact role")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]

    public string OfstedContactRole { get; set; }
    
    [BindProperty]
    public GetContactsResponse PageContacts { get; set; }
    
    public string SchoolName { get; set; }
    
    public string GetNextPage()
    {
        return string.Format(RouteConstants.Contacts, ProjectId);
    }

    public EditOfstedContactModel(IGetContactsService getContactsService,IGetProjectOverviewService projectOverviewService,IAddContactsService addContactsService,ErrorService errorService, ILogger<EditOfstedContactModel> logger )
    {
        _getContactsService = getContactsService;
        _getProjectOverviewService = projectOverviewService;
        _errorService = errorService;
        _addContactsService = addContactsService;
        _logger = logger;
    }
    
    public async Task<IActionResult> OnGet()
    {
        _logger.LogMethodEntered();

        try
        {
            var projectId = RouteData.Values["projectId"] as string;
            PageContacts = await _getContactsService.Execute(projectId);
            ProjectId = projectId;
            var project = await _getProjectOverviewService.Execute(projectId);
            SchoolName = project.ProjectStatus.CurrentFreeSchoolName;
        }
        catch (Exception ex)
        {
            _logger.LogErrorMsg(ex);
        }

        return Page();
    }
    
    public async Task<IActionResult> OnPost()
    {
        PageContacts = new GetContactsResponse()
        {
            Contacts = new ContactsTask()
            {
                OfstedContact = new Contact()
                {
                    Name = OfstedContactName,
                    Email = OfstedContactEmail,
                    PhoneNumber = OfstedContactPhoneNumber,
                    Role = OfstedContactRole
                }
            }
        };

        var projectId = RouteData.Values["projectId"] as string;
        var project = await _getProjectOverviewService.Execute(projectId);
        ProjectId = projectId;
        SchoolName = project.ProjectStatus.CurrentFreeSchoolName;

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(ModelState.Keys, ModelState);
            return Page();
        }

        if (!IsNamePopulated(OfstedContactName))
        {
            ModelState.AddModelError("ofsted-contact-name", "Enter the full name, for example John Smith");
            _errorService.AddErrors(ModelState.Keys, ModelState);
            return Page();
        }

        if (OfstedContactName.Any(char.IsDigit))
        {
            ModelState.AddModelError("ofsted-contact-name", "Ofsted contact name must not include numbers");
        }

        if (OfstedContactEmail?.Length > 100)
        {
            ModelState.AddModelError("ofsted-contact-email", "Ofsted contact email must be 100 characters or less");
        }

        if (!IsEmailValid(OfstedContactEmail))
        {
            ModelState.AddModelError("ofsted-contact-email", "Enter an email address in the correct format, like firstname.surname@outlook.com");
        }

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(ModelState.Keys, ModelState);
            return Page();
        }

        var updateContactsRequest = new UpdateContactsRequest()
        {
            Contacts = new ContactsTask()
            {
                OfstedContact = new Contact()
                {
                    Name = OfstedContactName,
                    Email = OfstedContactEmail,
                    PhoneNumber = OfstedContactPhoneNumber,
                    Role = OfstedContactRole
                }
            }
            
        };

        await _addContactsService.Execute(ProjectId, updateContactsRequest);

        return Redirect(GetNextPage());
    }

    private static bool IsNamePopulated(string name)
    {
        return name != null && name.Contains(' ');
    }

    private static bool IsEmailValid(string email)
    {
        return string.IsNullOrEmpty(email) || new EmailAddressAttribute().IsValid(email);
    }
}