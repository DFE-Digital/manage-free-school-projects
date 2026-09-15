using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Task;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.LocalAuthority.SpecificationPublicationDate;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Tasks.LocalAuthority
{
    public class SpecificationPublicationDateModelTests
    {
        private const string ProjectId = "NEW-SCHOOL-1";
        private static readonly DateTime PublicationDate = new(2026, 3, 20);

        [Fact]
        public async Task OnGet_PopulatesTheSchoolNameAndDateFromTheProject()
        {
            var harness = new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School",
                NewSchoolSpecificationPublicationDate = new NewSchoolSpecificationPublicationDateTask
                {
                    NewSchoolSpecificationPublicationDate = PublicationDate
                }
            });
            var model = BuildModel(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.CurrentFreeSchoolName.Should().Be("Test School");
            model.SpecificationPublicationDate.Should().Be(PublicationDate);
            await harness.GetProjectService.Received(1)
                .Execute(ProjectId, TaskName.NewSchoolSpecificationPublicationDate);
        }

        [Fact]
        public async Task OnGet_WhenTheTaskHasNotBeenStarted_LeavesTheDateUnset()
        {
            var harness = new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School",
                NewSchoolSpecificationPublicationDate = null
            });
            var model = BuildModel(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.CurrentFreeSchoolName.Should().Be("Test School");
            model.SpecificationPublicationDate.Should().BeNull();
        }

        [Fact]
        public async Task OnGet_WhenTheProjectCannotBeFetched_StillReturnsThePage()
        {
            var harness = new NewSchoolTaskPageHarness().GetFailsWith(new InvalidOperationException("API down"));
            var model = BuildModel(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.SpecificationPublicationDate.Should().BeNull();
        }

        [Fact]
        public async Task OnPost_WhenModelStateIsInvalid_ReturnsThePageWithoutUpdating()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.ModelState.AddModelError("specification-publication-date", "Enter a valid date");

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            await harness.UpdateProjectTaskService.DidNotReceiveWithAnyArgs().Execute(default, default);
        }

        [Fact]
        public async Task OnPost_WithADate_SendsTheDateAndCompletesTheTask()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.SpecificationPublicationDate = PublicationDate;

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.TaskList, ProjectId));
            harness.CapturedRequest!.NewSchoolSpecificationPublicationDate.NewSchoolSpecificationPublicationDate
                .Should().Be(PublicationDate);
            harness.CapturedStatusRequest!.TaskName
                .Should().Be(TaskName.NewSchoolSpecificationPublicationDate.ToString());
            harness.CapturedStatusRequest!.ProjectTaskStatus.Should().Be(ProjectTaskStatus.Completed);
        }

        /// <summary>
        /// Clearing the date is how a user undoes the task, so the status has to drop back to
        /// not started rather than stay completed.
        /// </summary>
        [Fact]
        public async Task OnPost_WithoutADate_MarksTheTaskNotStarted()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.SpecificationPublicationDate = null;

            await model.OnPost();

            harness.CapturedRequest!.NewSchoolSpecificationPublicationDate.NewSchoolSpecificationPublicationDate
                .Should().BeNull();
            harness.CapturedStatusRequest!.ProjectTaskStatus.Should().Be(ProjectTaskStatus.NotStarted);
        }

        [Fact]
        public async Task OnPost_WhenTheUpdateFails_Rethrows()
        {
            var harness = new NewSchoolTaskPageHarness().UpdateFailsWith(new InvalidOperationException("API down"));
            var model = BuildModel(harness);
            model.SpecificationPublicationDate = PublicationDate;

            await model.Invoking(m => m.OnPost())
                .Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("API down");

            await harness.UpdateTaskStatusService.DidNotReceiveWithAnyArgs().Execute(default, default);
        }

        private static SpecificationPublicationDateModel BuildModel(NewSchoolTaskPageHarness harness)
        {
            return new SpecificationPublicationDateModel(
                harness.GetProjectService,
                harness.UpdateProjectTaskService,
                harness.UpdateTaskStatusService,
                Substitute.For<ILogger<SpecificationPublicationDateModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                PageContext = NewSchoolTaskPageHarness.BuildPageContext()
            };
        }
    }
}
