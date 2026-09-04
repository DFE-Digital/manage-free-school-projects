using System;
using System.Linq;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.UseCases.Project.Proposals
{
    public class ProposalServiceTests
    {
        private const string ProjectId = "NEW-SCHOOL-1";

        [Fact]
        public async Task Create_StoresTheProposalAgainstTheProject()
        {
            using var context = BuildContext();
            var service = new CreateProposalService(context);

            var request = new CreateProposalRequest
            {
                ProjectId = ProjectId,
                Proposer = ProposalProposer.AcademyTrust,
                TrustReferenceNumber = "TR12345",
                TrustName = "Test Trust",
                TrustType = TrustType.MultiAcademyTrust,
                ProposedFaithStatus = FaithStatus.Designation,
                ProposedFaithType = FaithType.RomanCatholic,
                OtherFaithType = "Some other faith"
            };

            var response = await service.Execute(request);

            var stored = context.Proposals.Single();
            stored.ProjectId.Should().Be(ProjectId);
            stored.Proposer.Should().Be("Academy trust (including Diocese academy trust)");
            stored.TrustReferenceNumber.Should().Be("TR12345");
            stored.TrustName.Should().Be("Test Trust");
            stored.TrustType.Should().Be("MAT");
            stored.ProposedFaithStatus.Should().Be("Designation");
            stored.ProposedFaithType.Should().Be("Roman Catholic");
            stored.OtherFaithType.Should().Be("Some other faith");

            response.ProjectId.Should().Be(ProjectId);
            response.Proposer.Should().Be(ProposalProposer.AcademyTrust);
        }

        [Fact]
        public async Task Create_GivesEachProposalItsOwnRid()
        {
            using var context = BuildContext();
            var service = new CreateProposalService(context);

            var first = await service.Execute(BuildMinimalRequest());
            var second = await service.Execute(BuildMinimalRequest());

            first.Rid.Should().NotBeNullOrWhiteSpace();
            first.Rid.Should().HaveLength(11);
            second.Rid.Should().NotBe(first.Rid);
            context.Proposals.Select(p => p.Rid).Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task Create_WhenTheOptionalAnswersAreMissing_StoresThemAsNull()
        {
            using var context = BuildContext();
            var service = new CreateProposalService(context);

            var response = await service.Execute(new CreateProposalRequest
            {
                ProjectId = ProjectId,
                Proposer = ProposalProposer.LocalAuthorityThatPushedSpecification,
                TrustType = null,
                FaithOfDiocese = null,
                FaithTypeOfOtherReligiousOrganisation = null,
                ProposedFaithStatus = FaithStatus.None,
                ProposedFaithType = FaithType.None
            });

            var stored = context.Proposals.Single();
            stored.TrustType.Should().BeNull();
            stored.FaithOfDiocese.Should().BeNull();
            stored.FaithTypeOfOtherReligiousOrganisation.Should().BeNull();

            response.TrustType.Should().BeNull();
            response.FaithOfDiocese.Should().BeNull();
            response.FaithTypeOfOtherReligiousOrganisation.Should().BeNull();
        }

        [Fact]
        public async Task Create_StoresTheLocalAuthorityJourneyAnswers()
        {
            using var context = BuildContext();
            var service = new CreateProposalService(context);

            await service.Execute(new CreateProposalRequest
            {
                ProjectId = ProjectId,
                Proposer = ProposalProposer.JointProposal,
                OtherLocalAuthorityRegion = ProjectRegion.London,
                OtherLocalAuthority = "Bristol City Council",
                JointProposalLocalAuthorityRegion = ProjectRegion.SouthWest,
                JointProposalLocalAuthority = "Cornwall",
                ProposedFaithStatus = FaithStatus.None,
                ProposedFaithType = FaithType.None
            });

            var stored = context.Proposals.Single();
            stored.OtherLocalAuthority.Should().Be("Bristol City Council");
            stored.JointProposalLocalAuthority.Should().Be("Cornwall");
        }

        [Fact]
        public async Task List_ReturnsOnlyTheProposalsForTheProject()
        {
            using var context = BuildContext();
            context.Proposals.AddRange(
                BuildProposal("RID-1", ProjectId, ProposalProposer.Diocese, p => p.NameOfDiocese = "Diocese of Bristol"),
                BuildProposal("RID-2", "SOME-OTHER-PROJECT", ProposalProposer.Diocese, p => p.NameOfDiocese = "Elsewhere"));
            await context.SaveChangesAsync();

            var result = await new GetProposalService(context).ExecuteList(ProjectId);

            result.Should().ContainSingle();
            result[0].Rid.Should().Be("RID-1");
            result[0].ProjectId.Should().Be(ProjectId);
            result[0].Proposer.Should().Be(ProposalProposer.Diocese);
            result[0].Status.Should().Be(ProposalStatus.Active);
        }

        [Theory]
        [InlineData(ProposalProposer.AcademyTrust, "Test Trust")]
        [InlineData(ProposalProposer.Diocese, "Diocese of Bristol")]
        [InlineData(ProposalProposer.AnotherReligiousOrganisation, "Some organisation")]
        [InlineData(ProposalProposer.AnotherLocalAuthority, "Bristol City Council")]
        [InlineData(ProposalProposer.JointProposal, "Cornwall")]
        [InlineData(ProposalProposer.LocalAuthorityThatPushedSpecification, "")]
        public async Task List_TakesTheNameFromTheProposersOwnJourney(
            ProposalProposer proposer, string expectedName)
        {
            using var context = BuildContext();
            context.Proposals.Add(BuildProposal("RID-1", ProjectId, proposer, p =>
            {
                p.TrustName = "Test Trust";
                p.NameOfDiocese = "Diocese of Bristol";
                p.NameOfOtherReligiousOrganisation = "Some organisation";
                p.OtherLocalAuthority = "Bristol City Council";
                p.JointProposalLocalAuthority = "Cornwall";
            }));
            await context.SaveChangesAsync();

            var result = await new GetProposalService(context).ExecuteList(ProjectId);

            result.Single().Name.Should().Be(expectedName);
        }

        [Theory]
        [InlineData("Roman Catholic", FaithType.RomanCatholic)]
        [InlineData("Church of England", FaithType.ChurchOfEngland)]
        public async Task List_ReadsBackTheProposedFaith(string stored, FaithType expected)
        {
            using var context = BuildContext();
            context.Proposals.Add(BuildProposal("RID-1", ProjectId, ProposalProposer.Diocese,
                p => p.ProposedFaithType = stored));
            await context.SaveChangesAsync();

            var result = await new GetProposalService(context).ExecuteList(ProjectId);

            result.Single().ProposedFaithType.Should().Be(expected);
        }

        [Fact]
        public async Task List_WhenTheFaithWasNeverSet_ReturnsNull()
        {
            using var context = BuildContext();
            context.Proposals.Add(BuildProposal("RID-1", ProjectId, ProposalProposer.Diocese,
                p => p.ProposedFaithType = null));
            await context.SaveChangesAsync();

            var result = await new GetProposalService(context).ExecuteList(ProjectId);

            result.Single().ProposedFaithType.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task List_WhenNoFaithWasStored_ReturnsNull(string stored)
        {
            using var context = BuildContext();
            context.Proposals.Add(BuildProposal("RID-1", ProjectId, ProposalProposer.Diocese,
                p => p.ProposedFaithType = stored));
            await context.SaveChangesAsync();

            var result = await new GetProposalService(context).ExecuteList(ProjectId);

            result.Single().ProposedFaithType.Should().BeNull();
        }

        [Theory]
        [InlineData("Designation", FaithStatus.Designation)]
        [InlineData("Ethos", FaithStatus.Ethos)]
        [InlineData("None", FaithStatus.None)]
        public async Task List_ReadsBackTheProposedFaithStatus(string stored, FaithStatus expected)
        {
            using var context = BuildContext();
            context.Proposals.Add(BuildProposal("RID-1", ProjectId, ProposalProposer.Diocese,
                p => p.ProposedFaithStatus = stored));
            await context.SaveChangesAsync();

            var result = await new GetProposalService(context).ExecuteList(ProjectId);

            result.Single().ProposedFaithStatus.Should().Be(expected);
        }

        /// <summary>
        /// A proposal stored before the faith status was captured has nothing in the column, and
        /// listing it must not throw the way the mapper does for an unrecognised value.
        /// </summary>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task List_WhenNoFaithStatusWasStored_ReturnsNotSet(string stored)
        {
            using var context = BuildContext();
            context.Proposals.Add(BuildProposal("RID-1", ProjectId, ProposalProposer.Diocese,
                p => p.ProposedFaithStatus = stored));
            await context.SaveChangesAsync();

            var result = await new GetProposalService(context).ExecuteList(ProjectId);

            result.Single().ProposedFaithStatus.Should().Be(FaithStatus.NotSet);
        }

        [Fact]
        public async Task List_WhenTheFaithStatusWasNeverSet_ReturnsNotSet()
        {
            using var context = BuildContext();
            context.Proposals.Add(BuildProposal("RID-1", ProjectId, ProposalProposer.Diocese,
                p => p.ProposedFaithStatus = null));
            await context.SaveChangesAsync();

            var result = await new GetProposalService(context).ExecuteList(ProjectId);

            result.Single().ProposedFaithStatus.Should().Be(FaithStatus.NotSet);
        }

        [Fact]
        public async Task List_WhenTheProjectHasNoProposals_ReturnsEmpty()
        {
            using var context = BuildContext();

            var result = await new GetProposalService(context).ExecuteList(ProjectId);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetSingle_ReturnsTheProposalWithThatRid()
        {
            using var context = BuildContext();
            context.Proposals.Add(BuildProposal("RID-1", ProjectId, ProposalProposer.Diocese, p =>
            {
                p.NameOfDiocese = "Diocese of London";
                p.FaithOfDiocese = "Roman Catholic";
                p.ProposedFaithStatus = "Designation";
                p.ProposedFaithType = "Roman Catholic";
            }));
            context.Proposals.Add(BuildProposal("RID-2", ProjectId, ProposalProposer.Diocese,
                p => p.NameOfDiocese = "Diocese of Bath"));
            await context.SaveChangesAsync();

            var result = await new GetProposalService(context).ExecuteSingle("RID-1");

            result.Rid.Should().Be("RID-1");
            result.ProjectId.Should().Be(ProjectId);
            result.Proposer.Should().Be(ProposalProposer.Diocese);
            result.NameOfDiocese.Should().Be("Diocese of London");
            result.FaithOfDiocese.Should().Be(FaithOfDiocese.RomanCatholic);
            result.ProposedFaithStatus.Should().Be(FaithStatus.Designation);
            result.ProposedFaithType.Should().Be(FaithType.RomanCatholic);
        }

        [Fact]
        public async Task GetSingle_WhenThereIsNoProposalWithThatRid_ReturnsNull()
        {
            using var context = BuildContext();
            context.Proposals.Add(BuildProposal("RID-1", ProjectId, ProposalProposer.Diocese,
                p => p.ProposedFaithStatus = "None"));
            await context.SaveChangesAsync();

            var result = await new GetProposalService(context).ExecuteSingle("MISSING");

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetSingle_ReadsBackTheLocalAuthorityAnswers()
        {
            using var context = BuildContext();
            context.Proposals.Add(BuildProposal("RID-1", ProjectId, ProposalProposer.AnotherLocalAuthority, p =>
            {
                p.OtherLocalAuthority = "Bolton";
                p.OtherLocalAuthorityRegion = "North West";
                p.JointProposalLocalAuthority = "Camden";
                p.JointProposalLocalAuthorityRegion = "London";
                p.ProposedFaithStatus = "None";
                p.ProposedFaithType = "None";
            }));
            await context.SaveChangesAsync();

            var result = await new GetProposalService(context).ExecuteSingle("RID-1");

            result.OtherLocalAuthority.Should().Be("Bolton");
            result.OtherLocalAuthorityRegion.Should().Be("North West");
            result.JointProposalLocalAuthority.Should().Be("Camden");
            result.JointProposalLocalAuthorityRegion.Should().Be("London");
        }

        [Fact]
        public async Task GetSingle_ReadsBackTheTrustAnswers()
        {
            using var context = BuildContext();
            context.Proposals.Add(BuildProposal("RID-1", ProjectId, ProposalProposer.AcademyTrust, p =>
            {
                p.TrustReferenceNumber = "TR12345";
                p.TrustName = "Test Trust";
                p.TrustType = "MAT";
                p.ProposedFaithStatus = "None";
                p.ProposedFaithType = "None";
            }));
            await context.SaveChangesAsync();

            var result = await new GetProposalService(context).ExecuteSingle("RID-1");

            result.TrustReferenceNumber.Should().Be("TR12345");
            result.TrustName.Should().Be("Test Trust");
            result.TrustType.Should().Be(TrustType.MultiAcademyTrust);
        }

        private static CreateProposalRequest BuildMinimalRequest() => new()
        {
            ProjectId = ProjectId,
            Proposer = ProposalProposer.Diocese,
            ProposedFaithStatus = FaithStatus.None,
            ProposedFaithType = FaithType.None
        };

        private static Proposal BuildProposal(
            string rid, string projectId, ProposalProposer proposer, Action<Proposal> configure)
        {
            var proposal = new Proposal
            {
                Rid = rid,
                ProjectId = projectId,
                Proposer = global::Dfe.ManageFreeSchoolProjects.API.UseCases.Project.ProjectMapper.ToProposer(proposer)
            };

            configure(proposal);

            return proposal;
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
