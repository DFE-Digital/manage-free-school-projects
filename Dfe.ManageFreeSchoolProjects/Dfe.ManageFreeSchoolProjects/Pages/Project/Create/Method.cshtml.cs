using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Create
{
    public class MethodModel(ErrorService errorService, ICreateProjectCache createProjectCache)
        : CreateProjectBaseModel(createProjectCache)
    {
        [BindProperty(Name = "method")]
        [Display(Name = "method")]
        [Required(ErrorMessage = "Select what project you want to create")]
        public ProjectType Method { get; set; }

        public IActionResult OnGet()
        {
            if (!User.IsInRole(RolesConstants.ProjectRecordCreator))
            {
                return new UnauthorizedResult();
            }

            Method = CreateProjectCache.Get().ProjectType;

            // we reset session
            CreateProjectCache.Delete();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            var projCache = CreateProjectCache.Get();

            var nextPage = GetNextPage(projCache, Method);

            UpdateCacheWithCreateMethod(Method, projCache);

            return nextPage;
        }

        private RedirectResult GetNextPage(CreateProjectCacheItem projCache, ProjectType chosenMethod)
        {
            var hasApplicationWaveOrApplicationNumber = !string.IsNullOrEmpty(projCache.ApplicationWave) ||
                                                        !string.IsNullOrEmpty(projCache.ApplicationNumber);

            switch (chosenMethod)
            {
                case ProjectType.PresumptionRoute:
                    ClearCentralRouteFields(projCache);

                    return Redirect(projCache.ReachedCheckYourAnswers
                        ? RouteConstants.CreateProjectCheckYourAnswers
                        : RouteConstants.CreateProjectId);

                case ProjectType.CentralRoute:
                    return Redirect(projCache.ReachedCheckYourAnswers && hasApplicationWaveOrApplicationNumber
                        ? RouteConstants.CreateProjectCheckYourAnswers
                        : RouteConstants.CreateApplicationNumber);
                case ProjectType.NewSchool:
                    return Redirect(projCache.ReachedCheckYourAnswers
                        ? RouteConstants.CreateProjectCheckYourAnswers
                        : RouteConstants.CreateProjectId);
                default:
                    throw new InvalidOperationException($"Unrecognized method {chosenMethod}");
            }
        }

        private void ClearCentralRouteFields(CreateProjectCacheItem projCache)
        {
            projCache.ApplicationNumber = null;
            projCache.ApplicationWave = null;
            CreateProjectCache.Update(projCache);
        }

        private void UpdateCacheWithCreateMethod(ProjectType method, CreateProjectCacheItem projCache)
        {
            projCache.ProjectType = method;
            CreateProjectCache.Update(projCache);
        }
    }
}