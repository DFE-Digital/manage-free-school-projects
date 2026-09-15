using System.Net;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Trust;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Proposals
{
    public class SearchTrustByTrnModelTests
    {
        private const string ProjectId = CreateProposalPageHarness.ProjectId;

        [Fact]
        public void OnGet_PreFillsTheTrnAlreadyInTheCache()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                Trust = new TrustTask { TRN = "TR12345" }
            });
            var model = BuildModel(harness, out _);

            var result = model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.TRN.Should().Be("TR12345");
        }

        [Fact]
        public void OnGet_WhenNoTrustHasBeenChosen_LeavesTheTrnEmpty()
        {
            var harness = new CreateProposalPageHarness();
            var model = BuildModel(harness, out _);

            model.OnGet();

            model.TRN.Should().BeNull();
        }

        [Fact]
        public async Task OnPost_WhenTheTrnIsMissing_ReturnsThePageWithErrors()
        {
            var harness = new CreateProposalPageHarness();
            var model = BuildModel(harness, out var trustService);
            model.ModelState.AddModelError("trn", "Enter the TRN");

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            await trustService.DidNotReceiveWithAnyArgs().Execute(default!);
        }

        [Theory]
        [InlineData("ABC1234")]
        [InlineData("TR1234")]
        [InlineData("1234567")]
        [InlineData("TRABCDE")]
        public async Task OnPost_WhenTheTrnIsMalformed_ShowsTheFormatErrorWithoutCallingTheApi(string trn)
        {
            var harness = new CreateProposalPageHarness();
            var model = BuildModel(harness, out var trustService);
            model.TRN = trn;

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.GetError("trn").Message
                .Should().Be("The TRN must start with the letters TR, followed by at least 5 numbers");
            await trustService.DidNotReceiveWithAnyArgs().Execute(default!);
        }

        [Theory]
        [InlineData("TR12345")]
        [InlineData("tr12345")]
        public async Task OnPost_WhenTheTrustExists_CachesItAndMovesOn(string trn)
        {
            var cacheItem = new CreateProposalCacheItem();
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = BuildModel(harness, out var trustService);
            model.TRN = trn;

            var trust = new TrustTask { TRN = trn, TrustName = "Test Trust" };
            trustService.Execute(trn).Returns(new GetTrustByRefResponse { Trust = trust });

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.Proposals_Create_Confirm_Trust, ProjectId));
            cacheItem.Trust.Should().BeSameAs(trust);
            harness.Cache.Received(1).Update(cacheItem);
        }

        [Fact]
        public async Task OnPost_WhenTheTrustIsNotFound_ShowsAnErrorAndStaysOnThePage()
        {
            var harness = new CreateProposalPageHarness();
            var model = BuildModel(harness, out var trustService);
            model.TRN = "TR12345";

            trustService.Execute("TR12345")
                .ThrowsAsync(new HttpRequestException("not found", null, HttpStatusCode.NotFound));

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.GetError("trn").Message
                .Should().Be("Trust ID not found. Enter a different ID");
            harness.Cache.DidNotReceive().Update(Arg.Any<CreateProposalCacheItem>());
        }

        [Fact]
        public async Task OnPost_WhenTheApiFailsForAnotherReason_Rethrows()
        {
            var harness = new CreateProposalPageHarness();
            var model = BuildModel(harness, out var trustService);
            model.TRN = "TR12345";

            trustService.Execute("TR12345")
                .ThrowsAsync(new HttpRequestException("boom", null, HttpStatusCode.InternalServerError));

            await model.Invoking(m => m.OnPost())
                .Should().ThrowAsync<HttpRequestException>()
                .WithMessage("boom");
        }

        [Fact]
        public void OnGet_LinksBackToTheProposer()
        {
            var harness = new CreateProposalPageHarness();
            var model = new TestableSearchTrustByTrnModel(
                harness.Cache,
                Substitute.For<IGetTrustByRefService>(),
                Substitute.For<ILogger<SearchTrustByTrnModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                PageContext = CreateProposalPageHarness.BuildPageContext()
            };

            model.OnGet();

            model.BackLinkValue.Should()
                .Be(string.Format(RouteConstants.Proposals_Create_Proposer, ProjectId));
        }

        private static SearchTrustByTrnModel BuildModel(
            CreateProposalPageHarness harness, out IGetTrustByRefService trustService)
        {
            trustService = Substitute.For<IGetTrustByRefService>();

            return new SearchTrustByTrnModel(
                harness.Cache,
                trustService,
                Substitute.For<ILogger<SearchTrustByTrnModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                PageContext = CreateProposalPageHarness.BuildPageContext()
            };
        }

        private sealed class TestableSearchTrustByTrnModel(
            ICreateProposalCache cache,
            IGetTrustByRefService trustService,
            ILogger<SearchTrustByTrnModel> logger,
            ErrorService errorService)
            : SearchTrustByTrnModel(cache, trustService, logger, errorService)
        {
            public string BackLinkValue => BackLink;
        }
    }
}
