using System.Net;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Enums;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Proposals.Edit
{
    /// <summary>
    /// The two pages that change the trust on a proposal: search for a TRN, then confirm the trust
    /// that was found.
    /// </summary>
    public class UpdateProposalTrustPagesTests
    {
        private const string ProjectId = UpdateProposalPageHarness.ProjectId;
        private const string Rid = UpdateProposalPageHarness.Rid;
        private const string Trn = "TR12345";

        [Fact]
        public async Task SearchTrustByTrn_OnGet_ShowsTheTrustAlreadyOnTheProposal()
        {
            var harness = new UpdateProposalPageHarness()
                .WithProposal(new ProposalResponse { Rid = Rid, TrustReferenceNumber = Trn });
            var model = BuildSearch(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.TRN.Should().Be(Trn);
            UpdateProposalPageHarness.BackLinkOf(model)
                .Should().Be(string.Format(RouteConstants.Proposals_Details, ProjectId, Rid));
        }

        [Fact]
        public async Task SearchTrustByTrn_OnGet_WhenTheProposalCannotBeFound_Returns404()
        {
            var harness = new UpdateProposalPageHarness().WithNoProposal();
            var model = BuildSearch(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task SearchTrustByTrn_OnPost_LooksUpTheTrustAndGoesToConfirmIt()
        {
            var harness = new UpdateProposalPageHarness()
                .WithTrust(new TrustTask { TRN = Trn, TrustName = "Test Trust" });
            var model = BuildSearch(harness);
            model.TRN = Trn;

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>().Which.Url.Should().Be(
                string.Format(RouteConstants.Proposals_Edit_Confirm_Trust, ProjectId, Rid, Trn));
            await harness.GetTrustByRefService.Received(1).Execute(Trn);
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("TR123")]
        [InlineData("NOTATRN")]
        public async Task SearchTrustByTrn_OnPost_WhenTheTrnIsMalformed_RedisplaysThePageWithAnError(string trn)
        {
            var harness = new UpdateProposalPageHarness();
            var model = BuildSearch(harness);
            model.TRN = trn;

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            await harness.GetTrustByRefService.DidNotReceiveWithAnyArgs().Execute(default);
        }

        [Fact]
        public async Task SearchTrustByTrn_OnPost_WhenNothingIsEntered_RedisplaysThePageWithAnError()
        {
            var harness = new UpdateProposalPageHarness();
            var model = BuildSearch(harness);
            model.ModelState.AddModelError("trn", "Enter the TRN");

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
        }

        [Fact]
        public async Task ConfirmTrust_OnGet_ShowsTheTrustThatWasSearchedFor()
        {
            var trust = new TrustTask { TRN = Trn, TrustName = "Test Trust" };
            var harness = new UpdateProposalPageHarness().WithTrust(trust);
            var model = BuildConfirm(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Trust.Should().BeSameAs(trust);
            UpdateProposalPageHarness.BackLinkOf(model).Should().Be(
                string.Format(RouteConstants.Proposals_Edit_SearchTrustByTRN, ProjectId, Rid));
        }

        [Fact]
        public async Task ConfirmTrust_OnGet_WhenTheProposalCannotBeFound_Returns404()
        {
            var harness = new UpdateProposalPageHarness().WithNoProposal();
            var model = BuildConfirm(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ConfirmTrust_OnPost_WhenConfirmed_SavesTheTrustAndReturnsToTheDetailsPage()
        {
            var harness = new UpdateProposalPageHarness()
                .WithProposal(new ProposalResponse { Rid = Rid, Proposer = ProposalProposer.AcademyTrust })
                .WithTrust(new TrustTask
                {
                    TRN = Trn,
                    TrustName = "Test Trust",
                    TrustType = TrustType.MultiAcademyTrust
                });
            var model = BuildConfirm(harness);
            model.ConfirmOption = YesNoOption.Yes;

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.Proposals_Details, ProjectId, Rid));

            var saved = harness.SavedRequest;
            saved.TrustReferenceNumber.Should().Be(Trn);
            saved.TrustName.Should().Be("Test Trust");
            saved.TrustType.Should().Be(TrustType.MultiAcademyTrust);
        }

        [Fact]
        public async Task ConfirmTrust_OnPost_WhenTheTrustIsWrong_GoesBackToTheSearchWithoutSaving()
        {
            var harness = new UpdateProposalPageHarness();
            var model = BuildConfirm(harness);
            model.ConfirmOption = YesNoOption.No;

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>().Which.Url.Should().Be(
                string.Format(RouteConstants.Proposals_Edit_SearchTrustByTRN, ProjectId, Rid));
            harness.SavedRequest.Should().BeNull();
        }

        [Fact]
        public async Task ConfirmTrust_OnPost_WhenNothingIsSelected_RedisplaysThePageWithoutSaving()
        {
            var harness = new UpdateProposalPageHarness();
            var model = BuildConfirm(harness);
            model.ModelState.AddModelError(
                nameof(ConfirmTrustModel.ConfirmOption), "Please select an option to confirm the trust");

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            harness.SavedRequest.Should().BeNull();
        }

        [Fact]
        public async Task ConfirmTrust_OnPost_WhenTheTrustHasGone_RedisplaysThePageWithAnError()
        {
            var harness = new UpdateProposalPageHarness();
            harness.GetTrustByRefService.Execute(Arg.Any<string>())
                .Throws(new HttpRequestException("not found", null, HttpStatusCode.NotFound));
            var model = BuildConfirm(harness);
            model.ConfirmOption = YesNoOption.Yes;

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            harness.SavedRequest.Should().BeNull();
        }

        [Fact]
        public async Task ConfirmTrust_OnPost_WhenTheLookupFailsForAnotherReason_LetsTheErrorBubbleUp()
        {
            var harness = new UpdateProposalPageHarness();
            harness.GetTrustByRefService.Execute(Arg.Any<string>())
                .Throws(new HttpRequestException("boom", null, HttpStatusCode.InternalServerError));
            var model = BuildConfirm(harness);
            model.ConfirmOption = YesNoOption.Yes;

            await model.Invoking(m => m.OnPost()).Should().ThrowAsync<HttpRequestException>();
        }

        private static SearchTrustByTrnModel BuildSearch(UpdateProposalPageHarness harness) =>
            new(harness.GetTrustByRefService,
                harness.GetProposalService,
                Substitute.For<ILogger<SearchTrustByTrnModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                Rid = Rid,
                PageContext = UpdateProposalPageHarness.BuildPageContext()
            };

        private static ConfirmTrustModel BuildConfirm(UpdateProposalPageHarness harness) =>
            new(harness.GetTrustByRefService,
                harness.GetProposalService,
                harness.UpdateProposalService,
                Substitute.For<ILogger<ConfirmTrustModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                Rid = Rid,
                Trn = Trn,
                PageContext = UpdateProposalPageHarness.BuildPageContext()
            };
    }
}
