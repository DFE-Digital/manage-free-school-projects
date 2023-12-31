﻿using Dfe.ManageFreeSchoolProjects.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using Dfe.ManageFreeSchoolProjects.Services.Project;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual
{
    public class CreateProjectBaseModel : PageModel
    {
        protected internal string BackLink { get; set; }

        public bool IsUserAuthorised()
        {
            return User.IsInRole(RolesConstants.ProjectRecordCreator);
        }

        public string GetPreviousPage(CreateProjectPageName currentPageName, CreateProjectNavigation navigationCache,
            string routeParameter = "")
        {
            if (navigationCache == CreateProjectNavigation.BackToCheckYourAnswers)
                return RouteConstants.CreateProjectCheckYourAnswers;

            var faithTypeOrFaithStatusRoute = navigationCache == CreateProjectNavigation.GoToFaithType
                ? RouteConstants.CreateFaithType
                : RouteConstants.CreateFaithStatus;

            return currentPageName switch
            {
                CreateProjectPageName.ProjectId => RouteConstants.CreateProjectMethod,
                CreateProjectPageName.SchoolName => RouteConstants.CreateProjectId,
                CreateProjectPageName.Region => RouteConstants.CreateProjectSchool,
                CreateProjectPageName.LocalAuthority => RouteConstants.CreateProjectRegion,
                CreateProjectPageName.SearchTrust => RouteConstants.CreateProjectLocalAuthority,
                CreateProjectPageName.ConfirmTrustSearch => RouteConstants.CreateProjectSearchTrust,
                CreateProjectPageName.SchoolType => string.Format(RouteConstants.CreateProjectConfirmTrust,
                    routeParameter),
                CreateProjectPageName.SchoolPhase => RouteConstants.CreateProjectSchoolType,
                CreateProjectPageName.ClassType => RouteConstants.CreateProjectSchoolPhase,
                CreateProjectPageName.AgeRange => RouteConstants.CreateClassType,
                CreateProjectPageName.Capacity => RouteConstants.CreateProjectAgeRange,
                CreateProjectPageName.FormsOfEntry => RouteConstants.CreateProjectCapacity,
                CreateProjectPageName.FaithStatus => RouteConstants.CreateFormsOfEntry,
                CreateProjectPageName.FaithType => RouteConstants.CreateFaithStatus,
                CreateProjectPageName.ProvisionalOpeningDate => faithTypeOrFaithStatusRoute,
                CreateProjectPageName.NotifyUser => RouteConstants.CreateProjectProvisionalOpeningDate,
                CreateProjectPageName.CheckYourAnswers => RouteConstants.CreateNotifyUser,
                _ => throw new ArgumentOutOfRangeException($"Unsupported create project page {currentPageName}")
            };
        }

        public string GetNextPage(CreateProjectPageName currentPageName,
            CreateProjectNavigation navigationCache = CreateProjectNavigation.Default, string routeParameter = "")
        {
            var faithTypeOrFaithStatusRoute = navigationCache == CreateProjectNavigation.GoToFaithType
                ? RouteConstants.CreateFaithType
                : RouteConstants.CreateProjectProvisionalOpeningDate;

            return currentPageName switch
            {
                CreateProjectPageName.ProjectId => RouteConstants.CreateProjectSchool,
                CreateProjectPageName.SchoolName => RouteConstants.CreateProjectRegion,
                CreateProjectPageName.Region => RouteConstants.CreateProjectLocalAuthority,
                CreateProjectPageName.LocalAuthority => RouteConstants.CreateProjectSearchTrust,
                CreateProjectPageName.SearchTrust => string.Format(RouteConstants.CreateProjectConfirmTrust,
                    routeParameter),
                CreateProjectPageName.ConfirmTrustSearch => RouteConstants.CreateProjectSchoolType,
                CreateProjectPageName.SchoolType => RouteConstants.CreateProjectSchoolPhase,
                CreateProjectPageName.SchoolPhase => RouteConstants.CreateClassType,
                CreateProjectPageName.ClassType => RouteConstants.CreateProjectAgeRange,
                CreateProjectPageName.AgeRange => RouteConstants.CreateProjectCapacity,
                CreateProjectPageName.Capacity => RouteConstants.CreateFormsOfEntry,
                CreateProjectPageName.FormsOfEntry => RouteConstants.CreateFaithStatus,
                CreateProjectPageName.FaithStatus => faithTypeOrFaithStatusRoute,
                CreateProjectPageName.FaithType => RouteConstants.CreateProjectProvisionalOpeningDate,
                CreateProjectPageName.ProvisionalOpeningDate => RouteConstants.CreateNotifyUser,
                CreateProjectPageName.NotifyUser => RouteConstants.CreateProjectCheckYourAnswers,
                CreateProjectPageName.CheckYourAnswers => RouteConstants.CreateProjectConfirmation,
                _ => throw new ArgumentOutOfRangeException($"Unsupported create project page {currentPageName}")
            };
        }
    }
}