using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.SiteInformation;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.SiteInformation
{
    public class ViewSiteInformationModelTests
    {
        private const string ProjectId = "FS-1";

        private readonly IGetProjectSitesService _getProjectSitesService = Substitute.For<IGetProjectSitesService>();
        private readonly IGetProjectOverviewService _getProjectOverviewService = Substitute.For<IGetProjectOverviewService>();

        [Theory]
        [InlineData(SiteInformationWave.NewSchool, "New School")]
        [InlineData(SiteInformationWave.Presumption, "Presumption")]
        public async Task OnGet_LoadsTheSitesForTheWaveAndTheProjectDetails(string wave, string projectType)
        {
            var sites = new Fixture().Create<GetProjectSitesResponse>();
            _getProjectSitesService.Execute(ProjectId, wave).Returns(sites);
            _getProjectOverviewService.Execute(ProjectId).Returns(new ProjectOverviewResponse
            {
                ProjectType = projectType,
                ProjectStatus = new ProjectStatusResponse
                {
                    CurrentFreeSchoolName = "Test School",
                    ProjectStatus = ProjectStatus.Preopening
                }
            });

            var model = new ViewSiteInformationModel(
                _getProjectSitesService,
                _getProjectOverviewService,
                Substitute.For<ILogger<ViewSiteInformationModel>>())
            {
                ProjectId = ProjectId,
                Wave = wave
            };

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.SiteInformation.Should().BeSameAs(sites);
            model.SchoolName.Should().Be("Test School");
            model.ProjectStatus.Should().Be(ProjectStatus.Preopening);
            model.ProjectType.Should().Be(projectType);
        }
    }
}
