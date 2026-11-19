using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Task;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.School;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Services.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Tasks.School
{
    public class ViewSchoolTaskTests
    {
        private const string ProjectId = "FS-1";

        private readonly IGetProjectByTaskService _getProjectService = Substitute.For<IGetProjectByTaskService>();
        private readonly IGetProjectOverviewService _getProjectOverviewService = Substitute.For<IGetProjectOverviewService>();
        private readonly IGetTaskStatusService _getTaskStatusService = Substitute.For<IGetTaskStatusService>();
        private readonly IUpdateTaskStatusService _updateTaskStatusService = Substitute.For<IUpdateTaskStatusService>();

        public ViewSchoolTaskTests()
        {
            _getProjectService.Execute(Arg.Any<string>(), Arg.Any<TaskName>())
                .Returns(new GetProjectByTaskResponse
                {
                    School = new SchoolTask { EstablishmentType = EstablishmentType.AcademySponsorLed }
                });

            _getTaskStatusService.Execute(Arg.Any<string>(), Arg.Any<string>())
                .Returns(new TaskStatusResponse { ProjectTaskStatus = ProjectTaskStatus.InProgress });
        }

        /// <summary>
        /// The view only shows the establishment type for New School projects, so the page has to
        /// expose the project type it got from the overview.
        /// </summary>
        [Theory]
        [InlineData("New School")]
        [InlineData("Presumption")]
        [InlineData("Central Route")]
        public async Task OnGet_SetsTheProjectTypeFromTheProjectOverview(string projectType)
        {
            ReturnsProjectType(projectType);
            var model = BuildModel();

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.ProjectType.Should().Be(projectType);
            await _getProjectOverviewService.Received(1).Execute(ProjectId);
        }

        [Fact]
        public async Task OnGet_LoadsTheSchoolTaskAndItsStatus()
        {
            ReturnsProjectType("New School");
            var model = BuildModel();

            await model.OnGet();

            model.Project.School.EstablishmentType.Should().Be(EstablishmentType.AcademySponsorLed);
            model.ProjectTaskStatus.Should().Be(ProjectTaskStatus.InProgress);
            await _getProjectService.Received(1).Execute(ProjectId, TaskName.School);
            await _getTaskStatusService.Received(1).Execute(ProjectId, TaskName.School.ToString());
        }

        /// <summary>
        /// The project type only controls whether the establishment type is shown, so a failed
        /// overview lookup hides that row rather than failing the whole page.
        /// </summary>
        [Fact]
        public async Task OnGet_WhenTheOverviewCannotBeFetched_StillShowsTheTaskWithoutAProjectType()
        {
            _getProjectOverviewService.Execute(Arg.Any<string>())
                .ThrowsAsync(new InvalidOperationException("API down"));
            var model = BuildModel();

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.ProjectType.Should().BeNull();
            model.Project.School.EstablishmentType.Should().Be(EstablishmentType.AcademySponsorLed);
        }

        /// <summary>
        /// Posting only updates the task status, so it does not need the project type.
        /// </summary>
        [Theory]
        [InlineData(true, ProjectTaskStatus.Completed)]
        [InlineData(false, ProjectTaskStatus.InProgress)]
        public async Task OnPost_UpdatesTheTaskStatusAndRedirectsToTheTaskList(bool markAsCompleted, ProjectTaskStatus expectedStatus)
        {
            var model = BuildModel();
            model.MarkAsCompleted = markAsCompleted;

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.TaskList, ProjectId));
            await _updateTaskStatusService.Received(1).Execute(ProjectId, Arg.Is<UpdateTaskStatusRequest>(r =>
                r.TaskName == TaskName.School.ToString() && r.ProjectTaskStatus == expectedStatus));
            await _getProjectOverviewService.DidNotReceiveWithAnyArgs().Execute(default!);
        }

        private void ReturnsProjectType(string projectType)
        {
            _getProjectOverviewService.Execute(Arg.Any<string>())
                .Returns(new ProjectOverviewResponse { ProjectType = projectType });
        }

        private ViewSchoolTask BuildModel()
        {
            return new ViewSchoolTask(
                _getProjectService,
                _getProjectOverviewService,
                Substitute.For<ILogger<ViewSchoolTask>>(),
                _getTaskStatusService,
                _updateTaskStatusService)
            {
                ProjectId = ProjectId
            };
        }
    }
}
