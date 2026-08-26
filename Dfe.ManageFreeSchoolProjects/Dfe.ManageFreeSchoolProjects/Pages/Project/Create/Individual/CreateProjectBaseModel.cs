using Dfe.ManageFreeSchoolProjects.Constants;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Services.Project;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual
{
    public class CreateProjectBaseModel(ICreateProjectCache createProjectCache) : PageModel
    {
        protected internal string BackLink { get; set; }
        protected readonly ICreateProjectCache CreateProjectCache = createProjectCache;

        public bool IsUserAuthorised()
        {
            return User.IsInRole(RolesConstants.ProjectRecordCreator);
        }

        public string GetPreviousPage(CreateProjectPageName currentPageName, string routeParameter = "")
        {
            return currentPageName switch
            {
                CreateProjectPageName.LocalAuthority => RouteConstants.CreateProjectRegion,
                CreateProjectPageName.FaithType => RouteConstants.CreateFaithStatus,
                CreateProjectPageName.ProvisionalOpeningDate => PreviousProvisionalOpeningDate(),
                CreateProjectPageName.ConfirmTrustSearch => RouteConstants.CreateProjectSearchTrust,
                CreateProjectPageName.ClassType => RouteConstants.CreateProjectSchoolType,
                _ => DefaultPreviousRoute(currentPageName, routeParameter)
            };
        }

        private string PreviousProvisionalOpeningDate()
        {
            var cache = CreateProjectCache.Get();

            var faithStatus = cache.ReachedCheckYourAnswers ? cache.PreviousFaithStatus : cache.FaithStatus;

            if (faithStatus == API.Contracts.Project.Tasks.FaithStatus.None)
                return cache.ReachedCheckYourAnswers
                    ? RouteConstants.CreateProjectCheckYourAnswers
                    : RouteConstants.CreateFaithStatus;

            return RouteConstants.CreateFaithType;
        }

        private string DefaultPreviousRoute(CreateProjectPageName currentPageName, string routeParameter)
        {
            var cache = CreateProjectCache.Get();

            if (cache.ReachedCheckYourAnswers)
                return RouteConstants.CreateProjectCheckYourAnswers;

            return currentPageName switch
            {
                CreateProjectPageName.ProjectId => cache.ProjectType == ProjectType.CentralRoute
                    ? RouteConstants.CreateApplicationWave
                    : RouteConstants.CreateProjectMethod,
                CreateProjectPageName.ApplicationNumber => RouteConstants.CreateProjectMethod,
                CreateProjectPageName.ApplicationWave => RouteConstants.CreateApplicationNumber,
                CreateProjectPageName.SchoolName => RouteConstants.CreateProjectId,
                CreateProjectPageName.Region => RouteConstants.CreateProjectSchool,
                CreateProjectPageName.SearchTrust => RouteConstants.CreateProjectLocalAuthority,
                CreateProjectPageName.SchoolType => cache.ProjectType == ProjectType.NewSchool ? RouteConstants.CreateProjectLocalAuthority : string.Format(RouteConstants.CreateProjectConfirmTrust, routeParameter),
                CreateProjectPageName.ClassType => RouteConstants.CreateProjectSchoolType,
                CreateProjectPageName.SchoolPhase => RouteConstants.CreateClassType,
                CreateProjectPageName.AgeRange => RouteConstants.CreateProjectSchoolPhase,
                CreateProjectPageName.Capacity => RouteConstants.CreateProjectAgeRange,
                CreateProjectPageName.FaithStatus => RouteConstants.CreateProjectCapacity,
                CreateProjectPageName.ProjectAssignedTo => RouteConstants.CreateProjectProvisionalOpeningDate,
                CreateProjectPageName.CheckYourAnswers => RouteConstants.CreateProjectAssignedTo,
                _ => throw new ArgumentOutOfRangeException($"Unsupported create project page {currentPageName}")
            };
        }

        public string GetNextPage(CreateProjectPageName currentPageName, string routeParameter = "")
        {
            return currentPageName switch
            {
                CreateProjectPageName.FaithStatus => NextFaithStatus(),
                CreateProjectPageName.Region => RouteConstants.CreateProjectLocalAuthority,
                CreateProjectPageName.SearchTrust => string.Format(RouteConstants.CreateProjectConfirmTrust,
                    routeParameter),
                CreateProjectPageName.SchoolType => RouteConstants.CreateClassType,
                CreateProjectPageName.ApplicationWave => NextPageAfterApplicationWave(),
                CreateProjectPageName.ApplicationNumber => RouteConstants.CreateApplicationWave,
                _ => DefaultNextRoute(currentPageName)
            };
        }

        private string DefaultNextRoute(CreateProjectPageName currentPageName)
        {
            var cache = CreateProjectCache.Get();

            if (cache.ReachedCheckYourAnswers)
                return RouteConstants.CreateProjectCheckYourAnswers;

            return currentPageName switch
            {
                CreateProjectPageName.ProjectId => RouteConstants.CreateProjectSchool,
                CreateProjectPageName.SchoolName => RouteConstants.CreateProjectRegion,
                CreateProjectPageName.LocalAuthority => cache.ProjectType == ProjectType.NewSchool ? RouteConstants.CreateProjectSchoolType : RouteConstants.CreateProjectSearchTrust,
                CreateProjectPageName.ConfirmTrustSearch => RouteConstants.CreateProjectSchoolType,
                CreateProjectPageName.SchoolType => RouteConstants.CreateClassType,
                CreateProjectPageName.ClassType => RouteConstants.CreateProjectSchoolPhase,
                CreateProjectPageName.SchoolPhase => RouteConstants.CreateProjectAgeRange,
                CreateProjectPageName.AgeRange => RouteConstants.CreateProjectCapacity,
                CreateProjectPageName.Capacity => RouteConstants.CreateFaithStatus,
                CreateProjectPageName.FaithType => cache.ProjectType == ProjectType.NewSchool ? RouteConstants.CreateProjectAssignedTo : RouteConstants.CreateProjectProvisionalOpeningDate,
                CreateProjectPageName.ProvisionalOpeningDate => RouteConstants.CreateProjectAssignedTo,
                CreateProjectPageName.ProjectAssignedTo => RouteConstants.CreateProjectCheckYourAnswers,
                CreateProjectPageName.CheckYourAnswers => RouteConstants.CreateProjectConfirmation,
                _ => throw new ArgumentOutOfRangeException($"Unsupported create project page {currentPageName}")
            };
        }

        private string NextFaithStatus()
        {
            var cache = CreateProjectCache.Get();

            var faithStatus = cache.ReachedCheckYourAnswers ? cache.PreviousFaithStatus : cache.FaithStatus;

            if (faithStatus == API.Contracts.Project.Tasks.FaithStatus.None)
                return cache.ReachedCheckYourAnswers
                    ? RouteConstants.CreateProjectCheckYourAnswers
                    : RouteConstants.CreateProjectProvisionalOpeningDate;

            return RouteConstants.CreateFaithType;
        }

        private string NextPageAfterApplicationWave()
        {
            var cache = CreateProjectCache.Get();

            return cache.ProjectType == ProjectType.CentralRoute && cache.ReachedCheckYourAnswers
                ? RouteConstants.CreateProjectCheckYourAnswers
                : RouteConstants.CreateProjectId;
        }
    }
}