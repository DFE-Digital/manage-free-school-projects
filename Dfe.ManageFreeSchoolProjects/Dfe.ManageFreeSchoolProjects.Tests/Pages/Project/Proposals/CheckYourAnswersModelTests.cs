using System.Net;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Proposals
{
    public class CheckYourAnswersModelTests
    {
        private const string ProjectId = CreateProposalPageHarness.ProjectId;

        [Fact]
        public void OnGet_RecordsThatCheckYourAnswersWasReached()
        {
            var cacheItem = new CreateProposalCacheItem { Proposer = ProposalProposer.Diocese };
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = BuildModel(harness, out _, out _);

            var result = model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Cache.Should().BeSameAs(cacheItem);
            cacheItem.ReachedCheckYourAnswers.Should().BeTrue();
            harness.Cache.Received(1).Update(cacheItem);
        }

        [Fact]
        public async Task OnPostAsync_SendsTheCachedAnswersToTheApi()
        {
            var cacheItem = new CreateProposalCacheItem
            {
                Proposer = ProposalProposer.AcademyTrust,
                Trust = new TrustTask
                {
                    TRN = "TR12345",
                    TrustName = "Test Trust",
                    TrustType = TrustType.MultiAcademyTrust
                },
                NameOfDiocese = "Diocese of Bristol",
                FaithOfDiocese = FaithOfDiocese.ChurchOfEngland,
                NameOfOtherReligiousOrganisation = "Other org",
                FaithTypeOfOtherReligiousOrganisation = FaithType.Hindu,
                OtherFaithTypeOfOtherReligiousOrganisation = "Other faith",
                OtherLocalAuthorityRegion = ProjectRegion.London,
                OtherLocalAuthority = "Bristol City Council",
                JointProposalLocalAuthorityRegion = ProjectRegion.SouthWest,
                JointProposalLocalAuthority = "Bath and North East Somerset",
                ProposedFaithStatus = FaithStatus.Designation,
                ProposedFaithType = FaithType.RomanCatholic,
                OtherFaithType = "Some other faith"
            };
            var harness = new CreateProposalPageHarness().With(cacheItem);
            var model = BuildModel(harness, out _, out var captured);

            var result = await model.OnPostAsync();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.Proposals, ProjectId));

            var request = captured.Value;
            request.Should().NotBeNull();
            request!.ProjectId.Should().Be(ProjectId);
            request.Proposer.Should().Be(ProposalProposer.AcademyTrust);
            request.TrustReferenceNumber.Should().Be("TR12345");
            request.TrustName.Should().Be("Test Trust");
            request.TrustType.Should().Be(TrustType.MultiAcademyTrust);
            request.NameOfDiocese.Should().Be("Diocese of Bristol");
            request.FaithOfDiocese.Should().Be(FaithOfDiocese.ChurchOfEngland);
            request.NameOfOtherReligiousOrganisation.Should().Be("Other org");
            request.FaithTypeOfOtherReligiousOrganisation.Should().Be(FaithType.Hindu);
            request.OtherFaithTypeOfOtherReligiousOrganisation.Should().Be("Other faith");
            request.OtherLocalAuthorityRegion.Should().Be(ProjectRegion.London);
            request.OtherLocalAuthority.Should().Be("Bristol City Council");
            request.JointProposalLocalAuthorityRegion.Should().Be(ProjectRegion.SouthWest);
            request.JointProposalLocalAuthority.Should().Be("Bath and North East Somerset");
            request.ProposedFaithStatus.Should().Be(FaithStatus.Designation);
            request.ProposedFaithType.Should().Be(FaithType.RomanCatholic);
            request.OtherFaithType.Should().Be("Some other faith");
        }

        [Fact]
        public async Task OnPostAsync_WhenThereIsNoTrust_SendsTheTrustFieldsAsNull()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                Proposer = ProposalProposer.AnotherLocalAuthority,
                Trust = null
            });
            var model = BuildModel(harness, out _, out var captured);

            await model.OnPostAsync();

            captured.Value!.TrustReferenceNumber.Should().BeNull();
            captured.Value!.TrustName.Should().BeNull();
            captured.Value!.TrustType.Should().BeNull();
        }

        [Fact]
        public async Task OnPostAsync_WhenTheApiFails_ShowsAnErrorAndStaysOnThePage()
        {
            var harness = new CreateProposalPageHarness().With(new CreateProposalCacheItem
            {
                Proposer = ProposalProposer.Diocese
            });
            var model = BuildModel(harness, out var createService, out _);
            createService.Execute(Arg.Any<CreateProposalRequest>())
                .Returns<Task<CreateProposalResponse>>(_ => throw new HttpRequestException(
                    "boom", null, HttpStatusCode.InternalServerError));

            var result = await model.OnPostAsync();

            result.Should().BeOfType<PageResult>();
            harness.ErrorService.HasErrors().Should().BeTrue();
            harness.ErrorService.GetError("projectid").Message
                .Should().Be("Error occurred while creating proposal.");
        }

        private static CheckYourAnswersModel BuildModel(
            CreateProposalPageHarness harness,
            out ICreateProposalService createService,
            out CapturedRequest captured)
        {
            createService = Substitute.For<ICreateProposalService>();
            var capturedRequest = new CapturedRequest();
            captured = capturedRequest;

            createService.Execute(Arg.Any<CreateProposalRequest>())
                .Returns(call =>
                {
                    capturedRequest.Value = call.Arg<CreateProposalRequest>();
                    return Task.FromResult(new CreateProposalResponse());
                });

            return new CheckYourAnswersModel(
                harness.Cache,
                createService,
                Substitute.For<ILogger<CheckYourAnswersModel>>(),
                harness.ErrorService)
            {
                ProjectId = ProjectId,
                PageContext = CreateProposalPageHarness.BuildPageContext()
            };
        }

        internal sealed class CapturedRequest
        {
            public CreateProposalRequest? Value { get; set; }
        }
    }
}
