using Dfe.ManageFreeSchoolProjects.API.Contracts.Dashboard;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Services.Dashboard;
using FluentAssertions;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Services
{
    /// <summary>
    /// The lookup shared by the create and update proposal local authority pages.
    /// </summary>
    public class GetLocalAuthoritiesServiceExtensionsTests
    {
        [Fact]
        public async Task GetByRegion_AsksForTheRegionByItsDescription()
        {
            var service = Substitute.For<IGetLocalAuthoritiesService>();
            service.Execute(Arg.Any<List<string>>()).Returns(new GetLocalAuthoritiesResponse());

            await service.GetByRegion(ProjectRegion.YorkshireAndHumber);

            await service.Received(1).Execute(
                Arg.Is<List<string>>(r => r.Single() == "Yorkshire and the Humber"));
        }

        [Fact]
        public async Task GetByRegion_KeysTheAuthoritiesByCodeAndOrdersThemByName()
        {
            var service = BuildService(("350", "Wigan"), ("354", "Bolton"), ("352", "Salford"));

            var result = await service.GetByRegion(ProjectRegion.NorthWest);

            result.Should().Equal(new Dictionary<string, string>
            {
                ["354"] = "Bolton",
                ["352"] = "Salford",
                ["350"] = "Wigan"
            });
        }

        [Fact]
        public async Task GetByRegion_FlattensEveryRegionInTheResponse()
        {
            var service = Substitute.For<IGetLocalAuthoritiesService>();
            service.Execute(Arg.Any<List<string>>()).Returns(new GetLocalAuthoritiesResponse
            {
                Regions =
                [
                    new RegionResponse
                    {
                        RegionName = "North West",
                        LocalAuthorities = [new LocalAuthorityResponse { LACode = "354", Name = "Bolton" }]
                    },
                    new RegionResponse
                    {
                        RegionName = "London",
                        LocalAuthorities = [new LocalAuthorityResponse { LACode = "202", Name = "Camden" }]
                    }
                ]
            });

            var result = await service.GetByRegion(ProjectRegion.NorthWest);

            result.Should().ContainKeys("354", "202");
        }

        [Fact]
        public async Task GetByRegion_WhenTheRegionHasNoAuthorities_ReturnsNothing()
        {
            var service = BuildService();

            var result = await service.GetByRegion(ProjectRegion.NorthWest);

            result.Should().BeEmpty();
        }

        private static IGetLocalAuthoritiesService BuildService(params (string Code, string Name)[] authorities)
        {
            var service = Substitute.For<IGetLocalAuthoritiesService>();

            service.Execute(Arg.Any<List<string>>()).Returns(new GetLocalAuthoritiesResponse
            {
                Regions =
                [
                    new RegionResponse
                    {
                        RegionName = "Region",
                        LocalAuthorities = authorities
                            .Select(a => new LocalAuthorityResponse { LACode = a.Code, Name = a.Name })
                            .ToList()
                    }
                ]
            });

            return service;
        }
    }
}
