using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Task;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.LocalAuthority.DateOfDecision;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Tasks.LocalAuthority
{
    public class DateOfDecisionModelTests
    {
        private const string ProjectId = "NEW-SCHOOL-1";
        private static readonly DateTime DecisionDate = new(2026, 6, 12);

        [Fact]
        public async Task OnGet_PopulatesTheSchoolNameAndDateFromTheProject()
        {
            var harness = new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School",
                NewSchoolDateOfDecision = new NewSchoolDateOfDecisionTask
                {
                    NewSchoolDateOfDecision = DecisionDate
                }
            });
            var model = BuildModel(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.CurrentFreeSchoolName.Should().Be("Test School");
            model.DateOfDecision.Should().Be(DecisionDate);
            await harness.GetProjectService.Received(1)
                .Execute(ProjectId, TaskName.NewSchoolDateOfDecision);
        }

        [Fact]
        public async Task OnGet_WhenTheTaskHasNotBeenStarted_LeavesTheDateUnset()
        {
            var harness = new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School",
                NewSchoolDateOfDecision = null
            });
            var model = BuildModel(harness);

            await model.OnGet();

            model.CurrentFreeSchoolName.Should().Be("Test School");
            model.DateOfDecision.Should().BeNull();
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
            model.DateOfDecision.Should().BeNull();
        }

        [Fact]
        public async Task OnPost_WhenModelStateIsInvalid_ReturnsThePageWithoutUpdating()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.ModelState.AddModelError("date-of-decision", "Enter a valid date");

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
            model.DateOfDecision = DecisionDate;

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.TaskList, ProjectId));
            harness.CapturedRequest!.NewSchoolDateOfDecision.NewSchoolDateOfDecision.Should().Be(DecisionDate);
            harness.CapturedStatusRequest!.TaskName.Should().Be(TaskName.NewSchoolDateOfDecision.ToString());
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
            model.DateOfDecision = null;

            await model.OnPost();

            harness.CapturedRequest!.NewSchoolDateOfDecision.NewSchoolDateOfDecision.Should().BeNull();
            harness.CapturedStatusRequest!.ProjectTaskStatus.Should().Be(ProjectTaskStatus.NotStarted);
        }

        [Fact]
        public async Task OnPost_WhenTheUpdateFails_Rethrows()
        {
            var harness = new NewSchoolTaskPageHarness().UpdateFailsWith(new InvalidOperationException("API down"));
            var model = BuildModel(harness);
            model.DateOfDecision = DecisionDate;

            await model.Invoking(m => m.OnPost())
                .Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("API down");

            await harness.UpdateTaskStatusService.DidNotReceiveWithAnyArgs().Execute(default, default);
        }

        private static DateOfDecisionModel BuildModel(NewSchoolTaskPageHarness harness)
        {
            return new DateOfDecisionModel(
                harness.GetProjectService,
                harness.UpdateProjectTaskService,
                harness.UpdateTaskStatusService,
                Substitute.For<ILogger<DateOfDecisionModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                PageContext = NewSchoolTaskPageHarness.BuildPageContext()
            };
        }
    }
}
