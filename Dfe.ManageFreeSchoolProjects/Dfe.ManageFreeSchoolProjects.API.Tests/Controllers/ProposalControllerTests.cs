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
            var created = new ProposalResponse { Rid = "RID-1", ProjectId = ProjectId };
            createService.Execute(Arg.Any<CreateProposalRequest>()).Returns(created);

            var updateService = Substitute.For<IUpdateProposalService>();
            var controller = BuildController(createService, updateService, Substitute.For<IGetProposalService>());

            var result = await controller.CreateProposal(new CreateProposalRequest { ProjectId = ProjectId });

            var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(StatusCodes.Status201Created);
            objectResult.Value.Should().BeOfType<ApiSingleResponseV2<ProposalResponse>>()
                .Which.Data.Should().BeSameAs(created);
        }

        [Fact]
        public async Task CreateProposal_WhenThereIsNoProjectId_Returns400WithoutCreating()
        {
            var createService = Substitute.For<ICreateProposalService>();
            var updateService = Substitute.For<IUpdateProposalService>();
            var controller = BuildController(createService, updateService, Substitute.For<IGetProposalService>());

            var result = await controller.CreateProposal(new CreateProposalRequest { ProjectId = null });

            result.Should().BeOfType<BadRequestObjectResult>();
            await createService.DidNotReceiveWithAnyArgs().Execute(default!);
        }

        [Theory]
        [InlineData("")]
        public async Task CreateProposal_WhenTheProjectIdIsMissing_Returns400WithoutCreating(string projectId)
        {
            var createService = Substitute.For<ICreateProposalService>();
            var updateService = Substitute.For<IUpdateProposalService>();
            var controller = BuildController(createService, updateService, Substitute.For<IGetProposalService>());

            var result = await controller.CreateProposal(new CreateProposalRequest { ProjectId = projectId });

            var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequest.Value.Should().BeAssignableTo<IEnumerable<string>>()
                .Which.Should().NotBeEmpty();
            await createService.DidNotReceiveWithAnyArgs().Execute(default!);
        }

        [Fact]
        public async Task GetProposalList_Returns200WithTheProposals()
        {
            var proposals = new List<GetProposalSummaryResponse>
            {
                new() { Rid = "RID-1", ProjectId = ProjectId, Proposer = ProposalProposer.Diocese }
            };
            var getService = Substitute.For<IGetProposalService>();
            getService.ExecuteList(ProjectId).Returns(proposals);

            var controller = BuildController(Substitute.For<ICreateProposalService>(), Substitute.For<IUpdateProposalService>(), getService);

            var result = await controller.GetProposals(ProjectId);

            var objectResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            objectResult.Value.Should().BeOfType<ApiSingleResponseV2<List<GetProposalSummaryResponse>>>()
                .Which.Data.Should().BeSameAs(proposals);
            await getService.Received(1).ExecuteList(ProjectId);
        }

        [Fact]
        public async Task GetProposalList_WhenThereAreNoProposals_ReturnsAnEmptyList()
        {
            var getService = Substitute.For<IGetProposalService>();
            getService.ExecuteList(ProjectId).Returns([]);

            var controller = BuildController(Substitute.For<ICreateProposalService>(), Substitute.For<IUpdateProposalService>(), getService);

            var result = await controller.GetProposals(ProjectId);

            var objectResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.Value.Should().BeOfType<ApiSingleResponseV2<List<GetProposalSummaryResponse>>>()
                .Which.Data.Should().BeEmpty();
        }

        [Fact]
        public async Task GetProposalByRid_Returns200WithTheProposal()
        {
            var proposal = new ProposalResponse { Rid = "RID-1", ProjectId = ProjectId };
            var getService = Substitute.For<IGetProposalService>();
            getService.ExecuteSingle("RID-1").Returns(proposal);

            var controller = BuildController(
                Substitute.For<ICreateProposalService>(), Substitute.For<IUpdateProposalService>(), getService);

            var result = await controller.GetProposalByRid("RID-1");

            var objectResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            objectResult.Value.Should().BeOfType<ApiSingleResponseV2<ProposalResponse>>()
                .Which.Data.Should().BeSameAs(proposal);
            await getService.Received(1).ExecuteSingle("RID-1");
        }

        [Fact]
        public async Task GetProposalByRid_WhenThereIsNoSuchProposal_ReturnsNoData()
        {
            var getService = Substitute.For<IGetProposalService>();
            getService.ExecuteSingle("MISSING").Returns((ProposalResponse)null);

            var controller = BuildController(
                Substitute.For<ICreateProposalService>(), Substitute.For<IUpdateProposalService>(), getService);

            var result = await controller.GetProposalByRid("MISSING");

            var objectResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            objectResult.Value.Should().BeOfType<ApiSingleResponseV2<ProposalResponse>>()
                .Which.Data.Should().BeNull();
        }

        [Fact]
        public async Task UpdateProposal_WhenTheRequestIsValid_Returns200WithTheUpdatedProposal()
        {
            var updated = new ProposalResponse { Rid = "RID-1", ProjectId = ProjectId };
            var updateService = Substitute.For<IUpdateProposalService>();
            updateService.Execute(Arg.Any<UpdateProposalRequest>()).Returns(updated);

            var controller = BuildController(
                Substitute.For<ICreateProposalService>(), updateService, Substitute.For<IGetProposalService>());

            var request = new UpdateProposalRequest { Rid = "RID-1", Proposer = ProposalProposer.Diocese };

            var result = await controller.UpdateProposal(request);

            var objectResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            objectResult.Value.Should().BeOfType<ApiSingleResponseV2<ProposalResponse>>()
                .Which.Data.Should().BeSameAs(updated);
            await updateService.Received(1).Execute(request);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task UpdateProposal_WhenTheRidIsMissing_Returns400WithoutUpdating(string rid)
        {
            var updateService = Substitute.For<IUpdateProposalService>();

            var controller = BuildController(
                Substitute.For<ICreateProposalService>(), updateService, Substitute.For<IGetProposalService>());

            var result = await controller.UpdateProposal(new UpdateProposalRequest { Rid = rid });

            var badRequest = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequest.Value.Should().BeAssignableTo<IEnumerable<string>>().Which.Should().NotBeEmpty();
            await updateService.DidNotReceiveWithAnyArgs().Execute(default!);
        }

        private static ProposalController BuildController(
            ICreateProposalService createService, IUpdateProposalService updateService, IGetProposalService getService)
        {
            return new ProposalController(
                createService,
                getService,
                updateService,
                new UpdateProposalRequestValidator(),
                new CreateProposalRequestValidator(),
                Substitute.For<ILogger<ProposalController>>());
        }
    }
}
