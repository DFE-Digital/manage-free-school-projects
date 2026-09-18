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
            var harness = new NewSchoolTaskPageHarness().ReturnsProject(new GetProjectByTaskResponse
            {
                SchoolName = "Test School"
            });
            var proposalService = BuildProposalService(
                BuildProposal("RID-1", ProposalProposer.Diocese),
                BuildProposal("RID-2", ProposalProposer.AnotherLocalAuthority));

            var model = BuildModel(harness, proposalService);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.CurrentFreeSchoolName.Should().Be("Test School");
            model.Proposals.Should().Equal(
                new KeyValuePair<string, string>("RID-1", "Diocese"),
                new KeyValuePair<string, string>("RID-2", "Another local authority"));
            await proposalService.Received(1).ExecuteList(ProjectId);
        }

        [Fact]
        public async Task OnGet_WhenTheProjectHasNoProposals_LeavesTheListEmpty()
        {
            var model = BuildModel(BuildHarness(),BuildProposalService());

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Proposals.Should().BeEmpty();
        }

        /// <summary>
        /// A failure fetching the project or its proposals is swallowed so the user still gets the
        /// page. The list has to stay empty rather than null, because the view enumerates it.
        /// </summary>
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

        /// <summary>
        /// The dropdown is rebuilt from the API on every request, so a rejected post has to reload
        /// the proposals or the user would be sent back to an empty list.
        /// </summary>
        [Fact]
        public async Task OnPost_WhenModelStateIsInvalid_ReloadsTheProposalsAndReturnsThePage()
        {
            var harness = BuildHarness();
            var proposalService = BuildProposalService(BuildProposal("RID-1", ProposalProposer.Diocese));

            var model = BuildModel(harness, proposalService);
            model.ModelState.AddModelError("chosen-proposal", "Select the proposal that has been chosen");

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            model.Proposals.Should().ContainSingle()
                .Which.Should().Be(new KeyValuePair<string, string>("RID-1", "Diocese"));
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

        /// <summary>A harness whose project loads, so the page gets as far as the proposals.</summary>
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
