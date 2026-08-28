using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create;
using Dfe.ManageFreeSchoolProjects.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Proposals
{
    public class CreateProposerModelTests
    {
        private const string ProjectId = CreateProposalPageHarness.ProjectId;

        [Fact]
        public void OnGet_WhenStartingANewProposal_ClearsTheSession()
        {
            var harness = new CreateProposalPageHarness()
                .With(new CreateProposalCacheItem { Proposer = ProposalProposer.Diocese });
            var model = BuildModel(harness);
            model.IsNewProposal = true;

            var result = model.OnGet();

            result.Should().BeOfType<PageResult>();
            harness.Cache.Received(1).Delete();
            model.Proposer.Should().BeNull();
        }

        [Fact]
        public void OnGet_WhenReturningToThePage_PreSelectsTheCachedProposer()
        {
            var harness = new CreateProposalPageHarness()
                .With(new CreateProposalCacheItem { Proposer = ProposalProposer.Diocese });
            var model = BuildModel(harness);

            var result = model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Proposer.Should().Be(ProposalProposer.Diocese);
            harness.Cache.DidNotReceive().Delete();
        }

        [Fact]
        public void OnGet_WhenTheProposerDiffersFromThePrevious_RecordsThePrevious()
        {
            var cacheItem = new CreateProposalCacheItem
            {
                Proposer = ProposalProposer.Diocese,
                PreviousProposer = ProposalProposer.AcademyTrust
            };
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = BuildModel(harness);

            model.OnGet();

            cacheItem.PreviousProposer.Should().Be(ProposalProposer.Diocese);
            harness.Cache.Received(1).Update(cacheItem);
        }

        [Fact]
        public void OnGet_WhenTheProposerIsUnchanged_DoesNotRewriteTheCache()
        {
            var cacheItem = new CreateProposalCacheItem
            {
                Proposer = ProposalProposer.Diocese,
                PreviousProposer = ProposalProposer.Diocese
            };
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = BuildModel(harness);

            model.OnGet();

            harness.Cache.DidNotReceive().Update(Arg.Any<CreateProposalCacheItem>());
        }

        [Fact]
        public void OnGet_LinksBackToTheProposalList()
        {
            var harness = new CreateProposalPageHarness();
            var model = BuildTestableModel(harness);

            model.OnGet();

            model.BackLinkValue.Should().Be(string.Format(RouteConstants.Proposals, ProjectId));
        }

        [Fact]
        public void OnPost_WhenNoProposerIsSelected_ReturnsThePageWithErrors()
        {
            var harness = new CreateProposalPageHarness();
            var model = BuildModel(harness);
            model.ModelState.AddModelError("proposer", "Select the proposer");

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            harness.Cache.DidNotReceive().Update(Arg.Any<CreateProposalCacheItem>());
        }

        [Theory]
        [InlineData(ProposalProposer.AcademyTrust, RouteConstants.Proposals_Create_SearchTrustByTRN)]
        [InlineData(ProposalProposer.Diocese, RouteConstants.Proposals_Create_Name_Of_Diocese)]
        [InlineData(ProposalProposer.AnotherReligiousOrganisation, RouteConstants.Proposals_Create_Name_Of_Other_Religious_Organisation)]
        [InlineData(ProposalProposer.LocalAuthorityThatPushedSpecification, RouteConstants.Proposals_Create_Proposed_Faith_Status)]
        [InlineData(ProposalProposer.AnotherLocalAuthority, RouteConstants.Proposals_Create_Other_Local_Authority_Region)]
        [InlineData(ProposalProposer.JointProposal, RouteConstants.Proposals_Create_Joint_Proposal_Region)]
        public void OnPost_RedirectsToTheJourneyForTheChosenProposer(
            ProposalProposer proposer, string expectedRoute)
        {
            var cacheItem = new CreateProposalCacheItem { PreviousProposer = proposer };
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = BuildModel(harness);
            model.Proposer = proposer;

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(expectedRoute, ProjectId));
            cacheItem.Proposer.Should().Be(proposer);
            harness.Cache.Received().Update(cacheItem);
        }

        [Fact]
        public void OnPost_WhenTheProposerChanged_ClearsTheAbandonedJourney()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                PreviousProposer = ProposalProposer.Diocese,
                NameOfDiocese = "Diocese of Bristol"
            });
            var model = BuildModel(harness);
            model.Proposer = ProposalProposer.AcademyTrust;

            model.OnPost();

            harness.Cache.Received(1).Delete();
        }

        [Fact]
        public void OnPost_WhenTheProposerIsUnchanged_KeepsTheExistingAnswers()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                PreviousProposer = ProposalProposer.Diocese,
                NameOfDiocese = "Diocese of Bristol"
            });
            var model = BuildModel(harness);
            model.Proposer = ProposalProposer.Diocese;

            model.OnPost();

            harness.Cache.DidNotReceive().Delete();
        }

        private static CreateProposerModel BuildModel(CreateProposalPageHarness harness)
        {
            return new CreateProposerModel(
                harness.Cache,
                Substitute.For<ILogger<CreateProposerModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                PageContext = CreateProposalPageHarness.BuildPageContext()
            };
        }

        private static TestableCreateProposerModel BuildTestableModel(CreateProposalPageHarness harness)
        {
            return new TestableCreateProposerModel(
                harness.Cache,
                Substitute.For<ILogger<CreateProposerModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                PageContext = CreateProposalPageHarness.BuildPageContext()
            };
        }

        private sealed class TestableCreateProposerModel(
            ICreateProposalCache cache, ILogger<CreateProposerModel> logger, ErrorService errorService)
            : CreateProposerModel(cache, logger, errorService)
        {
            public string BackLinkValue => BackLink;
        }
    }
}
