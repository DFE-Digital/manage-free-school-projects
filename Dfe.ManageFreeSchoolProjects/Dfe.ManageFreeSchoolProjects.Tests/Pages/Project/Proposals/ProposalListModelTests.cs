using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
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

            var model = BuildModel(overviewService);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Project.Should().BeSameAs(project);
            await overviewService.Received(1).Execute(ProjectId);
        }

        [Fact]
        public async Task OnGet_ListsTheProposalsForTheProject()
        {
            var overviewService = Substitute.For<IGetProjectOverviewService>();
            overviewService.Execute(ProjectId).Returns(new ProjectOverviewResponse());

            var model = BuildModel(overviewService);

            await model.OnGet();

            model.Proposals.Should().ContainSingle()
                .Which.ProjectId.Should().Be(ProjectId);
        }

        [Fact]
        public async Task OnGet_WhenTheProjectCannotBeFetched_StillReturnsThePage()
        {
            var overviewService = Substitute.For<IGetProjectOverviewService>();
            overviewService.Execute(Arg.Any<string>())
                .ThrowsAsync(new InvalidOperationException("API down"));

            var model = BuildModel(overviewService);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Project.Should().BeNull();
            model.Proposals.Should().BeNull();
        }

        private static ProposalListModel BuildModel(IGetProjectOverviewService overviewService)
        {
            return new ProposalListModel(
                overviewService,
                Substitute.For<IGetProposalService>(),
                Substitute.For<ILogger<ProposalListModel>>())
            {
                ProjectId = ProjectId,
                PageContext = CreateProposalPageHarness.BuildPageContext()
            };
        }
    }
}
