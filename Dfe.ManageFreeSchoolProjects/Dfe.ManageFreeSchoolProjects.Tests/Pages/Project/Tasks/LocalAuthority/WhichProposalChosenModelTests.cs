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
    public class WhichProposalChosenModelTests
    {
        private const string ProjectId = "NEW-SCHOOL-1";

        [Fact]
        public async Task OnGet_PopulatesTheSchoolNameAndTheProposalsToChooseFrom()
        {
            var harness = BuildHarness();
            var proposalService = BuildProposalService(
                BuildProposal("RID-1", ProposalProposer.Diocese, "Diocese of Bristol"),
                BuildProposal("RID-2", ProposalProposer.AnotherLocalAuthority, "Cornwall Council"));

            var model = BuildModel(harness, proposalService);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.CurrentFreeSchoolName.Should().Be("Test School");
            model.Proposals.Should().Equal(
                new KeyValuePair<string, string>("RID-1", "Diocese [Diocese of Bristol]"),
                new KeyValuePair<string, string>("RID-2", "Another local authority [Cornwall Council]"));
            await proposalService.Received(1).ExecuteList(ProjectId);
        }

        [Fact]
        public async Task OnGet_ForTheAuthorityThatPublishedTheSpecification_NamesItFromTheProjectsLocalAuthority()
        {
            var harness = BuildHarness();
            harness.GetProjectService.Execute(ProjectId, TaskName.RegionAndLocalAuthority)
                .Returns(new GetProjectByTaskResponse
                {
                    RegionAndLocalAuthority = new RegionAndLocalAuthorityTask
                    {
                        LocalAuthority = "Bristol City Council"
                    }
                });

            var proposalService = BuildProposalService(
                BuildProposal("RID-1", ProposalProposer.LocalAuthorityThatPushedSpecification, name: null));

            var model = BuildModel(harness, proposalService);

            await model.OnGet();

            model.Proposals.Should().ContainSingle()
                .Which.Should().Be(new KeyValuePair<string, string>(
                    "RID-1", "Local authority that published the specification [Bristol City Council]"));
            await harness.GetProjectService.Received(1).Execute(ProjectId, TaskName.RegionAndLocalAuthority);
        }

        [Fact]
        public async Task OnGet_WhenTheProjectHasNoLocalAuthorityRecorded_StillListsTheProposals()
        {
            var harness = BuildHarness();
            harness.GetProjectService.Execute(ProjectId, TaskName.RegionAndLocalAuthority)
                .Returns(new GetProjectByTaskResponse { RegionAndLocalAuthority = null });

            var proposalService = BuildProposalService(
                BuildProposal("RID-1", ProposalProposer.LocalAuthorityThatPushedSpecification, name: null),
                BuildProposal("RID-2", ProposalProposer.Diocese, "Diocese of Bristol"));

            var model = BuildModel(harness, proposalService);

            await model.OnGet();

            model.Proposals.Should().HaveCount(2);
            model.Proposals[1].Should().Be(
                new KeyValuePair<string, string>("RID-2", "Diocese [Diocese of Bristol]"));
        }

        [Fact]
        public async Task OnGet_WhenTheProjectHasNoProposals_LeavesTheListEmpty()
        {
            var model = BuildModel(BuildHarness(),BuildProposalService());

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Proposals.Should().BeEmpty();
        }

        [Fact]
        public async Task OnGet_WhenTheProposalsCannotBeFetched_StillReturnsThePageWithAnEmptyList()
        {
            var proposalService = Substitute.For<IGetProposalService>();
            proposalService.ExecuteList(ProjectId).ThrowsAsync(new InvalidOperationException("API down"));

            var model = BuildModel(BuildHarness(),proposalService);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Proposals.Should().BeEmpty();
        }

        [Fact]
        public async Task OnPost_WithAChosenProposal_GoesToTheConfirmationPageForThatProposal()
        {
            var model = BuildModel(BuildHarness(),BuildProposalService());
            model.ChosenProposalId = "RID-2";

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(
                    string.Format(RouteConstants.NewSchoolConfirmProposalChosen, ProjectId, "RID-2"));
        }

        [Fact]
        public async Task OnPost_WhenModelStateIsInvalid_ReloadsTheProposalsAndReturnsThePage()
        {
            var harness = BuildHarness();
            var proposalService = BuildProposalService(
                BuildProposal("RID-1", ProposalProposer.Diocese, "Diocese of Bristol"));

            var model = BuildModel(harness, proposalService);
            model.ModelState.AddModelError("chosen-proposal", "Select the proposal that has been chosen");

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            model.Proposals.Should().ContainSingle()
                .Which.Should().Be(new KeyValuePair<string, string>("RID-1", "Diocese [Diocese of Bristol]"));
        }

        [Fact]
        public void ChosenProposalId_IsRequired()
        {
            var model = BuildModel(BuildHarness(),BuildProposalService());
            model.ChosenProposalId = null;

            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(
                model, new ValidationContext(model), results, validateAllProperties: true);

            isValid.Should().BeFalse();
            results.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Select the proposal that has been chosen");
        }

        private static NewSchoolTaskPageHarness BuildHarness() =>
            new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School"
            });

        private static GetProposalSummaryResponse BuildProposal(
            string rid, ProposalProposer proposer, string? name = "Test Proposer") =>
            new()
            {
                Rid = rid,
                ProjectId = ProjectId,
                Proposer = proposer,
                Name = name,
                Status = ProposalStatus.Active
            };

        private static IGetProposalService BuildProposalService(params GetProposalSummaryResponse[] proposals)
        {
            var proposalService = Substitute.For<IGetProposalService>();
            proposalService.ExecuteList(Arg.Any<string>())
                .Returns(new ApiSingleResponseV2<List<GetProposalSummaryResponse>>([.. proposals]));

            return proposalService;
        }

        private static WhichProposalChosenModel BuildModel(
            NewSchoolTaskPageHarness harness, IGetProposalService proposalService)
        {
            return new WhichProposalChosenModel(
                harness.GetProjectService,
                proposalService,
                Substitute.For<ILogger<WhichProposalChosenModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                PageContext = NewSchoolTaskPageHarness.BuildPageContext()
            };
        }
    }
}
