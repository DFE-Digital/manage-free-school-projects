using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Proposals
{
    public class ProposalJourneyPagesTests
    {
        private const string ProjectId = CreateProposalPageHarness.ProjectId;

        [Fact]
        public void NameOfDiocese_OnGet_PreFillsTheCachedName()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                NameOfDiocese = "Diocese of Bristol"
            });
            var model = NameOfDiocese(harness);

            var result = model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.NameOfDiocese.Should().Be("Diocese of Bristol");
        }

        [Fact]
        public void NameOfDiocese_OnPost_WhenMissing_ReturnsThePageWithErrors()
        {
            var harness = new CreateProposalPageHarness();
            var model = NameOfDiocese(harness);
            model.ModelState.AddModelError("name-of-diocese", "Enter the name of the Diocese");

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            harness.Cache.DidNotReceive().Update(Arg.Any<CreateProposalCacheItem>());
        }

        [Fact]
        public void NameOfDiocese_OnPost_StoresTheNameAndAsksForTheFaith()
        {
            var cacheItem = new CreateProposalCacheItem();
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = NameOfDiocese(harness);
            model.NameOfDiocese = "Diocese of Bristol";

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.Proposals_Create_Faith_Of_Diocese, ProjectId));
            cacheItem.NameOfDiocese.Should().Be("Diocese of Bristol");
            harness.Cache.Received(1).Update(cacheItem);
        }

        [Fact]
        public void FaithOfDiocese_OnGet_PreSelectsTheCachedFaith()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                FaithOfDiocese = FaithOfDiocese.RomanCatholic
            });
            var model = FaithOfDioceseModelFor(harness);

            model.OnGet();

            model.FaithOfDiocese.Should().Be(FaithOfDiocese.RomanCatholic);
        }

        [Fact]
        public void FaithOfDiocese_OnPost_WhenMissing_ReturnsThePageWithErrors()
        {
            var harness = new CreateProposalPageHarness();
            var model = FaithOfDioceseModelFor(harness);
            model.ModelState.AddModelError("faith-of-diocese", "Select the faith of the diocese");

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
        }

        [Theory]
        [InlineData(FaithOfDiocese.ChurchOfEngland)]
        [InlineData(FaithOfDiocese.RomanCatholic)]
        public void FaithOfDiocese_OnPost_StoresTheFaithAndMovesOn(FaithOfDiocese faith)
        {
            var cacheItem = new CreateProposalCacheItem();
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = FaithOfDioceseModelFor(harness);
            model.FaithOfDiocese = faith;

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.Proposals_Create_Proposed_Faith_Status, ProjectId));
            cacheItem.FaithOfDiocese.Should().Be(faith);
        }

        [Fact]
        public void NameOfOtherReligiousOrganisation_OnGet_PreFillsTheCachedName()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                NameOfOtherReligiousOrganisation = "Some organisation"
            });
            var model = NameOfOtherReligiousOrganisation(harness);

            model.OnGet();

            model.NameOfOtherReligiousOrganisation.Should().Be("Some organisation");
        }

        [Fact]
        public void NameOfOtherReligiousOrganisation_OnPost_WhenMissing_ReturnsThePageWithErrors()
        {
            var harness = new CreateProposalPageHarness();
            var model = NameOfOtherReligiousOrganisation(harness);
            model.ModelState.AddModelError(
                "name-of-other-religious-organisation", "Enter the name of the other religious organisation");

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
        }

        [Fact]
        public void NameOfOtherReligiousOrganisation_OnPost_StoresTheNameAndAsksForTheFaith()
        {
            var cacheItem = new CreateProposalCacheItem();
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = NameOfOtherReligiousOrganisation(harness);
            model.NameOfOtherReligiousOrganisation = "Some organisation";

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>().Which.Url.Should().Be(string.Format(
                RouteConstants.Proposals_Create_Faith_Of_Other_Religious_Organisation, ProjectId));
            cacheItem.NameOfOtherReligiousOrganisation.Should().Be("Some organisation");
        }

        [Fact]
        public void FaithOfOtherReligiousOrganisation_OnGet_PreSelectsTheCachedFaith()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                FaithTypeOfOtherReligiousOrganisation = FaithType.Sikh
            });
            var model = FaithOfOtherReligiousOrganisation(harness);

            model.OnGet();

            model.FaithTypeOfOtherReligiousOrganisation.Should().Be(FaithType.Sikh);
        }

        [Fact]
        public void FaithOfOtherReligiousOrganisation_OnPost_WhenMissing_ReturnsThePageWithErrors()
        {
            var harness = new CreateProposalPageHarness();
            var model = FaithOfOtherReligiousOrganisation(harness);
            model.ModelState.AddModelError(
                "faith-of-other-religious-organisation", "Select the faith of the other religious organisation");

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
        }

        [Fact]
        public void FaithOfOtherReligiousOrganisation_OnPost_StoresTheFaithAndMovesOn()
        {
            var cacheItem = new CreateProposalCacheItem();
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = FaithOfOtherReligiousOrganisation(harness);
            model.FaithTypeOfOtherReligiousOrganisation = FaithType.Jewish;

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.Proposals_Create_Proposed_Faith_Status, ProjectId));
            cacheItem.FaithTypeOfOtherReligiousOrganisation.Should().Be(FaithType.Jewish);
            cacheItem.OtherFaithTypeOfOtherReligiousOrganisation.Should().BeNull();
        }

        [Theory]
        [InlineData(FaithType.Other, "Some faith", "Some faith")]
        [InlineData(FaithType.Other, "  ", null)]
        [InlineData(FaithType.Other, null, null)]
        [InlineData(FaithType.Hindu, "Some faith", null)]
        public void FaithOfOtherReligiousOrganisation_OnPost_OnlyStoresTheOtherFaithWhenItApplies(
            FaithType faith, string otherFaith, string expected)
        {
            var cacheItem = new CreateProposalCacheItem();
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = FaithOfOtherReligiousOrganisation(harness);
            model.FaithTypeOfOtherReligiousOrganisation = faith;
            model.OtherFaithType = otherFaith;

            model.OnPost();

            cacheItem.OtherFaithTypeOfOtherReligiousOrganisation.Should().Be(expected);
        }

        [Fact]
        public void ProposedFaithType_OnGet_PreSelectsTheCachedType()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                ProposedFaithType = FaithType.Methodist
            });
            var model = ProposedFaithType(harness);

            model.OnGet();

            model.FaithType.Should().Be(FaithType.Methodist);
        }

        [Fact]
        public void ProposedFaithType_OnPost_WhenInvalid_ReturnsThePageWithErrors()
        {
            var harness = new CreateProposalPageHarness();
            var model = ProposedFaithType(harness);
            model.ModelState.AddModelError("faith-type", "Select the faith type");

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
        }

        [Theory]
        [InlineData(FaithType.Other, "Some faith", "Some faith")]
        [InlineData(FaithType.Other, "  ", "")]
        [InlineData(FaithType.Other, null, "")]
        [InlineData(FaithType.Muslim, "Some faith", "")]
        public void ProposedFaithType_OnPost_OnlyStoresTheOtherFaithWhenItApplies(
            FaithType faith, string otherFaith, string expected)
        {
            var cacheItem = new CreateProposalCacheItem();
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = ProposedFaithType(harness);
            model.FaithType = faith;
            model.OtherFaithType = otherFaith;

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.Proposals_Create_Check_Your_Answers, ProjectId));
            cacheItem.ProposedFaithType.Should().Be(faith);
            cacheItem.OtherFaithType.Should().Be(expected);
        }

        [Fact]
        public void OtherLocalAuthorityRegion_OnGet_PreSelectsTheCachedRegion()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                OtherLocalAuthorityRegion = ProjectRegion.London
            });
            var model = OtherLocalAuthorityRegion(harness);

            model.OnGet();

            model.Region.Should().Be(nameof(ProjectRegion.London));
        }

        [Fact]
        public void OtherLocalAuthorityRegion_OnPost_WhenMissing_ReturnsThePageWithErrors()
        {
            var harness = new CreateProposalPageHarness();
            var model = OtherLocalAuthorityRegion(harness);
            model.ModelState.AddModelError("region", "Select the region");

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            harness.Cache.DidNotReceive().Update(Arg.Any<CreateProposalCacheItem>());
        }

        [Fact]
        public void OtherLocalAuthorityRegion_OnPost_StoresTheRegionAndAsksForTheAuthority()
        {
            var cacheItem = new CreateProposalCacheItem();
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = OtherLocalAuthorityRegion(harness);
            model.Region = nameof(ProjectRegion.YorkshireAndHumber);

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.Proposals_Create_Other_Local_Authority, ProjectId));
            cacheItem.OtherLocalAuthorityRegion.Should().Be(ProjectRegion.YorkshireAndHumber);
        }

        [Fact]
        public void JointProposalRegion_OnGet_PreSelectsTheCachedRegion()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                JointProposalLocalAuthorityRegion = ProjectRegion.NorthEast
            });
            var model = JointProposalRegion(harness);

            model.OnGet();

            model.Region.Should().Be(nameof(ProjectRegion.NorthEast));
        }

        [Fact]
        public void JointProposalRegion_OnPost_WhenMissing_ReturnsThePageWithErrors()
        {
            var harness = new CreateProposalPageHarness();
            var model = JointProposalRegion(harness);
            model.ModelState.AddModelError("region", "Select the region");

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
        }

        [Fact]
        public void JointProposalRegion_OnPost_StoresTheRegionAndAsksForTheAuthority()
        {
            var cacheItem = new CreateProposalCacheItem();
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = JointProposalRegion(harness);
            model.Region = nameof(ProjectRegion.EastMidlands);

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>().Which.Url.Should().Be(string.Format(
                RouteConstants.Proposals_Create_Joint_Proposal_Local_Authority, ProjectId));
            cacheItem.JointProposalLocalAuthorityRegion.Should().Be(ProjectRegion.EastMidlands);
        }

        private static NameOfDioceseModel NameOfDiocese(CreateProposalPageHarness harness) =>
            new(harness.Cache, Substitute.For<ILogger<NameOfDioceseModel>>(), harness.ErrorService)
            { ProjectId = ProjectId, PageContext = CreateProposalPageHarness.BuildPageContext() };

        private static FaithOfDioceseModel FaithOfDioceseModelFor(CreateProposalPageHarness harness) =>
            new(harness.Cache, Substitute.For<ILogger<FaithOfDioceseModel>>(), harness.ErrorService)
            { ProjectId = ProjectId, PageContext = CreateProposalPageHarness.BuildPageContext() };

        private static NameOfOtherReligiousOrganisationModel NameOfOtherReligiousOrganisation(
            CreateProposalPageHarness harness) =>
            new(harness.Cache, Substitute.For<ILogger<NameOfOtherReligiousOrganisationModel>>(), harness.ErrorService)
            { ProjectId = ProjectId, PageContext = CreateProposalPageHarness.BuildPageContext() };

        private static FaithOfOtherReligiousOrganisationModel FaithOfOtherReligiousOrganisation(
            CreateProposalPageHarness harness) =>
            new(harness.Cache, Substitute.For<ILogger<FaithOfOtherReligiousOrganisationModel>>(), harness.ErrorService)
            { ProjectId = ProjectId, PageContext = CreateProposalPageHarness.BuildPageContext() };

        private static ProposedFaithTypeModel ProposedFaithType(CreateProposalPageHarness harness) =>
            new(harness.Cache, Substitute.For<ILogger<ProposedFaithTypeModel>>(), harness.ErrorService)
            { ProjectId = ProjectId, PageContext = CreateProposalPageHarness.BuildPageContext() };

        private static OtherLocalAuthorityRegionModel OtherLocalAuthorityRegion(CreateProposalPageHarness harness) =>
            new(harness.Cache, Substitute.For<ILogger<OtherLocalAuthorityRegionModel>>(), harness.ErrorService)
            { ProjectId = ProjectId, PageContext = CreateProposalPageHarness.BuildPageContext() };

        private static JointProposalRegionModel JointProposalRegion(CreateProposalPageHarness harness) =>
            new(harness.Cache, Substitute.For<ILogger<JointProposalRegionModel>>(), harness.ErrorService)
            { ProjectId = ProjectId, PageContext = CreateProposalPageHarness.BuildPageContext() };
    }
}
