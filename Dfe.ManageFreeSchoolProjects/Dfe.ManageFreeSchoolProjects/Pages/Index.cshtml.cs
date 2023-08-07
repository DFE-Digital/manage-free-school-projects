﻿using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels.Project;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.BuildFreeSchools.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public ProjectResponse[] Projects { get; set; }

        [BindProperty]
        public bool UserCanCreateProject { get; set; }

        [BindProperty]
        public string Username { get; set; }

        private IGetProjectsByUserService _getProjectsByUserService { get; set; }

        public IndexModel(IGetProjectsByUserService getProjectsByUserService)
        {
            _getProjectsByUserService = getProjectsByUserService;
        }
		public async Task OnGetAsync()
		{
			//_logger.LogInformation("Case::DetailsPageModel::OnGetAsync");

			// Fetch UI data
			await LoadPage();
		}

		private async Task<ActionResult> LoadPage()
		{
			try
			{
				Projects = await _getProjectsByUserService.GetProjects(User.Identity.Name.ToString());
				UserCanCreateProject = User.IsInRole("teamlead");
				Username = User.Identity.Name.ToString();
				return Page();
			}
			catch (Exception ex)
			{
				//_logger.LogError("Case::DetailsPageModel::LoadPage::Exception - {Message}", ex.Message);

				//TempData["Error.Message"] = ErrorOnGetPage;
				return Page();
			}
		}
	}
}