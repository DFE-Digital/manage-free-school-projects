﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Extensions;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Dashboard;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.RegionAndLA;

public class EditLocalAuthority : PageModel
{
    private readonly IGetLocalAuthoritiesService _getLocalAuthoritiesService;
    private readonly IGetProjectByTaskService _getProjectByTaskService;
    private readonly IUpdateProjectByTaskService _updateProjectByTaskService;
    private readonly ErrorService _errorService;

    [BindProperty(SupportsGet = true)] 
    public string ProjectId { get; set; }

    [BindProperty] 
    public List<string> LocalAuthorities { get; set; }

    [FromQuery(Name = "region")] 
    public string Region { get; set; }

    [BindProperty(Name = "local-authority")]
    [Required(ErrorMessage = "Select the local authority of the free school.")]
    public string LocalAuthority { get; set; }

    public GetProjectByTaskResponse Project { get; set; }

    public string CurrentFreeSchoolName { get; set; }

    public EditLocalAuthority(IGetLocalAuthoritiesService getLocalAuthoritiesService,
        IGetProjectByTaskService getProjectByTaskService, IUpdateProjectByTaskService updateProjectByTaskService,
        ErrorService errorService)
    {
        _getLocalAuthoritiesService = getLocalAuthoritiesService;
        _getProjectByTaskService = getProjectByTaskService;
        _updateProjectByTaskService = updateProjectByTaskService;
        _errorService = errorService;
    }

    public async Task<ActionResult> OnGet()
    {
        Project = await _getProjectByTaskService.Execute(ProjectId);

        CurrentFreeSchoolName = Project.School.CurrentFreeSchoolName;
        TempData["CurrentFreeSchoolName"] = CurrentFreeSchoolName;

        var localAuthoritiesAndCodes = await GetLocalAuthoritiesByRegion();

        LocalAuthorities = localAuthoritiesAndCodes.Values.ToList();
        LocalAuthority = Project.RegionAndLocalAuthority.LocalAuthority;

        return Page();
    }


    public async Task<ActionResult> OnPost()
    {
        IDictionary<string, string> localAuthorities;

        if (!ModelState.IsValid)
        {
            CurrentFreeSchoolName = TempData.Peek("CurrentFreeSchoolName") as string;

            localAuthorities = await GetLocalAuthoritiesByRegion();
            LocalAuthorities = localAuthorities.Values.ToList();

            _errorService.AddErrors(ModelState.Keys, ModelState);
            return Page();
        }

        localAuthorities = await GetLocalAuthoritiesByRegion();

        var localAuthorityCode = localAuthorities.SingleOrDefault(x => x.Value == LocalAuthority).Key;

        // var region = (ProjectRegion)Enum.Parse(typeof(ProjectRegion), Region);

        await _updateProjectByTaskService.Execute(ProjectId, new UpdateProjectByTaskRequest
        {
            RegionAndLocalAuthorityTask = new RegionAndLocalAuthorityTask
            {
                Region = Region,
                LocalAuthority = LocalAuthority,
                LocalAuthorityCode = localAuthorityCode
            }
        });

        return Redirect(string.Format(RouteConstants.ViewRegionAndLocalAuthorityTask, ProjectId));
    }

    private async Task<Dictionary<string, string>> GetLocalAuthoritiesByRegion()
    {
        // var projectRegion = (ProjectRegion)Enum.Parse(typeof(ProjectRegion), Region);

        var response = await _getLocalAuthoritiesService.Execute(new List<string> { Region });

        var authorities = new Dictionary<string, string>();

        response.LocalAuthorities.ForEach(authority => { authorities.Add(authority.LACode, authority.Name); });

        return authorities;
    }
}