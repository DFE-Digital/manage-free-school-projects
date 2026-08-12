using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Projects;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Extensions;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual
{
    public class CheckYourAnswersModel(
        ErrorService errorService,
        ICreateProjectCache createProjectCache,
        ICreateProjectService createProjectService,
        INotifyUserService notifyUserService)
        : PageModel
    {
        public CreateProjectCacheItem Project { get; set; }

        public bool IsLocalAuthority { get; private set; }

        public bool IsMainStream { get; private set; }

        public IActionResult OnGet()
        {
            if (!User.IsInRole(RolesConstants.ProjectRecordCreator))
            {
                return new UnauthorizedResult();
            }

            Project = createProjectCache.Get();

            Project.ReachedCheckYourAnswers = true;

            IsLocalAuthority = Project.ProjectType == ProjectType.LocalAuthority;
            IsMainStream = Project.SchoolType == SchoolType.Mainstream;

            createProjectCache.Update(Project);

            return Page();
        }

        private string GenerateApplicationWave(ProjectType type)
        {
            switch(type)
            {
                case ProjectType.LocalAuthority:
                    return "LocalAuthority";
                case ProjectType.PresumptionRoute:
                    return "FS - Presumption";
                default:
                    return string.Empty;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var createProjectRequest = new CreateProjectRequest();
            var project = createProjectCache.Get();

            var projReq = new ProjectDetails
            {
                ProjectId = project.ProjectId,
                SchoolName = project.SchoolName,
                SchoolType = project.SchoolType,
                SchoolPhase = project.SchoolPhase,
                CreatedBy = User.Identity?.Name,
                LocalAuthority = project.LocalAuthority,
                LocalAuthorityCode = project.LocalAuthorityCode,
                Region = project.Region.ToDescription(),
                TRN = project.TRN,
                TrustName = project.TrustName,
                AgeRange = project.AgeRange,
                NurseryCapacity = (int)project.NurseryCapacity,
                YRY6Capacity = (int)project.YRY6Capacity,
                Y7Y11Capacity = (int)project.Y7Y11Capacity,
                Y12Y14Capacity = (int)project.Y12Y14Capacity,
                APResourcesProvision = project.APResourcesProvision ?? 0,
                SENResourcedProvisionSENUnit = project.SENResourcedProvisionSENUnit ?? 0,
                Nursery = project.Nursery,
                SixthForm = project.SixthForm,
                AlternativeProvision = project.AlternativeProvision,
                SpecialEducationNeeds = project.SpecialEducationNeeds,
                ResidentialOrBoarding = project.ResidentialOrBoarding,
                FormsOfEntry = project.FormsOfEntry,
                FaithStatus = project.FaithStatus,
                FaithType = project.FaithType,
                OtherFaithType = project.OtherFaithType,
                ProvisionalOpeningDate = project.ProvisionalOpeningDate,
                ProjectAssignedToName = project.ProjectAssignedToName,
                ProjectAssignedToEmail = project.ProjectAssignedToEmail,
                ApplicationNumber = project.ApplicationNumber ?? string.Empty,
                ApplicationWave = GenerateApplicationWave(project.ProjectType) ?? project.ApplicationWave
            };
            
            createProjectRequest.Projects.Add(projReq);

            try
            {
                await createProjectService.Execute(createProjectRequest);

                await SendEmail();
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == HttpStatusCode.UnprocessableEntity)
                {
                    errorService.AddError("projectid", $"Project with ID {project.ProjectId} already exists");
                    Project = project;

                    IsLocalAuthority = Project.ProjectType == ProjectType.LocalAuthority;
                    IsMainStream = Project.SchoolType == SchoolType.Mainstream;

                    return Page();
                }

                if (e.StatusCode == HttpStatusCode.InternalServerError)
                    return Redirect(RouteConstants.CreateProjectConfirmation);

                throw;
            }

            return Redirect(RouteConstants.CreateProjectConfirmation);
        }

        private async Task SendEmail()
        {
            var projectUrl =
                $"{HttpContext.Request.Scheme}://" +
                $"{HttpContext.Request.Host}" +
                $"{string.Format(RouteConstants.ProjectOverview, createProjectCache.Get().ProjectId)}";

            await notifyUserService.Execute(createProjectCache.Get().ProjectAssignedToEmail, projectUrl);
        }
    }
}