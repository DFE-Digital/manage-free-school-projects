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
    public class ProposedFaithStatusModelTests
    {
        private const string ProjectId = CreateProposalPageHarness.ProjectId;

        [Fact]
        public void OnGet_PreSelectsTheCachedStatus()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                ProposedFaithStatus = FaithStatus.Ethos
            });
            var model = BuildModel(harness);

            var result = model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Status.Should().Be(FaithStatus.Ethos);
        }

        [Fact]
        public void OnGet_WhenTheTrustJourneyWasUsed_LinksBackToConfirmTrust()
        {
            var model = BuildTestableModel(new CreateProposalCacheItem
            {
                Trust = new TrustTask { TRN = "TR12345" }
            });

            model.OnGet();

            model.BackLinkValue.Should()
                .Be(string.Format(RouteConstants.Proposals_Create_Confirm_Trust, ProjectId));
        }

        [Fact]
        public void OnGet_WhenTheDioceseJourneyWasUsed_LinksBackToFaithOfDiocese()
        {
            var model = BuildTestableModel(new CreateProposalCacheItem
            {
                FaithOfDiocese = FaithOfDiocese.ChurchOfEngland
            });

            model.OnGet();

            model.BackLinkValue.Should()
                .Be(string.Format(RouteConstants.Proposals_Create_Faith_Of_Diocese, ProjectId));
        }

        [Fact]
        public void OnGet_WhenTheReligiousOrganisationJourneyWasUsed_LinksBackToItsFaithPage()
        {
            var model = BuildTestableModel(new CreateProposalCacheItem
            {
                FaithTypeOfOtherReligiousOrganisation = FaithType.Hindu
            });

            model.OnGet();

            model.BackLinkValue.Should().Be(string.Format(
                RouteConstants.Proposals_Create_Faith_Of_Other_Religious_Organisation, ProjectId));
        }

        [Fact]
        public void OnGet_WhenTheOtherLocalAuthorityJourneyWasUsed_LinksBackToIt()
        {
            var model = BuildTestableModel(new CreateProposalCacheItem
            {
                OtherLocalAuthority = "Bristol City Council"
            });

            model.OnGet();

            model.BackLinkValue.Should()
                .Be(string.Format(RouteConstants.Proposals_Create_Other_Local_Authority, ProjectId));
        }

        [Fact]
        public void OnGet_WhenTheJointProposalJourneyWasUsed_LinksBackToIt()
        {
            var model = BuildTestableModel(new CreateProposalCacheItem
            {
                JointProposalLocalAuthority = "Bath and North East Somerset"
            });

            model.OnGet();

            model.BackLinkValue.Should().Be(string.Format(
                RouteConstants.Proposals_Create_Joint_Proposal_Local_Authority, ProjectId));
        }

        [Fact]
        public void OnGet_WhenNoOtherJourneyWasUsed_LinksBackToTheProposer()
        {
            var model = BuildTestableModel(new CreateProposalCacheItem());

            model.OnGet();

            model.BackLinkValue.Should()
                .Be(string.Format(RouteConstants.Proposals_Create_Proposer, ProjectId));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void OnGet_WhenALocalAuthorityIsBlank_DoesNotTreatItAsAnswered(string localAuthority)
        {
            var model = BuildTestableModel(new CreateProposalCacheItem
            {
                OtherLocalAuthority = localAuthority
            });

            model.OnGet();

            model.BackLinkValue.Should()
                .Be(string.Format(RouteConstants.Proposals_Create_Proposer, ProjectId));
        }

        [Fact]
        public void OnPost_WhenTheStatusIsInvalid_ReturnsThePageWithErrors()
        {
            var harness = new CreateProposalPageHarness();
            var model = BuildModel(harness);
            model.ModelState.AddModelError("faith-status", "Select the faith status");

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            harness.Cache.DidNotReceive().Update(Arg.Any<CreateProposalCacheItem>());
        }

        [Fact]
        public void OnPost_WhenThereIsNoFaith_SkipsStraightToCheckYourAnswers()
        {
            var cacheItem = new CreateProposalCacheItem();
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = BuildModel(harness);
            model.Status = FaithStatus.None;

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.Proposals_Create_Check_Your_Answers, ProjectId));
            cacheItem.ProposedFaithStatus.Should().Be(FaithStatus.None);
            harness.Cache.Received(1).Update(cacheItem);
        }

        [Theory]
        [InlineData(FaithStatus.Designation)]
        [InlineData(FaithStatus.Ethos)]
        public void OnPost_WhenThereIsAFaith_AsksForTheFaithType(FaithStatus status)
        {
            var cacheItem = new CreateProposalCacheItem();
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = BuildModel(harness);
            model.Status = status;

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.Proposals_Create_Proposed_Faith_Type, ProjectId));
            cacheItem.ProposedFaithStatus.Should().Be(status);
        }

        private static ProposedFaithStatusModel BuildModel(CreateProposalPageHarness harness)
        {
            return new ProposedFaithStatusModel(
                harness.Cache,
                Substitute.For<ILogger<ProposedFaithStatusModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                PageContext = CreateProposalPageHarness.BuildPageContext()
            };
        }

        private static TestableProposedFaithStatusModel BuildTestableModel(CreateProposalCacheItem cacheItem)
        {
            var harness = new CreateProposalPageHarness().With(cacheItem);

            return new TestableProposedFaithStatusModel(
                harness.Cache,
                Substitute.For<ILogger<ProposedFaithStatusModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                PageContext = CreateProposalPageHarness.BuildPageContext()
            };
        }

        private sealed class TestableProposedFaithStatusModel(
            ICreateProposalCache cache,
            ILogger<ProposedFaithStatusModel> logger,
            ErrorService errorService)
            : ProposedFaithStatusModel(cache, logger, errorService)
        {
            public string BackLinkValue => BackLink;
        }
    }
}
