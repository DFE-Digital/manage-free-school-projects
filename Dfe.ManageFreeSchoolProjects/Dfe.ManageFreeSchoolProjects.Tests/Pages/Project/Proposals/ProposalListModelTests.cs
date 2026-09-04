using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Proposals
{
    public class ProposalListModelTests
    {
        private const string ProjectId = CreateProposalPageHarness.ProjectId;

        [Fact]
        public async Task OnGet_LoadsTheProjectForTheRouteId()
        {
            var project = new ProjectOverviewResponse();
            var overviewService = Substitute.For<IGetProjectOverviewService>();
            overviewService.Execute(ProjectId).Returns(project);

            var model = BuildModel(overviewService, out _);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Project.Should().BeSameAs(project);
            await overviewService.Received(1).Execute(ProjectId);
        }

        [Fact]
        public async Task OnGet_ListsTheProposalsForTheProject()
        {
            var model = BuildModel(BuildOverviewService(), out var proposalService);

            var proposals = new List<GetProposalSummaryResponse>
            {
                new()
                {
                    Rid = "RID-1",
                    ProjectId = ProjectId,
                    Proposer = ProposalProposer.Diocese,
                    Name = "Diocese of Bristol",
                    ProposedFaithStatus = FaithStatus.Designation,
                    ProposedFaithType = FaithType.RomanCatholic,
                    Status = ProposalStatus.Active
                }
            };
            proposalService.ExecuteList(ProjectId)
                .Returns(new ApiSingleResponseV2<List<GetProposalSummaryResponse>>(proposals));

            await model.OnGet();

            model.Proposals.Should().BeSameAs(proposals);
            await proposalService.Received(1).ExecuteList(ProjectId);
        }

        [Fact]
        public async Task OnGet_WhenTheProjectHasNoProposals_LeavesTheListEmpty()
        {
            var model = BuildModel(BuildOverviewService(), out var proposalService);
            proposalService.ExecuteList(ProjectId)
                .Returns(new ApiSingleResponseV2<List<GetProposalSummaryResponse>>([]));

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Proposals.Should().BeEmpty();
        }

        /// <summary>
        /// A failure fetching the project is swallowed so the user still gets the page rather than an
        /// error, which also means the proposals are never asked for.
        /// </summary>
        [Fact]
        public async Task OnGet_WhenTheProjectCannotBeFetched_StillReturnsThePage()
        {
            var overviewService = Substitute.For<IGetProjectOverviewService>();
            overviewService.Execute(Arg.Any<string>())
                .ThrowsAsync(new InvalidOperationException("API down"));

            var model = BuildModel(overviewService, out var proposalService);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Project.Should().BeNull();
            model.Proposals.Should().BeNull();
            await proposalService.DidNotReceiveWithAnyArgs().ExecuteList(default!);
        }

        [Fact]
        public async Task OnGet_WhenTheProposalsCannotBeFetched_StillReturnsThePage()
        {
            var project = new ProjectOverviewResponse();
            var overviewService = Substitute.For<IGetProjectOverviewService>();
            overviewService.Execute(ProjectId).Returns(project);

            var model = BuildModel(overviewService, out var proposalService);
            proposalService.ExecuteList(ProjectId)
                .ThrowsAsync(new InvalidOperationException("API down"));

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Project.Should().BeSameAs(project);
            model.Proposals.Should().BeNull();
        }

        private static IGetProjectOverviewService BuildOverviewService()
        {
            var overviewService = Substitute.For<IGetProjectOverviewService>();
            overviewService.Execute(ProjectId).Returns(new ProjectOverviewResponse());

            return overviewService;
        }

        private static ProposalListModel BuildModel(
            IGetProjectOverviewService overviewService, out IGetProposalService proposalService)
        {
            proposalService = Substitute.For<IGetProposalService>();

            // Default to an empty list so the tests that are not about proposals still get a page.
            proposalService.ExecuteList(Arg.Any<string>())
                .Returns(new ApiSingleResponseV2<List<GetProposalSummaryResponse>>([]));

            return new ProposalListModel(
                overviewService,
                proposalService,
                Substitute.For<ILogger<ProposalListModel>>())
            {
                ProjectId = ProjectId,
                PageContext = CreateProposalPageHarness.BuildPageContext()
            };
        }
    }
}
