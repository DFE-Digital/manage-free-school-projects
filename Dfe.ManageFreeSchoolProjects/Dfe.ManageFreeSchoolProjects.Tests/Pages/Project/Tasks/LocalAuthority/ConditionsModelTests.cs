using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Task;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.LocalAuthority.Conditions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Tasks.LocalAuthority
{
    public class ConditionsModelTests
    {
        private const string ProjectId = "NEW-SCHOOL-1";

        [Fact]
        public async Task OnGet_WhenConditionsWereApplied_SelectsYesAndShowsTheDescription()
        {
            var harness = new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School",
                NewSchoolConditions = new NewSchoolConditionsTask
                {
                    NewSchoolConditions = "Yes",
                    NewSchoolConditionsDescription = "Planning permission required"
                }
            });
            var model = BuildModel(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.CurrentFreeSchoolName.Should().Be("Test School");
            model.ConditionOption.Should().Be(YesNoOption.Yes);
            model.ConditionDescription.Should().Be("Planning permission required");
            await harness.GetProjectService.Received(1).Execute(ProjectId, TaskName.NewSchoolConditions);
        }

        /// <summary>
        /// Anything other than a stored "Yes" is treated as No, so a saved task always comes back
        /// with an option selected rather than a blank radio group.
        /// </summary>
        [Theory]
        [InlineData("No")]
        [InlineData("")]
        [InlineData(null)]
        public async Task OnGet_WhenConditionsWereNotApplied_SelectsNo(string? storedValue)
        {
            var harness = new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School",
                NewSchoolConditions = new NewSchoolConditionsTask { NewSchoolConditions = storedValue }
            });
            var model = BuildModel(harness);

            await model.OnGet();

            model.ConditionOption.Should().Be(YesNoOption.No);
        }

        [Fact]
        public async Task OnGet_WhenTheTaskHasNotBeenStarted_LeavesNothingSelected()
        {
            var harness = new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School",
                NewSchoolConditions = null
            });
            var model = BuildModel(harness);

            await model.OnGet();

            model.CurrentFreeSchoolName.Should().Be("Test School");
            model.ConditionOption.Should().BeNull();
            model.ConditionDescription.Should().BeNull();
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
            model.ConditionOption.Should().BeNull();
        }

        [Fact]
        public async Task OnPost_WhenModelStateIsInvalid_ReturnsThePageWithoutUpdating()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.ModelState.AddModelError(
                nameof(ConditionsModel.ConditionOption), "Please select an option to confirm the conditions applied");

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            await harness.UpdateProjectTaskService.DidNotReceiveWithAnyArgs().Execute(default, default);
        }

        [Fact]
        public async Task OnPost_WhenYesIsSelected_SendsTheDescriptionAndCompletesTheTask()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.ConditionOption = YesNoOption.Yes;
            model.ConditionDescription = "Planning permission required";

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.TaskList, ProjectId));
            harness.CapturedRequest!.NewSchoolConditions.NewSchoolConditions.Should().Be("Yes");
            harness.CapturedRequest!.NewSchoolConditions.NewSchoolConditionsDescription
                .Should().Be("Planning permission required");
            harness.CapturedStatusRequest!.TaskName.Should().Be(TaskName.NewSchoolConditions.ToString());
            harness.CapturedStatusRequest!.ProjectTaskStatus.Should().Be(ProjectTaskStatus.Completed);
        }

        /// <summary>
        /// Switching from Yes to No has to clear any description the user had already typed, so a
        /// stale condition is not left on the project.
        /// </summary>
        [Fact]
        public async Task OnPost_WhenNoIsSelected_ClearsTheDescription()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.ConditionOption = YesNoOption.No;
            model.ConditionDescription = "Typed before switching to No";

            await model.OnPost();

            harness.CapturedRequest!.NewSchoolConditions.NewSchoolConditions.Should().Be("No");
            harness.CapturedRequest!.NewSchoolConditions.NewSchoolConditionsDescription.Should().BeEmpty();
            harness.CapturedStatusRequest!.ProjectTaskStatus.Should().Be(ProjectTaskStatus.Completed);
        }

        [Fact]
        public async Task OnPost_WhenNoOptionIsSelected_MarksTheTaskNotStarted()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.ConditionOption = null;

            await model.OnPost();

            harness.CapturedRequest!.NewSchoolConditions.NewSchoolConditions.Should().Be("No");
            harness.CapturedStatusRequest!.ProjectTaskStatus.Should().Be(ProjectTaskStatus.NotStarted);
        }

        [Fact]
        public async Task OnPost_WhenTheUpdateFails_Rethrows()
        {
            var harness = new NewSchoolTaskPageHarness().UpdateFailsWith(new InvalidOperationException("API down"));
            var model = BuildModel(harness);
            model.ConditionOption = YesNoOption.Yes;

            await model.Invoking(m => m.OnPost())
                .Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("API down");

            await harness.UpdateTaskStatusService.DidNotReceiveWithAnyArgs().Execute(default, default);
        }

        private static ConditionsModel BuildModel(NewSchoolTaskPageHarness harness)
        {
            return new ConditionsModel(
                harness.GetProjectService,
                harness.UpdateProjectTaskService,
                harness.UpdateTaskStatusService,
                Substitute.For<ILogger<ConditionsModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                PageContext = NewSchoolTaskPageHarness.BuildPageContext()
            };
        }
    }
}
