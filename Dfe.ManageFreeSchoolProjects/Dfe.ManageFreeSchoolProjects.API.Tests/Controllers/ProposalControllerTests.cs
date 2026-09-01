using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.Controllers;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Proposals;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.Controllers
{
    public class ProposalControllerTests
    {
        private const string ProjectId = "NEW-SCHOOL-1";

        [Fact]
        public async Task CreateProposal_WhenTheRequestIsValid_Returns201WithTheCreatedProposal()
        {
            var createService = Substitute.For<ICreateProposalService>();
            var created = new CreateProposalResponse { Rid = "RID-1", ProjectId = ProjectId };
            createService.Execute(Arg.Any<CreateProposalRequest>()).Returns(created);

            var controller = BuildController(createService, Substitute.For<IGetProposalService>());

            var result = await controller.CreateProposal(new CreateProposalRequest { ProjectId = ProjectId });

            var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(StatusCodes.Status201Created);
            objectResult.Value.Should().BeOfType<ApiSingleResponseV2<CreateProposalResponse>>()
                .Which.Data.Should().BeSameAs(created);
        }

        [Fact]
        public async Task CreateProposal_WhenThereIsNoProjectId_Returns400WithoutCreating()
        {
            var createService = Substitute.For<ICreateProposalService>();
            var controller = BuildController(createService, Substitute.For<IGetProposalService>());

            var result = await controller.CreateProposal(new CreateProposalRequest { ProjectId = null });

            result.Should().BeOfType<BadRequestObjectResult>();
            await createService.DidNotReceiveWithAnyArgs().Execute(default!);
        }

        [Theory]
        [InlineData("")]
        public async Task CreateProposal_WhenTheProjectIdIsMissing_Returns400WithoutCreating(string projectId)
        {
            var createService = Substitute.For<ICreateProposalService>();
            var controller = BuildController(createService, Substitute.For<IGetProposalService>());

            var result = await controller.CreateProposal(new CreateProposalRequest { ProjectId = projectId });

            var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequest.Value.Should().BeAssignableTo<IEnumerable<string>>()
                .Which.Should().NotBeEmpty();
            await createService.DidNotReceiveWithAnyArgs().Execute(default!);
        }

        [Fact]
        public async Task GetProposalList_Returns200WithTheProposals()
        {
            var proposals = new List<GetProposalResponse>
            {
                new() { Rid = "RID-1", ProjectId = ProjectId, Proposer = ProposalProposer.Diocese }
            };
            var getService = Substitute.For<IGetProposalService>();
            getService.ExecuteList(ProjectId).Returns(proposals);

            var controller = BuildController(Substitute.For<ICreateProposalService>(), getService);

            var result = await controller.GetProjectTaskListSummary(ProjectId);

            var objectResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            objectResult.Value.Should().BeOfType<ApiSingleResponseV2<List<GetProposalResponse>>>()
                .Which.Data.Should().BeSameAs(proposals);
            await getService.Received(1).ExecuteList(ProjectId);
        }

        [Fact]
        public async Task GetProposalList_WhenThereAreNoProposals_ReturnsAnEmptyList()
        {
            var getService = Substitute.For<IGetProposalService>();
            getService.ExecuteList(ProjectId).Returns([]);

            var controller = BuildController(Substitute.For<ICreateProposalService>(), getService);

            var result = await controller.GetProjectTaskListSummary(ProjectId);

            var objectResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.Value.Should().BeOfType<ApiSingleResponseV2<List<GetProposalResponse>>>()
                .Which.Data.Should().BeEmpty();
        }

        private static ProposalController BuildController(
            ICreateProposalService createService, IGetProposalService getService)
        {
            return new ProposalController(
                createService,
                getService,
                new CreateProposalRequestValidator(),
                Substitute.For<ILogger<ProposalController>>());
        }
    }
}
