using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Exceptions;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.UseCases.Project.Tasks
{
    public class UpdateProjectByTaskServiceTests
    {
        private const string ProjectId = "NEW-SCHOOL-1";

        [Fact]
        public async Task Execute_MarksTheChosenProposalSuccessfulAndTheRestUnsuccessful()
        {
            using var context = BuildContext();
            SeedProject(context);
            context.Proposals.AddRange(
                BuildProposal("RID-1", ProjectId),
                BuildProposal("RID-2", ProjectId),
                BuildProposal("RID-3", ProjectId));
            await context.SaveChangesAsync();

            await BuildService(context).Execute(ProjectId, BuildDecisionRequest("RID-2"));

            StatusOf(context, "RID-1").Should().Be("Unsuccessful");
            StatusOf(context, "RID-2").Should().Be("Successful");
            StatusOf(context, "RID-3").Should().Be("Unsuccessful");
        }

        [Fact]
        public async Task Execute_LeavesTheProposalsOnOtherProjectsAlone()
        {
            using var context = BuildContext();
            SeedProject(context);
            context.Proposals.AddRange(
                BuildProposal("RID-1", ProjectId),
                BuildProposal("RID-2", "SOME-OTHER-PROJECT"));
            await context.SaveChangesAsync();

            await BuildService(context).Execute(ProjectId, BuildDecisionRequest("RID-1"));

            StatusOf(context, "RID-1").Should().Be("Successful");
            StatusOf(context, "RID-2").Should().Be("Active");
        }

        [Fact]
        public async Task Execute_WhenTheProposalIsNotRecognised_LeavesEveryProposalAlone()
        {
            using var context = BuildContext();
            SeedProject(context);
            context.Proposals.AddRange(
                BuildProposal("RID-1", ProjectId),
                BuildProposal("RID-2", ProjectId));
            await context.SaveChangesAsync();

            await BuildService(context).Execute(ProjectId, BuildDecisionRequest("NOT-A-PROPOSAL"));

            context.Proposals.Select(p => p.Status).Should().AllBe("Active");
        }

        [Fact]
        public async Task Execute_WhenTheTaskIsNotTheDecision_LeavesTheProposalsAlone()
        {
            using var context = BuildContext();
            SeedProject(context);
            context.Proposals.Add(BuildProposal("RID-1", ProjectId));
            await context.SaveChangesAsync();

            var request = new UpdateProjectByTaskRequest
            {
                NewSchoolConditions = new NewSchoolConditionsTask { NewSchoolConditions = "Yes" }
            };

            await BuildService(context).Execute(ProjectId, request);

            StatusOf(context, "RID-1").Should().Be("Active");
        }

        [Fact]
        public async Task Execute_WhenTheProjectDoesNotExist_Throws()
        {
            using var context = BuildContext();

            await new UpdateProjectByTaskService(context, [])
                .Invoking(service => service.Execute(ProjectId, BuildDecisionRequest("RID-1")))
                .Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Project {ProjectId} not found");
        }

        private static UpdateProjectByTaskRequest BuildDecisionRequest(string proposalId) => new()
        {
            NewSchoolDecision = new NewSchoolDecisionTask
            {
                NewSchoolDecision = "Approved without conditions",
                ProposalId = proposalId
            }
        };

        private static string StatusOf(MfspContext context, string rid) =>
            context.Proposals.Single(p => p.Rid == rid).Status;

        private static UpdateProjectByTaskService BuildService(MfspContext context) => new(context, []);

        private static Proposal BuildProposal(string rid, string projectId) => new()
        {
            Rid = rid,
            ProjectId = projectId,
            Proposer = ProjectMapper.ToProposer(ProposalProposer.Diocese),
            Status = "Active"
        };

        private static void SeedProject(MfspContext context)
        {
            context.Kpi.Add(new Kpi
            {
                Rid = "KPI-1",
                ProjectStatusProjectId = ProjectId,
                AprilIndicator = "N",
                FsType = "Free School",
                FsType1 = "Free School",
                MatUnitProjects = "0",
                SponsorUnitProjects = "0",
                UpperStatus = "Open",
                Wave = "Wave 15"
            });

            context.SaveChanges();
        }

        private static MfspContext BuildContext()
        {
            var options = new DbContextOptionsBuilder<MfspContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new MfspContext(options, null);
        }
    }
}
