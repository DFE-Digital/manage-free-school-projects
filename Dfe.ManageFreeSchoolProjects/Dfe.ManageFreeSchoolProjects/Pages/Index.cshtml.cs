﻿using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Pages.Dashboard;
using Dfe.ManageFreeSchoolProjects.Services.Dashboard;
using Dfe.ManageFreeSchoolProjects.Services.Reports;
using Dfe.ManageFreeSchoolProjects.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Dashboard;
using Microsoft.FeatureManagement;

namespace Dfe.BuildFreeSchools.Pages
{
	public class IndexModel : DashboardBasePageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly IAllProjectsReportService _allProjectsReportService;
		
		public IndexModel(
			IGetDashboardService getDashboardService,
			ICreateUserService createUserService,
			IGetLocalAuthoritiesService getLocalAuthoritiesService,
			IGetProjectManagersService getProjectManagersService,
			IAllProjectsReportService allProjectsReportService,
            IFeatureManager featureManager,
            ILogger<IndexModel> logger) : base(createUserService, getDashboardService, getLocalAuthoritiesService,
			getProjectManagersService, featureManager)
		{
			_logger = logger;
			_allProjectsReportService = allProjectsReportService;
		}

		public async Task<IActionResult> OnGetAsync()
		{
			_logger.LogMethodEntered();

			try
			{
				await AddUser();
				await LoadPage();
			}
			catch (Exception ex)
			{
				_logger.LogErrorMsg(ex);
				throw;
			}

			return Page();
		}

		public async Task<IActionResult> OnGetMovePage()
		{
			_logger.LogMethodEntered();

			try
			{
				await LoadPage();
			}
			catch (Exception ex)
			{
				_logger.LogErrorMsg(ex);
				throw;
			}

			return Page();
		}

		public async Task<IActionResult> OnPostSearch()
		{
			_logger.LogMethodEntered();

			try
			{
				await LoadPage();
			}
			catch (Exception ex)
			{
				_logger.LogErrorMsg(ex);
				throw;
			}

			return Page();
		}

		public async Task<IActionResult> OnGetClearFilters()
		{
			_logger.LogMethodEntered();
			try
			{
				await LoadPage();
			}
			catch (Exception ex)
			{
				_logger.LogErrorMsg(ex);
				throw;
			}

			return Page();
		}

		public async Task<IActionResult> OnGetDownloadFile()
		{
			_logger.LogMethodEntered();
			try
			{
				var now = DateTime.Now.Date.ToString("yyyy-MM-dd");
				var fileName = $"{now}-mfsp-all-projects-export.xlsx";

				var stream = await _allProjectsReportService.Execute();
				return File(stream, "application/octet-stream", fileName);
			}
			catch (Exception ex)
			{
				_logger.LogErrorMsg(ex);
				throw;
			}
		}
		
		public async Task<IActionResult>OnGetDownloadFilteredFile(string projectIds)
		{
			_logger.LogMethodEntered();
			try
			{
				var now = DateTime.Now.Date.ToString("yyyy-MM-dd");
				var fileName = $"{now}-mfsp-filtered-projects-export.xlsx";

				var stream = await _allProjectsReportService.ExecuteWithFilter(projectIds);
				return File(stream, "application/octet-stream", fileName);
			}
			catch (Exception ex)
			{
				_logger.LogErrorMsg(ex);
				throw;
			}
		}
		
		protected async Task LoadPage()
		{
			var parameters = new LoadDashboardParameters()
			{
				GetDashboardServiceParameters = new GetDashboardServiceParameters(),
				Url = string.Empty
			};

			await LoadDashboard(parameters);

			Dashboard.Header = "All projects";
		}
	}
}