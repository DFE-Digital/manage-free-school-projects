using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Proposals
{
    public class ProposalDetailsModelTests
    {
        private const string ProjectId = "NEW-SCHOOL-1";
        private const string Rid = "RID-1";

        [Fact]
        public async Task OnGetAsync_ShowsTheProposalAndTheProjectItBelongsTo()
        {
            var proposal = new ProposalResponse
            {
                Rid = Rid,
                ProjectId = ProjectId,
                Proposer = ProposalProposer.Diocese,
                NameOfDiocese = "Diocese of London"
            };
            var project = new ProjectOverviewResponse();

            var getProposalService = Substitute.For<IGetProposalService>();
            getProposalService.ExecuteSingle(Rid).Returns(new ApiSingleResponseV2<ProposalResponse>(proposal));

            var getProjectOverviewService = Substitute.For<IGetProjectOverviewService>();
            getProjectOverviewService.Execute(ProjectId).Returns(project);

            var model = BuildModel(getProposalService, getProjectOverviewService);

            var result = await model.OnGetAsync();

            result.Should().BeOfType<PageResult>();
            model.Proposal.Should().BeSameAs(proposal);
            model.Project.Should().BeSameAs(project);
        }

        [Fact]
        public async Task OnGetAsync_WhenTheProposalCannotBeFound_Returns404WithoutLoadingTheProject()
        {
            var getProposalService = Substitute.For<IGetProposalService>();
            getProposalService.ExecuteSingle(Rid).Returns(new ApiSingleResponseV2<ProposalResponse>(null!));

            var getProjectOverviewService = Substitute.For<IGetProjectOverviewService>();

            var model = BuildModel(getProposalService, getProjectOverviewService);

            var result = await model.OnGetAsync();

            result.Should().BeOfType<NotFoundResult>();
            await getProjectOverviewService.DidNotReceiveWithAnyArgs().Execute(default);
        }

        [Fact]
        public async Task OnGetAsync_WhenThereIsNoResponseAtAll_Returns404()
        {
            var getProposalService = Substitute.For<IGetProposalService>();
            getProposalService.ExecuteSingle(Rid).Returns((ApiSingleResponseV2<ProposalResponse>)null!);

            var model = BuildModel(getProposalService, Substitute.For<IGetProjectOverviewService>());

            var result = await model.OnGetAsync();

            result.Should().BeOfType<NotFoundResult>();
        }

        private static ProposalDetailsModel BuildModel(
            IGetProposalService getProposalService, IGetProjectOverviewService getProjectOverviewService) =>
            new(getProposalService,
                getProjectOverviewService,
                Substitute.For<ILogger<ProposalDetailsModel>>())
            {
                ProjectId = ProjectId,
                Rid = Rid
            };
    }
}
