using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.LocalAuthority.Decision;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Tasks.LocalAuthority
{
    public class DecisionModelTests
    {
        private const string ProjectId = "NEW-SCHOOL-1";
        private const string ProposalId = "RID-1";

        /// <summary>
        /// The radio values are rendered from these constants, so they are also the strings stored
        /// against the project and matched on when deciding whether to keep the conditions.
        /// </summary>
        [Fact]
        public void TheApprovalOutcomes_AreTheStringsStoredAgainstTheProject()
        {
            DecisionModel.ApprovedWithoutConditions.Should().Be("Approved without conditions");
            DecisionModel.ApprovedWithConditions.Should().Be("Approved with conditions");
        }

        [Fact]
        public async Task OnGet_PopulatesTheSchoolNameAndDecisionFromTheProject()
        {
            var harness = new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School",
                NewSchoolDecision = new NewSchoolDecisionTask
                {
                    NewSchoolDecision = "Approved with conditions",
                    NewSchoolDecisionCondition = "Planning permission required"
                }
            });
            var model = BuildModel(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.CurrentFreeSchoolName.Should().Be("Test School");
            model.Decision.Should().Be("Approved with conditions");
            model.ConditionDescription.Should().Be("Planning permission required");
            await harness.GetProjectService.Received(1).Execute(ProjectId, TaskName.NewSchoolDecision);
        }

        [Fact]
        public async Task OnGet_WhenTheTaskHasNotBeenStarted_LeavesTheDecisionUnset()
        {
            var harness = new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School",
                NewSchoolDecision = null
            });
            var model = BuildModel(harness);

            await model.OnGet();

            model.CurrentFreeSchoolName.Should().Be("Test School");
            model.Decision.Should().BeNull();
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
            model.Decision.Should().BeNull();
        }

        [Fact]
        public async Task OnPost_WhenModelStateIsInvalid_ReturnsThePageWithoutUpdating()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.ModelState.AddModelError("decision", "Select a decision");

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            await harness.UpdateProjectTaskService.DidNotReceiveWithAnyArgs().Execute(default, default);
        }

        [Fact]
        public async Task OnPost_WithADecision_SendsTheDecisionAndCompletesTheTask()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.Decision = "Approved without conditions";

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.TaskList, ProjectId));
            harness.CapturedRequest!.NewSchoolDecision.NewSchoolDecision
                .Should().Be("Approved without conditions");
            harness.CapturedStatusRequest!.TaskName.Should().Be(TaskName.NewSchoolDecision.ToString());
            harness.CapturedStatusRequest!.ProjectTaskStatus.Should().Be(ProjectTaskStatus.Completed);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task OnPost_ApprovedWithConditions_WithNoConditionsGiven_DoesNotSave(string conditions)
        {
            var harness = new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School"
            });
            var model = BuildModel(harness);
            model.Decision = "Approved with conditions";
            model.ConditionDescription = conditions;

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            model.ModelState[DecisionModel.ConditionDescriptionField]!.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Enter the conditions that have been applied");
            harness.ErrorService.GetError(DecisionModel.ConditionDescriptionField).Should().NotBeNull();
            await harness.UpdateProjectTaskService.DidNotReceiveWithAnyArgs().Execute(default, default);
        }

        [Fact]
        public async Task OnPost_WhenValidationFails_KeepsTheSchoolNameAndTheAnswersJustPosted()
        {
            var harness = new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School",
                NewSchoolDecision = new NewSchoolDecisionTask
                {
                    NewSchoolDecision = "Approved without conditions",
                    NewSchoolDecisionCondition = "Stored conditions"
                }
            });
            var model = BuildModel(harness);
            model.Decision = "Approved with conditions";
            model.ConditionDescription = null;

            await model.OnPost();

            model.CurrentFreeSchoolName.Should().Be("Test School");
            model.Decision.Should().Be("Approved with conditions");
            model.ConditionDescription.Should().BeNull();
        }

        [Fact]
        public async Task OnPost_ApprovedWithoutConditions_DoesNotRequireConditions()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.Decision = "Approved without conditions";
            model.ConditionDescription = null;

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>();
            model.ModelState.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task OnPost_ApprovedWithConditions_SendsTheConditionsThatWereTypedIn()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.Decision = "Approved with conditions";
            model.ConditionDescription = "Planning permission required";

            await model.OnPost();

            harness.CapturedRequest!.NewSchoolDecision.NewSchoolDecisionCondition
                .Should().Be("Planning permission required");
        }

        /// <summary>
        /// The conditions box is hidden when the decision is "Approved without conditions", so any
        /// text typed before switching option is discarded rather than saved against the project.
        /// </summary>
        [Fact]
        public async Task OnPost_ApprovedWithoutConditions_ClearsTheConditions()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.Decision = "Approved without conditions";
            model.ConditionDescription = "Typed before switching option";

            await model.OnPost();

            harness.CapturedRequest!.NewSchoolDecision.NewSchoolDecisionCondition.Should().BeEmpty();
        }

        [Fact]
        public async Task OnPost_SendsTheProposalIdFromTheRouteWithTheDecision()
        {
            var harness = new NewSchoolTaskPageHarness();
            var model = BuildModel(harness);
            model.Decision = "Approved with conditions";
            model.ConditionDescription = "Planning permission required";

            await model.OnPost();

            harness.CapturedRequest!.NewSchoolDecision.ProposalId.Should().Be(ProposalId);
        }

        [Fact]
        public void Decision_IsRequired()
        {
            var model = BuildModel(new NewSchoolTaskPageHarness());
            model.Decision = null;

            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(
                model, new ValidationContext(model), results, validateAllProperties: true);

            isValid.Should().BeFalse();
            results.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Select the decision");
        }

        [Fact]
        public async Task OnPost_WhenTheUpdateFails_Rethrows()
        {
            var harness = new NewSchoolTaskPageHarness().UpdateFailsWith(new InvalidOperationException("API down"));
            var model = BuildModel(harness);
            model.Decision = "Approved with conditions";
            model.ConditionDescription = "Planning permission required";

            await model.Invoking(m => m.OnPost())
                .Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("API down");

            await harness.UpdateTaskStatusService.DidNotReceiveWithAnyArgs().Execute(default, default);
        }

        private static DecisionModel BuildModel(NewSchoolTaskPageHarness harness)
        {
            return new DecisionModel(
                harness.GetProjectService,
                harness.UpdateProjectTaskService,
                harness.UpdateTaskStatusService,
                Substitute.For<ILogger<DecisionModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                ProposalId = ProposalId,
                PageContext = NewSchoolTaskPageHarness.BuildPageContext()
            };
        }
    }
}
