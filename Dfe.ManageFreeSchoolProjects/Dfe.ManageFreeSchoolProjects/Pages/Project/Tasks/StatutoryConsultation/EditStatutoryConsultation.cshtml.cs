using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Models;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;


namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.StatutoryConsultation
{
    public class EditStatutoryConsultationTaskModel : PageModel
    {
        private readonly IGetProjectByTaskService _getProjectService;
        private readonly IUpdateProjectByTaskService _updateProjectTaskService;
        private readonly ILogger<EditStatutoryConsultationTaskModel> _logger;
        private readonly ErrorService _errorService;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(Name = "expected-date-for-receiving-findings-from-trust", BinderType = typeof(DateInputModelBinder))]
        public DateTime? ExpectedDateForReceivingFindingsFromTrust { get; set; }

        [BindProperty(Name = "received-consultation-findings-from-trust")]
        public bool? ReceivedConsultationFindingsFromTrust { get; set; }

        [BindProperty(Name = "date-received", BinderType = typeof(DateInputModelBinder))]
        public DateTime? DateReceived { get; set; }

        [BindProperty(Name = "consultation-fulfils-trust-section-10-statutory-duty")]
        public bool? ConsultationFulfilsTrustSection10StatutoryDuty { get; set; }

        [BindProperty(Name = "comments")]
        [ValidText(100)]
        public string Comments { get; set; }

        [BindProperty(Name = "saved-findings-in-workplaces-folder")]
        public bool? SavedFindingsInWorkplacesFolder { get; set; }

        public string SchoolName { get; set; }

        public EditStatutoryConsultationTaskModel(IGetProjectByTaskService getProjectService,
            IUpdateProjectByTaskService updateProjectTaskService,
            ILogger<EditStatutoryConsultationTaskModel> logger,
            ErrorService errorService)
        {
            _getProjectService = getProjectService;
            _updateProjectTaskService = updateProjectTaskService;
            _logger = logger;
            _errorService = errorService;
        }

        public async Task<ActionResult> OnGet()
        {
            _logger.LogMethodEntered();

            await LoadProject();
            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            var project = await _getProjectService.Execute(ProjectId, TaskName.StatutoryConsultation);
            SchoolName = project.SchoolName;

            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            try
            {
                var request = new UpdateProjectByTaskRequest()
                {
                    StatutoryConsultation = new StatutoryConsultationTask()
                    {
                        ExpectedDateForReceivingFindingsFromTrust = ExpectedDateForReceivingFindingsFromTrust,
                        ReceivedConsultationFindingsFromTrust = ReceivedConsultationFindingsFromTrust,
                        DateReceived = DateReceived,
                        ConsultationFulfilsTrustSection10StatutoryDuty = ConsultationFulfilsTrustSection10StatutoryDuty,
                        Comments = Comments,
                        SavedFindingsInWorkplacesFolder = SavedFindingsInWorkplacesFolder

                    }
                };

                await _updateProjectTaskService.Execute(ProjectId, request);

                return Redirect(string.Format(RouteConstants.ViewStatutoryConsultation, ProjectId));
            }
            catch (Exception ex)
            {
                _logger.LogErrorMsg(ex);
                throw;
            }
        }

        private async Task LoadProject()
        {
            var project = await _getProjectService.Execute(ProjectId, TaskName.StatutoryConsultation);

            ExpectedDateForReceivingFindingsFromTrust = project.StatutoryConsultation.ExpectedDateForReceivingFindingsFromTrust;
            ReceivedConsultationFindingsFromTrust = project.StatutoryConsultation.ReceivedConsultationFindingsFromTrust;
            DateReceived = project.StatutoryConsultation.DateReceived;
            ConsultationFulfilsTrustSection10StatutoryDuty = project.StatutoryConsultation.ConsultationFulfilsTrustSection10StatutoryDuty;
            Comments = project.StatutoryConsultation.Comments;
            SavedFindingsInWorkplacesFolder = project.StatutoryConsultation.SavedFindingsInWorkplacesFolder;


            SchoolName = project.SchoolName;
        }
    }
}