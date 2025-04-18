﻿using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Pagination;
using Dfe.ManageFreeSchoolProjects.Services.Dashboard;
using Dfe.ManageFreeSchoolProjects.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.FeatureManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.BuildFreeSchools.Pages;

namespace Dfe.ManageFreeSchoolProjects.Pages.Dashboard
{
    public class DashboardBasePageModel(
        ICreateUserService createUserService,
        IGetDashboardService getDashboardAllService,
        IGetLocalAuthoritiesService getLocalAuthoritiesService,
        IGetProjectManagersService getProjectManagersService,
        IFeatureManager featureManager,
        IDashboardFiltersCache dashboardFiltersCache) : PageModel
    {
        [BindProperty(SupportsGet = true)] 
        public int PageNumber { get; set; } = 1;

        [BindProperty(Name = "search-by-project", SupportsGet = true)]
        public string ProjectSearchTerm { get; set; }

        [BindProperty(Name = "search-by-region", SupportsGet = true)]
        public List<string> RegionSearchTerm { get; set; } = new();

        [BindProperty(Name = "search-by-local-authority", SupportsGet = true)]
        public List<string> LocalAuthoritySearchTerm { get; set; } = new();

        [BindProperty(Name = "search-by-pmb", SupportsGet = true)]
        public List<string> ProjectManagedBySearchTerm { get; set; }

        [BindProperty(Name = "search-by-project-status", SupportsGet = true)]
        public List<string> ProjectStatusSearchTerm { get; set; } = new();

        public DashboardModel Dashboard { get; set; } = new();

        protected readonly ICreateUserService CreateUserService = createUserService;
        protected readonly IGetDashboardService GetDashboardService = getDashboardAllService;

        public async Task<JsonResult> OnGetLocalAuthoritiesByRegion(string regions)
        {
            if (string.IsNullOrEmpty(regions))
                return new JsonResult(new List<string>());

            var regionsToSearch = regions.Split(",").ToList();

            var localAuthoritiesResponse = await getLocalAuthoritiesService.Execute(regionsToSearch);

            return new JsonResult(localAuthoritiesResponse.Regions);
        }

        protected async Task AddUser()
        {
            var username = User.Identity?.Name;
            await CreateUserService.Execute(username);
        }

        protected async Task LoadDashboard(LoadDashboardParameters loadDashboardParameters)
        {
            var getDashboardServiceParameters = loadDashboardParameters.GetDashboardServiceParameters;
            
            var allowCentralRoute = await featureManager.IsEnabledAsync("AllowCentralRoute");
            if (!allowCentralRoute)
                getDashboardServiceParameters.Wave = "FS - Presumption";

            var allowProjectStatusFilter = await featureManager.IsEnabledAsync("AllowProjectStatusFilter");
            ProjectStatusSearchTerm = allowProjectStatusFilter ? ProjectStatusSearchTerm : new List<string>();

            var filterCache = dashboardFiltersCache.Get();

            if (!string.IsNullOrWhiteSpace(ProjectSearchTerm)
                || RegionSearchTerm.Count != 0
                || LocalAuthoritySearchTerm.Count != 0
                || ProjectManagedBySearchTerm.Count != 0
                || ProjectStatusSearchTerm.Count != 0)
            {
                SetDashboardFilterCacheFromFilterInput(filterCache);
            }
            else
            {
                SetFilterFieldsFromFilterCache(filterCache);
            }

            SetDashboardParamsFromFields(getDashboardServiceParameters);

            var projectIds = await GetDashboardService.ExecuteProjectIdList(getDashboardServiceParameters);

            var projectManagersResponse = getProjectManagersService.Execute();

            var response = await GetDashboardService.Execute(getDashboardServiceParameters);

            var paginationModel = PaginationMapping.ToModel(response.Paging);
            var query = await BuildPaginationQuery();
            paginationModel.Url = $"{loadDashboardParameters.Url}{query}";

            Dashboard = new DashboardModel
            {
                Projects = response.Data.ToList(),
                ProjectSearchTerm = ProjectSearchTerm,
                RegionSearchTerm = RegionSearchTerm,
                LocalAuthoritySearchTerm = LocalAuthoritySearchTerm,
                ProjectManagedBySearchTerm = ProjectManagedBySearchTerm,
                ProjectStatusSearchTerm = ProjectStatusSearchTerm,
                Pagination = paginationModel,
                UserCanCreateProject = User.IsInRole(RolesConstants.ProjectRecordCreator),
                ProjectManagers = projectManagersResponse.Result.ProjectManagers,
                IsMyProjectsPage = loadDashboardParameters.Url.Contains("/my"),
                TotalProjectIds = projectIds,
            };
        }

        private async Task<string> BuildPaginationQuery()
        {
            var query = new QueryString("?handler=movePage");

            if (!string.IsNullOrEmpty(ProjectSearchTerm))
                query = query.Add("search-by-project", ProjectSearchTerm);

            if (RegionSearchTerm.Count != 0)
                RegionSearchTerm.ForEach(r => query = query.Add("search-by-region", r));

            if (LocalAuthoritySearchTerm.Count != 0)
                LocalAuthoritySearchTerm.ForEach(l => query = query.Add("search-by-local-authority", l));

            if (ProjectManagedBySearchTerm.Count > 0)
                ProjectManagedBySearchTerm.ForEach(m => query = query.Add("search-by-pmb", m));

            var allowProjectStatusFilter = await featureManager.IsEnabledAsync("AllowProjectStatusFilter");

            if (ProjectStatusSearchTerm.Count > 0 && allowProjectStatusFilter)
                ProjectStatusSearchTerm.ForEach(m => query = query.Add("search-by-project-status", m));
            
            return query.ToString();
        }

        private void SetDashboardParamsFromFields(GetDashboardServiceParameters getDashboardParams)
        {
            getDashboardParams.Project = ProjectSearchTerm;
            getDashboardParams.Regions = RegionSearchTerm;
            getDashboardParams.LocalAuthorities = LocalAuthoritySearchTerm;
            getDashboardParams.ProjectManagedBy = ProjectManagedBySearchTerm;
            getDashboardParams.ProjectStatus = ProjectStatusSearchTerm;
            getDashboardParams.Page = PageNumber;
        }

        private void SetDashboardFilterCacheFromFilterInput(DashboardFiltersCacheItem filterCache)
        {
            filterCache.ProjectManagedBySearchTerm = ProjectManagedBySearchTerm;
            filterCache.ProjectSearchTerm = ProjectSearchTerm;
            filterCache.RegionSearchTerm = RegionSearchTerm;
            filterCache.ProjectStatusSearchTerm = ProjectStatusSearchTerm;
            filterCache.LocalAuthoritySearchTerm = LocalAuthoritySearchTerm;

            dashboardFiltersCache.Update(filterCache);
        }

        private void SetFilterFieldsFromFilterCache(DashboardFiltersCacheItem filterCache)
        {
            ProjectSearchTerm = filterCache.ProjectSearchTerm ?? string.Empty;
            RegionSearchTerm = filterCache.RegionSearchTerm ?? [];
            LocalAuthoritySearchTerm = filterCache.LocalAuthoritySearchTerm ?? [];
            ProjectManagedBySearchTerm = filterCache.ProjectManagedBySearchTerm ?? [];
            ProjectStatusSearchTerm = filterCache.ProjectStatusSearchTerm ?? [];
        }

        protected class LoadDashboardParameters
        {
            public GetDashboardServiceParameters GetDashboardServiceParameters { get; set; }
            public string Url { get; set; }
        }
    }
}