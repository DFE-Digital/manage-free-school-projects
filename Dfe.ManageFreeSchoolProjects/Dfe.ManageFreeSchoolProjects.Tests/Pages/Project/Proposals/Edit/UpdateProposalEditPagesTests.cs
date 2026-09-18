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
    /// The single field "update a proposal" pages. They all share a base model, so the shared
    /// behaviour is covered once and each page then covers the answer it is responsible for.
    /// </summary>
    public class UpdateProposalEditPagesTests
    {
        private const string ProjectId = UpdateProposalPageHarness.ProjectId;
        private const string Rid = UpdateProposalPageHarness.Rid;

        private static string DetailsUrl => string.Format(RouteConstants.Proposals_Details, ProjectId, Rid);

        [Fact]
        public async Task OnGet_WhenTheProposalCannotBeFound_Returns404()
        {
            var harness = new UpdateProposalPageHarness().WithNoProposal();
            var model = BuildNameOfDiocese(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task OnGet_ShowsTheStoredAnswerAndLinksBackToTheProposal()
        {
            var harness = new UpdateProposalPageHarness()
                .WithProposal(new ProposalResponse { Rid = Rid, NameOfDiocese = "Diocese of London" });
            var model = BuildNameOfDiocese(harness);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.NameOfDiocese.Should().Be("Diocese of London");
            UpdateProposalPageHarness.BackLinkOf(model).Should().Be(DetailsUrl);
        }

        [Fact]
        public async Task OnPost_WhenTheAnswerIsMissing_RedisplaysThePageWithoutSaving()
        {
            var harness = new UpdateProposalPageHarness();
            var model = BuildNameOfDiocese(harness);
            model.ModelState.AddModelError("name-of-diocese", "Enter the name of the Diocese");

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            harness.SavedRequest.Should().BeNull();
            UpdateProposalPageHarness.BackLinkOf(model).Should().Be(DetailsUrl);
        }

        [Fact]
        public async Task OnPost_SavesAgainstTheProposalAndReturnsToTheDetailsPage()
        {
            var harness = new UpdateProposalPageHarness()
                .WithProposal(new ProposalResponse { Rid = Rid, Proposer = ProposalProposer.Diocese });
            var model = BuildNameOfDiocese(harness);
            model.NameOfDiocese = "Diocese of Bath";

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>().Which.Url.Should().Be(DetailsUrl);

            var saved = harness.SavedRequest;
            saved.Rid.Should().Be(Rid);
            saved.Proposer.Should().Be(ProposalProposer.Diocese);
            saved.NameOfDiocese.Should().Be("Diocese of Bath");
        }

        [Fact]
        public async Task NameOfOtherReligiousOrganisation_ShowsAndSavesTheName()
        {
            var harness = new UpdateProposalPageHarness().WithProposal(new ProposalResponse
            {
                Rid = Rid,
                Proposer = ProposalProposer.AnotherReligiousOrganisation,
                NameOfOtherReligiousOrganisation = "Existing organisation"
            });
            var model = new NameOfOtherReligiousOrganisationModel(
                harness.GetProposalService,
                harness.UpdateProposalService,
                Substitute.For<ILogger<NameOfOtherReligiousOrganisationModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                Rid = Rid,
                PageContext = UpdateProposalPageHarness.BuildPageContext()
            };

            await model.OnGet();
            model.NameOfOtherReligiousOrganisation.Should().Be("Existing organisation");

            model.NameOfOtherReligiousOrganisation = "New organisation";
            await model.OnPost();

            harness.SavedRequest.NameOfOtherReligiousOrganisation.Should().Be("New organisation");
        }

        [Fact]
        public async Task FaithOfDiocese_ShowsAndSavesTheFaith()
        {
            var harness = new UpdateProposalPageHarness().WithProposal(new ProposalResponse
            {
                Rid = Rid,
                Proposer = ProposalProposer.Diocese,
                FaithOfDiocese = FaithOfDiocese.ChurchOfEngland
            });
            var model = new FaithOfDioceseModel(
                harness.GetProposalService,
                harness.UpdateProposalService,
                Substitute.For<ILogger<FaithOfDioceseModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                Rid = Rid,
                PageContext = UpdateProposalPageHarness.BuildPageContext()
            };

            await model.OnGet();
            model.FaithOfDiocese.Should().Be(FaithOfDiocese.ChurchOfEngland);

            model.FaithOfDiocese = FaithOfDiocese.RomanCatholic;
            await model.OnPost();

            harness.SavedRequest.FaithOfDiocese.Should().Be(FaithOfDiocese.RomanCatholic);
        }

        [Theory]
        [InlineData(FaithType.Other, "Jain", "Jain")]
        [InlineData(FaithType.Muslim, "Jain", null)]
        public async Task FaithOfOtherReligiousOrganisation_KeepsTheFreeTextOnlyWhenOtherIsChosen(
            FaithType chosen, string otherFaith, string? expected)
        {
            var harness = new UpdateProposalPageHarness().WithProposal(new ProposalResponse
            {
                Rid = Rid,
                Proposer = ProposalProposer.AnotherReligiousOrganisation
            });
            var model = BuildFaithOfOtherReligiousOrganisation(harness);
            model.FaithTypeOfOtherReligiousOrganisation = chosen;
            model.OtherFaithType = otherFaith;

            await model.OnPost();

            var saved = harness.SavedRequest;
            saved.FaithTypeOfOtherReligiousOrganisation.Should().Be(chosen);
            saved.OtherFaithTypeOfOtherReligiousOrganisation.Should().Be(expected);
        }

        [Fact]
        public async Task FaithOfOtherReligiousOrganisation_ShowsTheStoredFaithAndFreeText()
        {
            var harness = new UpdateProposalPageHarness().WithProposal(new ProposalResponse
            {
                Rid = Rid,
                FaithTypeOfOtherReligiousOrganisation = FaithType.Other,
                OtherFaithTypeOfOtherReligiousOrganisation = "Jain"
            });
            var model = BuildFaithOfOtherReligiousOrganisation(harness);

            await model.OnGet();

            model.FaithTypeOfOtherReligiousOrganisation.Should().Be(FaithType.Other);
            model.OtherFaithType.Should().Be("Jain");
        }

        [Fact]
        public async Task ProposedFaithStatus_ShowsAndSavesTheStatus()
        {
            var harness = new UpdateProposalPageHarness().WithProposal(new ProposalResponse
            {
                Rid = Rid,
                Proposer = ProposalProposer.Diocese,
                ProposedFaithStatus = FaithStatus.Ethos
            });
            var model = new ProposedFaithStatusModel(
                harness.GetProposalService,
                harness.UpdateProposalService,
                Substitute.For<ILogger<ProposedFaithStatusModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                Rid = Rid,
                PageContext = UpdateProposalPageHarness.BuildPageContext()
            };

            await model.OnGet();
            model.Status.Should().Be(FaithStatus.Ethos);

            model.Status = FaithStatus.Designation;
            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>().Which.Url.Should().Be(DetailsUrl);
            harness.SavedRequest.ProposedFaithStatus.Should().Be(FaithStatus.Designation);
        }

        [Theory]
        [InlineData(FaithType.Other, "Jain", "Jain")]
        [InlineData(FaithType.Hindu, "Jain", "")]
        public async Task ProposedFaithType_KeepsTheFreeTextOnlyWhenOtherIsChosen(
            FaithType chosen, string otherFaith, string? expected)
        {
            var harness = new UpdateProposalPageHarness();
            var model = BuildProposedFaithType(harness);
            model.FaithType = chosen;
            model.OtherFaithType = otherFaith;

            await model.OnPost();

            var saved = harness.SavedRequest;
            saved.ProposedFaithType.Should().Be(chosen);
            saved.OtherFaithType.Should().Be(expected);
        }

        [Fact]
        public async Task ProposedFaithType_ShowsTheStoredFaithAndFreeText()
        {
            var harness = new UpdateProposalPageHarness().WithProposal(new ProposalResponse
            {
                Rid = Rid,
                ProposedFaithType = FaithType.Other,
                OtherFaithType = "Jain"
            });
            var model = BuildProposedFaithType(harness);

            await model.OnGet();

            model.FaithType.Should().Be(FaithType.Other);
            model.OtherFaithType.Should().Be("Jain");
        }

        private static NameOfDioceseModel BuildNameOfDiocese(UpdateProposalPageHarness harness) =>
            new(harness.GetProposalService,
                harness.UpdateProposalService,
                Substitute.For<ILogger<NameOfDioceseModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                Rid = Rid,
                PageContext = UpdateProposalPageHarness.BuildPageContext()
            };

        private static FaithOfOtherReligiousOrganisationModel BuildFaithOfOtherReligiousOrganisation(
            UpdateProposalPageHarness harness) =>
            new(harness.GetProposalService,
                harness.UpdateProposalService,
                Substitute.For<ILogger<FaithOfOtherReligiousOrganisationModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                Rid = Rid,
                PageContext = UpdateProposalPageHarness.BuildPageContext()
            };

        private static ProposedFaithTypeModel BuildProposedFaithType(UpdateProposalPageHarness harness) =>
            new(harness.GetProposalService,
                harness.UpdateProposalService,
                Substitute.For<ILogger<ProposedFaithTypeModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                Rid = Rid,
                PageContext = UpdateProposalPageHarness.BuildPageContext()
            };
    }
}
