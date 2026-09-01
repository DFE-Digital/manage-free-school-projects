using Dfe.ManageFreeSchoolProjects.API.Contracts.Dashboard;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Dashboard;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Proposals
{
    public class LocalAuthorityModelTests
    {
        private const string ProjectId = CreateProposalPageHarness.ProjectId;

        [Fact]
        public async Task OnGet_ListsTheAuthoritiesForTheChosenRegion()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                OtherLocalAuthorityRegion = ProjectRegion.SouthWest,
                OtherLocalAuthority = "Bristol City Council"
            });
            var model = BuildOtherModel(harness, out var lookup);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.LocalAuthority.Should().Be("Bristol City Council");
            model.LocalAuthorities.Should().ContainKey("801").WhoseValue.Should().Be("Bristol City Council");
            await lookup.Received(1).Execute(Arg.Is<List<string>>(r => r.Single() == "South West"));
        }

        [Fact]
        public async Task OnGet_OrdersTheAuthoritiesByName()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                OtherLocalAuthorityRegion = ProjectRegion.SouthWest
            });
            var model = BuildOtherModel(harness, out _);

            await model.OnGet();

            model.LocalAuthorities.Values.Should().ContainInOrder(
                "Bath and North East Somerset", "Bristol City Council", "Cornwall");
        }

        [Fact]
        public async Task OnPost_WhenNoAuthorityIsSelected_RepopulatesTheListAndShowsErrors()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                OtherLocalAuthorityRegion = ProjectRegion.SouthWest
            });
            var model = BuildOtherModel(harness, out _);
            model.ModelState.AddModelError("local-authority", "Select the local authority");

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            model.LocalAuthorities.Should().NotBeEmpty();
            harness.Cache.DidNotReceive().Update(Arg.Any<CreateProposalCacheItem>());
        }

        [Fact]
        public async Task OnPost_StoresTheAuthorityAndItsCode()
        {
            var cacheItem = new CreateProposalCacheItem { OtherLocalAuthorityRegion = ProjectRegion.SouthWest };
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = BuildOtherModel(harness, out _);
            model.LocalAuthority = "Cornwall";

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.Proposals_Create_Proposed_Faith_Status, ProjectId));
            cacheItem.OtherLocalAuthority.Should().Be("Cornwall");
            cacheItem.OtherLocalAuthorityCode.Should().Be("908");
            harness.Cache.Received(1).Update(cacheItem);
        }

        [Fact]
        public async Task JointProposal_OnGet_ListsTheAuthoritiesForItsOwnRegion()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                JointProposalLocalAuthorityRegion = ProjectRegion.SouthWest,
                JointProposalLocalAuthority = "Cornwall"
            });
            var model = BuildJointModel(harness, out var lookup);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.LocalAuthority.Should().Be("Cornwall");
            await lookup.Received(1).Execute(Arg.Is<List<string>>(r => r.Single() == "South West"));
        }

        [Fact]
        public async Task JointProposal_OnPost_StoresTheAuthorityAndItsCode()
        {
            var cacheItem = new CreateProposalCacheItem
            {
                JointProposalLocalAuthorityRegion = ProjectRegion.SouthWest
            };
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = BuildJointModel(harness, out _);
            model.LocalAuthority = "Bristol City Council";

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.Proposals_Create_Proposed_Faith_Status, ProjectId));
            cacheItem.JointProposalLocalAuthority.Should().Be("Bristol City Council");
            cacheItem.JointProposalLocalAuthorityCode.Should().Be("801");
        }

        [Fact]
        public async Task JointProposal_OnPost_WhenNoAuthorityIsSelected_RepopulatesTheList()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                JointProposalLocalAuthorityRegion = ProjectRegion.SouthWest
            });
            var model = BuildJointModel(harness, out _);
            model.ModelState.AddModelError("local-authority", "Select the local authority");

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            model.LocalAuthorities.Should().NotBeEmpty();
        }

        private static IGetLocalAuthoritiesService BuildLookup()
        {
            var lookup = Substitute.For<IGetLocalAuthoritiesService>();

            lookup.Execute(Arg.Any<List<string>>()).Returns(new GetLocalAuthoritiesResponse
            {
                Regions =
                [
                    new RegionResponse
                    {
                        RegionName = "South West",
                        LocalAuthorities =
                        [
                            new LocalAuthorityResponse { LACode = "801", Name = "Bristol City Council" },
                            new LocalAuthorityResponse { LACode = "908", Name = "Cornwall" },
                            new LocalAuthorityResponse { LACode = "800", Name = "Bath and North East Somerset" }
                        ]
                    }
                ]
            });

            return lookup;
        }

        private static LocalAuthorityModel BuildOtherModel(
            CreateProposalPageHarness harness, out IGetLocalAuthoritiesService lookup)
        {
            lookup = BuildLookup();

            return new LocalAuthorityModel(
                harness.Cache,
                lookup,
                Substitute.For<ILogger<LocalAuthorityModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                PageContext = CreateProposalPageHarness.BuildPageContext()
            };
        }

        private static JointProposalLocalAuthorityModel BuildJointModel(
            CreateProposalPageHarness harness, out IGetLocalAuthoritiesService lookup)
        {
            lookup = BuildLookup();

            return new JointProposalLocalAuthorityModel(
                harness.Cache,
                lookup,
                Substitute.For<ILogger<JointProposalLocalAuthorityModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                PageContext = CreateProposalPageHarness.BuildPageContext()
            };
        }
    }
}
