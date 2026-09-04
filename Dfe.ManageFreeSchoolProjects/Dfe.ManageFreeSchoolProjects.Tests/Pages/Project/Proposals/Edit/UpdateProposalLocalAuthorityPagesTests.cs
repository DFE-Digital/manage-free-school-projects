using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Proposals.Edit
{
    /// <summary>
    /// The two "which local authority?" pages, which save both the region chosen on the previous
    /// page and the local authority picked here.
    /// </summary>
    public class UpdateProposalLocalAuthorityPagesTests
    {
        private const string ProjectId = UpdateProposalPageHarness.ProjectId;
        private const string Rid = UpdateProposalPageHarness.Rid;

        [Fact]
        public async Task OtherLocalAuthority_OnGet_ListsTheRegionsAuthoritiesAndPreSelectsTheStoredOne()
        {
            var harness = new UpdateProposalPageHarness()
                .WithProposal(new ProposalResponse { Rid = Rid, OtherLocalAuthority = "Bolton" })
                .WithLocalAuthorities(("350", "Wigan"), ("354", "Bolton"));
            var model = BuildOtherLocalAuthority(harness);
            model.Region = ProjectRegion.NorthWest;

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.LocalAuthority.Should().Be("Bolton");
            model.LocalAuthorities.Should().ContainKeys("350", "354");
        }

        [Fact]
        public async Task OtherLocalAuthority_OnGet_ListsTheAuthoritiesInAlphabeticalOrder()
        {
            var harness = new UpdateProposalPageHarness()
                .WithLocalAuthorities(("350", "Wigan"), ("354", "Bolton"), ("352", "Salford"));
            var model = BuildOtherLocalAuthority(harness);
            model.Region = ProjectRegion.NorthWest;

            await model.OnGet();

            model.LocalAuthorities.Values.Should().ContainInOrder("Bolton", "Salford", "Wigan");
        }

        [Fact]
        public async Task OtherLocalAuthority_OnGet_LinksBackToItsOwnRegionPage()
        {
            var harness = new UpdateProposalPageHarness().WithLocalAuthorities(("354", "Bolton"));
            var model = BuildOtherLocalAuthority(harness);
            model.Region = ProjectRegion.NorthWest;

            await model.OnGet();

            UpdateProposalPageHarness.BackLinkOf(model).Should().Be(
                string.Format(RouteConstants.Proposals_Edit_Other_Local_Authority_Region, ProjectId, Rid));
        }

        [Fact]
        public async Task OtherLocalAuthority_OnPost_SavesTheRegionAndTheAuthority()
        {
            var harness = new UpdateProposalPageHarness()
                .WithProposal(new ProposalResponse
                {
                    Rid = Rid,
                    Proposer = ProposalProposer.AnotherLocalAuthority
                })
                .WithLocalAuthorities(("354", "Bolton"));
            var model = BuildOtherLocalAuthority(harness);
            model.Region = ProjectRegion.NorthWest;
            model.LocalAuthority = "Bolton";

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.Proposals_Details, ProjectId, Rid));

            var saved = harness.SavedRequest;
            saved.OtherLocalAuthority.Should().Be("Bolton");
            saved.OtherLocalAuthorityRegion.Should().Be(ProjectRegion.NorthWest);
            saved.JointProposalLocalAuthority.Should().BeNull();
        }

        [Fact]
        public async Task OtherLocalAuthority_OnPost_WhenNoAuthorityIsChosen_RelistsTheAuthoritiesWithoutSaving()
        {
            var harness = new UpdateProposalPageHarness().WithLocalAuthorities(("354", "Bolton"));
            var model = BuildOtherLocalAuthority(harness);
            model.Region = ProjectRegion.NorthWest;
            model.ModelState.AddModelError("local-authority", "Select the local authority");

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            harness.SavedRequest.Should().BeNull();
            model.LocalAuthorities.Should().ContainKey("354");
        }

        [Fact]
        public async Task JointProposalLocalAuthority_OnPost_SavesTheJointProposalRegionAndAuthority()
        {
            var harness = new UpdateProposalPageHarness()
                .WithProposal(new ProposalResponse { Rid = Rid, Proposer = ProposalProposer.JointProposal })
                .WithLocalAuthorities(("354", "Bolton"));
            var model = BuildJointProposalLocalAuthority(harness);
            model.Region = ProjectRegion.London;
            model.LocalAuthority = "Bolton";

            await model.OnPost();

            var saved = harness.SavedRequest;
            saved.JointProposalLocalAuthority.Should().Be("Bolton");
            saved.JointProposalLocalAuthorityRegion.Should().Be(ProjectRegion.London);
            saved.OtherLocalAuthority.Should().BeNull();
        }

        [Fact]
        public async Task JointProposalLocalAuthority_OnGet_LinksBackToTheJointProposalRegionPage()
        {
            var harness = new UpdateProposalPageHarness().WithLocalAuthorities(("354", "Bolton"));
            var model = BuildJointProposalLocalAuthority(harness);
            model.Region = ProjectRegion.NorthWest;

            await model.OnGet();

            UpdateProposalPageHarness.BackLinkOf(model).Should().Be(
                string.Format(RouteConstants.Proposals_Edit_Joint_Proposal_Region, ProjectId, Rid));
        }

        [Fact]
        public async Task JointProposalLocalAuthority_OnGet_PreSelectsTheStoredJointProposalAuthority()
        {
            var harness = new UpdateProposalPageHarness()
                .WithProposal(new ProposalResponse { Rid = Rid, JointProposalLocalAuthority = "Bolton" })
                .WithLocalAuthorities(("354", "Bolton"));
            var model = BuildJointProposalLocalAuthority(harness);
            model.Region = ProjectRegion.NorthWest;

            await model.OnGet();

            model.LocalAuthority.Should().Be("Bolton");
        }

        private static OtherLocalAuthorityModel BuildOtherLocalAuthority(UpdateProposalPageHarness harness) =>
            new(harness.GetProposalService,
                harness.GetLocalAuthoritiesService,
                harness.UpdateProposalService,
                Substitute.For<ILogger<OtherLocalAuthorityModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                Rid = Rid,
                PageContext = UpdateProposalPageHarness.BuildPageContext()
            };

        private static JointProposalLocalAuthorityModel BuildJointProposalLocalAuthority(
            UpdateProposalPageHarness harness) =>
            new(harness.GetProposalService,
                harness.GetLocalAuthoritiesService,
                harness.UpdateProposalService,
                Substitute.For<ILogger<JointProposalLocalAuthorityModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                Rid = Rid,
                PageContext = UpdateProposalPageHarness.BuildPageContext()
            };
    }
}
