using System;
using System.Linq;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.UseCases.Project.Proposals
{
    public class UpdateProposalServiceTests
    {
        private const string ProjectId = "NEW-SCHOOL-1";
        private const string Rid = "RID-1";

        [Fact]
        public async Task Update_WhenTheProposalDoesNotExist_Throws()
        {
            using var context = BuildContext();
            var service = new UpdateProposalService(context);

            var act = () => service.Execute(new UpdateProposalRequest { Rid = "MISSING" });

            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*MISSING*");
        }

        [Fact]
        public async Task Update_WhenTheProposerIsNotRecognised_Throws()
        {
            using var context = await SeedProposal(ProposalProposer.Diocese);
            var service = new UpdateProposalService(context);

            var act = () => service.Execute(new UpdateProposalRequest
            {
                Rid = Rid,
                Proposer = (ProposalProposer)99
            });

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task Update_AcademyTrust_StoresTheNewTrust()
        {
            using var context = await SeedProposal(ProposalProposer.AcademyTrust, p =>
            {
                p.TrustReferenceNumber = "TR00001";
                p.TrustName = "Old Trust";
                p.TrustType = ProjectMapper.ToTrustType(TrustType.SingleAcademyTrust);
            });
            var service = new UpdateProposalService(context);

            var response = await service.Execute(new UpdateProposalRequest
            {
                Rid = Rid,
                Proposer = ProposalProposer.AcademyTrust,
                TrustReferenceNumber = "TR12345",
                TrustName = "New Trust",
                TrustType = TrustType.MultiAcademyTrust
            });

            var stored = context.Proposals.Single();
            stored.TrustReferenceNumber.Should().Be("TR12345");
            stored.TrustName.Should().Be("New Trust");
            stored.TrustType.Should().Be("MAT");

            response.TrustName.Should().Be("New Trust");
            response.TrustType.Should().Be(TrustType.MultiAcademyTrust);
        }

        [Fact]
        public async Task Update_AcademyTrust_WhenAnAnswerIsNotSupplied_LeavesItAlone()
        {
            using var context = await SeedProposal(ProposalProposer.AcademyTrust, p =>
            {
                p.TrustReferenceNumber = "TR00001";
                p.TrustName = "Old Trust";
                p.TrustType = ProjectMapper.ToTrustType(TrustType.SingleAcademyTrust);
            });
            var service = new UpdateProposalService(context);

            await service.Execute(new UpdateProposalRequest
            {
                Rid = Rid,
                Proposer = ProposalProposer.AcademyTrust,
                TrustName = "New Trust"
            });

            var stored = context.Proposals.Single();
            stored.TrustName.Should().Be("New Trust");
            stored.TrustReferenceNumber.Should().Be("TR00001");
            stored.TrustType.Should().Be(ProjectMapper.ToTrustType(TrustType.SingleAcademyTrust));
        }

        [Fact]
        public async Task Update_Diocese_StoresTheNameAndFaith()
        {
            using var context = await SeedProposal(ProposalProposer.Diocese);
            var service = new UpdateProposalService(context);

            await service.Execute(new UpdateProposalRequest
            {
                Rid = Rid,
                Proposer = ProposalProposer.Diocese,
                NameOfDiocese = "Diocese of London",
                FaithOfDiocese = FaithOfDiocese.RomanCatholic
            });

            var stored = context.Proposals.Single();
            stored.NameOfDiocese.Should().Be("Diocese of London");
            stored.FaithOfDiocese.Should().Be("Roman Catholic");
        }

        [Fact]
        public async Task Update_AnotherReligiousOrganisation_StoresTheNameAndFaith()
        {
            using var context = await SeedProposal(ProposalProposer.AnotherReligiousOrganisation);
            var service = new UpdateProposalService(context);

            await service.Execute(new UpdateProposalRequest
            {
                Rid = Rid,
                Proposer = ProposalProposer.AnotherReligiousOrganisation,
                NameOfOtherReligiousOrganisation = "An organisation",
                FaithTypeOfOtherReligiousOrganisation = FaithType.Other,
                OtherFaithTypeOfOtherReligiousOrganisation = "Jain"
            });

            var stored = context.Proposals.Single();
            stored.NameOfOtherReligiousOrganisation.Should().Be("An organisation");
            stored.FaithTypeOfOtherReligiousOrganisation.Should().Be("Other");
            stored.OtherFaithTypeOfOtherReligiousOrganisation.Should().Be("Jain");
        }

        /// <summary>
        /// The free text only belongs to the "Other" option, so choosing a named faith clears it.
        /// </summary>
        [Fact]
        public async Task Update_AnotherReligiousOrganisation_WhenTheFaithIsNotOther_ClearsTheFreeText()
        {
            using var context = await SeedProposal(ProposalProposer.AnotherReligiousOrganisation,
                p => p.OtherFaithTypeOfOtherReligiousOrganisation = "Jain");
            var service = new UpdateProposalService(context);

            await service.Execute(new UpdateProposalRequest
            {
                Rid = Rid,
                Proposer = ProposalProposer.AnotherReligiousOrganisation,
                FaithTypeOfOtherReligiousOrganisation = FaithType.Muslim,
                OtherFaithTypeOfOtherReligiousOrganisation = "Jain"
            });

            context.Proposals.Single().OtherFaithTypeOfOtherReligiousOrganisation.Should().BeNull();
        }

        [Fact]
        public async Task Update_AnotherLocalAuthority_StoresTheAuthorityAndItsRegion()
        {
            using var context = await SeedProposal(ProposalProposer.AnotherLocalAuthority);
            var service = new UpdateProposalService(context);

            await service.Execute(new UpdateProposalRequest
            {
                Rid = Rid,
                Proposer = ProposalProposer.AnotherLocalAuthority,
                OtherLocalAuthority = "Bolton",
                OtherLocalAuthorityRegion = ProjectRegion.NorthWest
            });

            var stored = context.Proposals.Single();
            stored.OtherLocalAuthority.Should().Be("Bolton");
            stored.OtherLocalAuthorityRegion.Should().Be("North West");
        }

        [Fact]
        public async Task Update_JointProposal_StoresTheAuthorityAndItsRegion()
        {
            using var context = await SeedProposal(ProposalProposer.JointProposal);
            var service = new UpdateProposalService(context);

            await service.Execute(new UpdateProposalRequest
            {
                Rid = Rid,
                Proposer = ProposalProposer.JointProposal,
                JointProposalLocalAuthority = "Camden",
                JointProposalLocalAuthorityRegion = ProjectRegion.London
            });

            var stored = context.Proposals.Single();
            stored.JointProposalLocalAuthority.Should().Be("Camden");
            stored.JointProposalLocalAuthorityRegion.Should().Be("London");
        }

        /// <summary>
        /// This proposer has no answers of its own; the shared faith answers below still apply.
        /// </summary>
        [Fact]
        public async Task Update_LocalAuthorityThatPublishedTheSpecification_OnlyUpdatesTheFaithAnswers()
        {
            using var context = await SeedProposal(ProposalProposer.LocalAuthorityThatPushedSpecification);
            var service = new UpdateProposalService(context);

            await service.Execute(new UpdateProposalRequest
            {
                Rid = Rid,
                Proposer = ProposalProposer.LocalAuthorityThatPushedSpecification,
                ProposedFaithStatus = FaithStatus.Ethos
            });

            context.Proposals.Single().ProposedFaithStatus.Should().Be("Ethos");
        }

        [Fact]
        public async Task Update_StoresTheFaithAnswersWhicheverProposerItIs()
        {
            using var context = await SeedProposal(ProposalProposer.Diocese);
            var service = new UpdateProposalService(context);

            await service.Execute(new UpdateProposalRequest
            {
                Rid = Rid,
                Proposer = ProposalProposer.Diocese,
                ProposedFaithStatus = FaithStatus.Designation,
                ProposedFaithType = FaithType.Other,
                OtherFaithType = "Jain"
            });

            var stored = context.Proposals.Single();
            stored.ProposedFaithStatus.Should().Be("Designation");
            stored.ProposedFaithType.Should().Be("Other");
            stored.OtherFaithType.Should().Be("Jain");
        }

        [Fact]
        public async Task Update_WhenTheFaithAnswersAreNotSupplied_LeavesThemAlone()
        {
            using var context = await SeedProposal(ProposalProposer.Diocese, p =>
            {
                p.ProposedFaithStatus = "Ethos";
                p.ProposedFaithType = "Muslim";
                p.OtherFaithType = "Jain";
            });
            var service = new UpdateProposalService(context);

            await service.Execute(new UpdateProposalRequest
            {
                Rid = Rid,
                Proposer = ProposalProposer.Diocese,
                NameOfDiocese = "Diocese of London"
            });

            var stored = context.Proposals.Single();
            stored.ProposedFaithStatus.Should().Be("Ethos");
            stored.ProposedFaithType.Should().Be("Muslim");
            stored.OtherFaithType.Should().Be("Jain");
        }

        [Fact]
        public async Task Update_OnlyChangesTheProposalItWasAskedFor()
        {
            using var context = BuildContext();
            context.Proposals.Add(BuildProposal(Rid, ProposalProposer.Diocese,
                p => p.NameOfDiocese = "Diocese of London"));
            context.Proposals.Add(BuildProposal("RID-2", ProposalProposer.Diocese,
                p => p.NameOfDiocese = "Diocese of Bath"));
            await context.SaveChangesAsync();

            var service = new UpdateProposalService(context);

            await service.Execute(new UpdateProposalRequest
            {
                Rid = Rid,
                Proposer = ProposalProposer.Diocese,
                NameOfDiocese = "Diocese of Leeds"
            });

            context.Proposals.Single(p => p.Rid == Rid).NameOfDiocese.Should().Be("Diocese of Leeds");
            context.Proposals.Single(p => p.Rid == "RID-2").NameOfDiocese.Should().Be("Diocese of Bath");
        }

        private static async Task<MfspContext> SeedProposal(
            ProposalProposer proposer, Action<Proposal> configure = null)
        {
            var context = BuildContext();

            context.Proposals.Add(BuildProposal(Rid, proposer, configure));
            await context.SaveChangesAsync();

            return context;
        }

        private static Proposal BuildProposal(
            string rid, ProposalProposer proposer, Action<Proposal> configure = null)
        {
            var proposal = new Proposal
            {
                Rid = rid,
                ProjectId = ProjectId,
                Proposer = ProjectMapper.ToProposer(proposer),
                ProposedFaithStatus = "None",
                ProposedFaithType = "None"
            };

            configure?.Invoke(proposal);

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
