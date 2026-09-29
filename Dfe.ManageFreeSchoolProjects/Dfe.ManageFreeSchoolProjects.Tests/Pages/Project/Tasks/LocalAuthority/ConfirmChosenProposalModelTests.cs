using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.LocalAuthority.Decision;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Tasks.LocalAuthority
{
    public class ConfirmChosenProposalModelTests
    {
        private const string ProjectId = "NEW-SCHOOL-1";
        private const string ProposalId = "RID-2";

        [Fact]
        public async Task OnGet_ShowsTheSchoolNameAndTheNameOfTheChosenProposal()
        {
            var harness = BuildHarness();
            var proposalService = BuildProposalService(
                BuildProposal("RID-1", ProposalProposer.Diocese),
                BuildProposal(ProposalId, ProposalProposer.AnotherLocalAuthority));

            var model = BuildModel(harness, proposalService);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.CurrentFreeSchoolName.Should().Be("Test School");
            model.ProposalName.Should().Be("Another local authority");
        }

        [Fact]
        public async Task OnGet_WhenTheProposalIsNotOneOfTheProjects_LeavesTheNameEmpty()
        {
            var proposalService = BuildProposalService(BuildProposal("RID-1", ProposalProposer.Diocese));

            var model = BuildModel(BuildHarness(), proposalService);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.ProposalName.Should().BeEmpty();
        }

        [Fact]
        public async Task OnGet_WhenTheProposalsCannotBeFetched_StillReturnsThePage()
        {
            var proposalService = Substitute.For<IGetProposalService>();
            proposalService.ExecuteList(ProjectId).ThrowsAsync(new InvalidOperationException("API down"));

            var model = BuildModel(BuildHarness(), proposalService);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.ProposalName.Should().BeEmpty();
        }

        [Fact]
        public async Task OnPost_AnsweringYes_GoesToTheDecisionPageForThatProposal()
        {
            var model = BuildModel(BuildHarness(), BuildProposalService());
            model.IsRightProposal = true;

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(
                    string.Format(RouteConstants.NewSchoolDecision, ProjectId, ProposalId));
        }

        [Fact]
        public async Task OnPost_AnsweringNo_GoesBackToTheProposalChoice()
        {
            var model = BuildModel(BuildHarness(), BuildProposalService());
            model.IsRightProposal = false;

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(
                    string.Format(RouteConstants.NewSchoolWhichProposalChosen, ProjectId));
        }

        [Fact]
        public async Task OnPost_WhenModelStateIsInvalid_ReloadsThePageAndRecordsTheError()
        {
            var harness = BuildHarness();
            var proposalService = BuildProposalService(
                BuildProposal(ProposalId, ProposalProposer.AnotherLocalAuthority));

            var model = BuildModel(harness, proposalService);
            model.ModelState.AddModelError("is-right-proposal", "Select yes if this is the right proposal");

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            model.CurrentFreeSchoolName.Should().Be("Test School");
            model.ProposalName.Should().Be("Another local authority");
        }

        [Fact]
        public void IsRightProposal_IsRequired()
        {
            var model = BuildModel(BuildHarness(), BuildProposalService());
            model.IsRightProposal = null;

            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(
                model, new ValidationContext(model), results, validateAllProperties: true);

            isValid.Should().BeFalse();
            results.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Select yes if this is the right proposal");
        }

        private static NewSchoolTaskPageHarness BuildHarness() =>
            new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School"
            });

        private static GetProposalSummaryResponse BuildProposal(string rid, ProposalProposer proposer) =>
            new()
            {
                Rid = rid,
                ProjectId = ProjectId,
                Proposer = proposer,
                Status = ProposalStatus.Active
            };

        private static IGetProposalService BuildProposalService(params GetProposalSummaryResponse[] proposals)
        {
            var proposalService = Substitute.For<IGetProposalService>();
            proposalService.ExecuteList(Arg.Any<string>())
                .Returns(new ApiSingleResponseV2<List<GetProposalSummaryResponse>>([.. proposals]));

            return proposalService;
        }

        private static ConfirmChosenProposalModel BuildModel(
            NewSchoolTaskPageHarness harness, IGetProposalService proposalService)
        {
            return new ConfirmChosenProposalModel(
                harness.GetProjectService,
                proposalService,
                Substitute.For<ILogger<ConfirmChosenProposalModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                ProposalId = ProposalId,
                PageContext = NewSchoolTaskPageHarness.BuildPageContext()
            };
        }
    }
}
