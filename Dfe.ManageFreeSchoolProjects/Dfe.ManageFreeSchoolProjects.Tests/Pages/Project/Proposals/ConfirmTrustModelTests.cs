using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Enums;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create;
using Dfe.ManageFreeSchoolProjects.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Proposals
{
    public class ConfirmTrustModelTests
    {
        private const string ProjectId = CreateProposalPageHarness.ProjectId;

        [Fact]
        public void OnGet_ShowsTheTrustFromTheCache()
        {
            var trust = new TrustTask { TRN = "TR12345", TrustName = "Test Trust" };
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem { Trust = trust });
            var model = BuildModel(harness);

            var result = model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Trust.Should().BeSameAs(trust);
            model.ConfirmOption.Should().BeNull();
        }

        [Theory]
        [InlineData(true, YesNoOption.Yes)]
        [InlineData(false, YesNoOption.No)]
        public void OnGet_PreSelectsAPreviousConfirmation(bool confirmed, YesNoOption expected)
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                Trust = new TrustTask { TRN = "TR12345" },
                TrustConfirmed = confirmed
            });
            var model = BuildModel(harness);

            model.OnGet();

            model.ConfirmOption.Should().Be(expected);
        }

        [Fact]
        public void OnPost_WhenNothingIsSelected_ReturnsThePageWithErrors()
        {
            var trust = new TrustTask { TRN = "TR12345" };
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem { Trust = trust });
            var model = BuildModel(harness);
            model.ModelState.AddModelError(
                nameof(ConfirmTrustModel.ConfirmOption), "Please select an option to confirm the trust");

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            harness.Cache.DidNotReceive().Update(Arg.Any<CreateProposalCacheItem>());
        }

        [Fact]
        public void OnPost_WhenThePageIsRedisplayed_StillShowsTheTrust()
        {
            var trust = new TrustTask { TRN = "TR12345", TrustName = "Test Trust" };
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem { Trust = trust });
            var model = BuildModel(harness);
            model.ModelState.AddModelError(nameof(ConfirmTrustModel.ConfirmOption), "Select an option");

            model.OnPost();

            model.Trust.Should().BeSameAs(trust);
        }

        [Fact]
        public void OnPost_WhenTheTrustIsWrong_GoesBackToTheSearch()
        {
            var cacheItem = new CreateProposalCacheItem { Trust = new TrustTask { TRN = "TR12345" } };
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = BuildModel(harness);
            model.ConfirmOption = YesNoOption.No;

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.Proposals_Create_SearchTrustByTRN, ProjectId));
            cacheItem.TrustConfirmed.Should().BeNull();
            harness.Cache.DidNotReceive().Update(Arg.Any<CreateProposalCacheItem>());
        }

        [Fact]
        public void OnPost_WhenTheTrustIsConfirmed_RecordsItAndMovesOn()
        {
            var cacheItem = new CreateProposalCacheItem { Trust = new TrustTask { TRN = "TR12345" } };
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = BuildModel(harness);
            model.ConfirmOption = YesNoOption.Yes;

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.Proposals_Create_Proposed_Faith_Status, ProjectId));
            cacheItem.TrustConfirmed.Should().BeTrue();
            harness.Cache.Received(1).Update(cacheItem);
        }

        [Fact]
        public void OnGet_LinksBackToTheTrustSearch()
        {
            var harness = new CreateProposalPageHarness();
            var model = new TestableConfirmTrustModel(
                harness.Cache,
                Substitute.For<ILogger<ConfirmTrustModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                PageContext = CreateProposalPageHarness.BuildPageContext()
            };

            model.OnGet();

            model.BackLinkValue.Should()
                .Be(string.Format(RouteConstants.Proposals_Create_SearchTrustByTRN, ProjectId));
        }

        private static ConfirmTrustModel BuildModel(CreateProposalPageHarness harness)
        {
            return new ConfirmTrustModel(
                harness.Cache,
                Substitute.For<ILogger<ConfirmTrustModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                PageContext = CreateProposalPageHarness.BuildPageContext()
            };
        }

        private sealed class TestableConfirmTrustModel(
            ICreateProposalCache cache, ILogger<ConfirmTrustModel> logger, ErrorService errorService)
            : ConfirmTrustModel(cache, logger, errorService)
        {
            public string BackLinkValue => BackLink;
        }
    }
}
