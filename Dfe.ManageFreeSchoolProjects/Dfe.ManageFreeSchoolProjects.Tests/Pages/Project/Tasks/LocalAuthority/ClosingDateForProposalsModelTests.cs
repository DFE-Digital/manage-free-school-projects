using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Task;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.LocalAuthority.ClosingDateForProposals;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Tasks.LocalAuthority
{
    public class ClosingDateForProposalsModelTests
    {
        private const string ProjectId = "NEW-SCHOOL-1";
        private static readonly DateTime ClosingDate = new(2026, 5, 1);

        [Fact]
        public async Task OnGet_PopulatesTheSchoolNameAndDateFromTheProject()
        {
            var harness = new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School",
                NewSchoolClosingDateForProposals = new NewSchoolClosingDateForProposalsTask
                {
                    NewSchoolClosingDateForProposals = ClosingDate
                }
            });
            var model = BuildModel(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.CurrentFreeSchoolName.Should().Be("Test School");
            model.ClosingDateForProposals.Should().Be(ClosingDate);
            await harness.GetProjectService.Received(1)
                .Execute(ProjectId, TaskName.NewSchoolClosingDateForProposals);
        }

        [Fact]
        public async Task OnGet_WhenTheTaskHasNotBeenStarted_LeavesTheDateUnset()
        {
            var harness = new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School",
                NewSchoolClosingDateForProposals = null
            });
            var model = BuildModel(harness);

            await model.OnGet();

            model.CurrentFreeSchoolName.Should().Be("Test School");
            model.ClosingDateForProposals.Should().BeNull();
        }

        /// <summary>
        /// A failure fetching the project is swallowed so the user still gets an empty form rather
        /// than an error page.
        /// </summary>
        [Fact]
        public async Task OnGet_WhenTheProjectCannotBeFetched_StillReturnsThePage()
        {
            var harness = new NewSchoolTaskPageHarness().GetFailsWith(new InvalidOperationException("API down"));
            var model = BuildModel(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.ClosingDateForProposals.Should().BeNull();
        }

        [Fact]
        public async Task OnPost_WhenModelStateIsInvalid_ReturnsThePageWithoutUpdating()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.ModelState.AddModelError("closing-date-for-proposals", "Enter a valid date");

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            await harness.UpdateProjectTaskService.DidNotReceiveWithAnyArgs()
                .Execute(default, default);
        }

        [Fact]
        public async Task OnPost_WithADate_SendsTheDateAndCompletesTheTask()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.ClosingDateForProposals = ClosingDate;

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.TaskList, ProjectId));
            harness.CapturedRequest!.NewSchoolClosingDateForProposals.NewSchoolClosingDateForProposals
                .Should().Be(ClosingDate);
            harness.CapturedStatusRequest!.TaskName.Should().Be(TaskName.NewSchoolClosingDateForProposals.ToString());
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
            model.ClosingDateForProposals = null;

            await model.OnPost();

            harness.CapturedRequest!.NewSchoolClosingDateForProposals.NewSchoolClosingDateForProposals
                .Should().BeNull();
            harness.CapturedStatusRequest!.ProjectTaskStatus.Should().Be(ProjectTaskStatus.NotStarted);
        }

        [Fact]
        public async Task OnPost_WhenTheUpdateFails_Rethrows()
        {
            var harness = new NewSchoolTaskPageHarness().UpdateFailsWith(new InvalidOperationException("API down"));
            var model = BuildModel(harness);
            model.ClosingDateForProposals = ClosingDate;

            await model.Invoking(m => m.OnPost())
                .Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("API down");

            await harness.UpdateTaskStatusService.DidNotReceiveWithAnyArgs().Execute(default, default);
        }

        private static ClosingDateForProposalsModel BuildModel(NewSchoolTaskPageHarness harness)
        {
            return new ClosingDateForProposalsModel(
                harness.GetProjectService,
                harness.UpdateProjectTaskService,
                harness.UpdateTaskStatusService,
                Substitute.For<ILogger<ClosingDateForProposalsModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                PageContext = NewSchoolTaskPageHarness.BuildPageContext()
            };
        }
    }
}
