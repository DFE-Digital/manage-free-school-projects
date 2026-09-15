using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Task;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.LocalAuthority.DecisionMaker;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Tasks.LocalAuthority
{
    public class DecisionMakerModelTests
    {
        private const string ProjectId = "NEW-SCHOOL-1";
        private const string RegionalDirector = "Regional Director on behalf of the Secretary of State";

        [Fact]
        public void Options_OffersTheTwoDecisionMakers()
        {
            var model = BuildModel(new NewSchoolTaskPageHarness());

            model.Options.Should().Equal("Local authority", RegionalDirector);
        }

        [Fact]
        public async Task OnGet_PopulatesTheSchoolNameAndDecisionMakerFromTheProject()
        {
            var harness = new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School",
                NewSchoolDecisionMaker = new NewSchoolDecisionMakerTask
                {
                    NewSchoolDecisionMaker = "Local authority"
                }
            });
            var model = BuildModel(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.CurrentFreeSchoolName.Should().Be("Test School");
            model.DecisionMaker.Should().Be("Local authority");
            await harness.GetProjectService.Received(1).Execute(ProjectId, TaskName.NewSchoolDecisionMaker);
        }

        [Fact]
        public async Task OnGet_WhenTheTaskHasNotBeenStarted_LeavesTheDecisionMakerUnset()
        {
            var harness = new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School",
                NewSchoolDecisionMaker = null
            });
            var model = BuildModel(harness);

            await model.OnGet();

            model.CurrentFreeSchoolName.Should().Be("Test School");
            model.DecisionMaker.Should().BeNull();
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
            model.DecisionMaker.Should().BeNull();
        }

        [Fact]
        public async Task OnPost_WhenModelStateIsInvalid_ReturnsThePageWithoutUpdating()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.ModelState.AddModelError("decision-maker", "Select a decision maker");

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            await harness.UpdateProjectTaskService.DidNotReceiveWithAnyArgs().Execute(default, default);
        }

        [Fact]
        public async Task OnPost_WithADecisionMaker_SendsItAndCompletesTheTask()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.DecisionMaker = RegionalDirector;

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.TaskList, ProjectId));
            harness.CapturedRequest!.NewSchoolDecisionMaker.NewSchoolDecisionMaker.Should().Be(RegionalDirector);
            harness.CapturedStatusRequest!.TaskName.Should().Be(TaskName.NewSchoolDecisionMaker.ToString());
            harness.CapturedStatusRequest!.ProjectTaskStatus.Should().Be(ProjectTaskStatus.Completed);
        }

        /// <summary>
        /// Clearing the answer is how a user undoes the task, so the status has to drop back to
        /// not started rather than stay completed.
        /// </summary>
        [Fact]
        public async Task OnPost_WithoutADecisionMaker_MarksTheTaskNotStarted()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.DecisionMaker = null;

            await model.OnPost();

            harness.CapturedRequest!.NewSchoolDecisionMaker.NewSchoolDecisionMaker.Should().BeNull();
            harness.CapturedStatusRequest!.ProjectTaskStatus.Should().Be(ProjectTaskStatus.NotStarted);
        }

        [Fact]
        public async Task OnPost_WhenTheUpdateFails_Rethrows()
        {
            var harness = new NewSchoolTaskPageHarness().UpdateFailsWith(new InvalidOperationException("API down"));
            var model = BuildModel(harness);
            model.DecisionMaker = "Local authority";

            await model.Invoking(m => m.OnPost())
                .Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("API down");

            await harness.UpdateTaskStatusService.DidNotReceiveWithAnyArgs().Execute(default, default);
        }

        private static DecisionMakerModel BuildModel(NewSchoolTaskPageHarness harness)
        {
            return new DecisionMakerModel(
                harness.GetProjectService,
                harness.UpdateProjectTaskService,
                harness.UpdateTaskStatusService,
                Substitute.For<ILogger<DecisionMakerModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                PageContext = NewSchoolTaskPageHarness.BuildPageContext()
            };
        }
    }
}
