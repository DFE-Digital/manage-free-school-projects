using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
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
    /// The two "which region is the local authority in?" pages. They save nothing themselves: the
    /// chosen region is carried into the local authority page.
    /// </summary>
    public class UpdateProposalRegionPagesTests
    {
        private const string ProjectId = UpdateProposalPageHarness.ProjectId;
        private const string Rid = UpdateProposalPageHarness.Rid;

        [Fact]
        public async Task OtherLocalAuthorityRegion_OnGet_PreSelectsTheStoredRegion()
        {
            var harness = new UpdateProposalPageHarness().WithProposal(new ProposalResponse
            {
                Rid = Rid,
                OtherLocalAuthorityRegion = "North West"
            });
            var model = BuildOtherLocalAuthorityRegion(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Region.Should().Be(nameof(ProjectRegion.NorthWest));
            UpdateProposalPageHarness.BackLinkOf(model)
                .Should().Be(string.Format(RouteConstants.Proposals_Details, ProjectId, Rid));
        }

        [Fact]
        public async Task OtherLocalAuthorityRegion_OnGet_WhenNoRegionIsStored_SelectsNothing()
        {
            var harness = new UpdateProposalPageHarness()
                .WithProposal(new ProposalResponse { Rid = Rid, OtherLocalAuthorityRegion = null });
            var model = BuildOtherLocalAuthorityRegion(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Region.Should().BeNull();
        }

        [Fact]
        public async Task OtherLocalAuthorityRegion_OnGet_WhenTheProposalCannotBeFound_Returns404()
        {
            var harness = new UpdateProposalPageHarness().WithNoProposal();
            var model = BuildOtherLocalAuthorityRegion(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void OtherLocalAuthorityRegion_OnPost_ContinuesToItsOwnLocalAuthorityPage()
        {
            var harness = new UpdateProposalPageHarness();
            var model = BuildOtherLocalAuthorityRegion(harness);
            model.Region = nameof(ProjectRegion.London);

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>().Which.Url.Should().Be(
                string.Format(RouteConstants.Proposals_Edit_Other_Local_Authority,
                    ProjectId, Rid, (int)ProjectRegion.London));
        }

        [Fact]
        public void OtherLocalAuthorityRegion_OnPost_WhenNoRegionIsChosen_RedisplaysThePage()
        {
            var harness = new UpdateProposalPageHarness();
            var model = BuildOtherLocalAuthorityRegion(harness);
            model.ModelState.AddModelError("region", "Select the region");

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
        }

        [Fact]
        public async Task JointProposalRegion_OnGet_PreSelectsTheStoredJointProposalRegion()
        {
            var harness = new UpdateProposalPageHarness().WithProposal(new ProposalResponse
            {
                Rid = Rid,
                JointProposalLocalAuthorityRegion = "South West"
            });
            var model = BuildJointProposalRegion(harness);

            await model.OnGet();

            model.Region.Should().Be(nameof(ProjectRegion.SouthWest));
        }

        [Fact]
        public void JointProposalRegion_OnPost_ContinuesToTheJointProposalLocalAuthorityPage()
        {
            var harness = new UpdateProposalPageHarness();
            var model = BuildJointProposalRegion(harness);
            model.Region = nameof(ProjectRegion.EastMidlands);

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>().Which.Url.Should().Be(
                string.Format(RouteConstants.Proposals_Edit_Joint_Proposal_Local_Authority,
                    ProjectId, Rid, (int)ProjectRegion.EastMidlands));
        }

        private static OtherLocalAuthorityRegionModel BuildOtherLocalAuthorityRegion(
            UpdateProposalPageHarness harness) =>
            new(harness.GetProposalService,
                Substitute.For<ILogger<OtherLocalAuthorityRegionModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                Rid = Rid,
                PageContext = UpdateProposalPageHarness.BuildPageContext()
            };

        private static JointProposalRegionModel BuildJointProposalRegion(UpdateProposalPageHarness harness) =>
            new(harness.GetProposalService,
                Substitute.For<ILogger<JointProposalRegionModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                Rid = Rid,
                PageContext = UpdateProposalPageHarness.BuildPageContext()
            };
    }
}
