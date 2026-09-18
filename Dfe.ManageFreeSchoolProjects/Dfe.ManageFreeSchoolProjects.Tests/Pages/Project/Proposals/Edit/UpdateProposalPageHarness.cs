using System.Reflection;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Dashboard;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Dashboard;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Dfe.ManageFreeSchoolProjects.Services.Trust;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit;
using Microsoft.AspNetCore.Routing;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Proposals.Edit
{
    /// <summary>
    /// Builds the substitutes every "update a proposal" page needs, and captures the update request
    /// the page sends so tests can assert on what would be saved.
    /// </summary>
    internal sealed class UpdateProposalPageHarness
    {
        public const string ProjectId = "NEW-SCHOOL-1";
        public const string Rid = "RID-1";

        public IGetProposalService GetProposalService { get; } = Substitute.For<IGetProposalService>();
        public IUpdateProposalService UpdateProposalService { get; } = Substitute.For<IUpdateProposalService>();
        public IGetLocalAuthoritiesService GetLocalAuthoritiesService { get; } = Substitute.For<IGetLocalAuthoritiesService>();
        public IGetTrustByRefService GetTrustByRefService { get; } = Substitute.For<IGetTrustByRefService>();
        public ErrorService ErrorService { get; } = new();

        public UpdateProposalPageHarness()
        {
            WithProposal(new ProposalResponse { Rid = Rid, ProjectId = ProjectId });
        }

        public UpdateProposalPageHarness WithProposal(ProposalResponse proposal)
        {
            GetProposalService.ExecuteSingle(Rid).Returns(new ApiSingleResponseV2<ProposalResponse>(proposal));
            return this;
        }

        /// <summary>The proposal cannot be found, so pages should return a 404.</summary>
        public UpdateProposalPageHarness WithNoProposal()
        {
            GetProposalService.ExecuteSingle(Rid).Returns(new ApiSingleResponseV2<ProposalResponse>(null!));
            return this;
        }

        public UpdateProposalPageHarness WithLocalAuthorities(params (string Code, string Name)[] authorities)
        {
            var response = new GetLocalAuthoritiesResponse
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
            };

            GetLocalAuthoritiesService.Execute(Arg.Any<List<string>>()).Returns(response);
            return this;
        }

        public UpdateProposalPageHarness WithTrust(TrustTask trust)
        {
            GetTrustByRefService.Execute(Arg.Any<string>())
                .Returns(new GetTrustByRefResponse { Trust = trust });
            return this;
        }

        /// <summary>
        /// The single update request the page sent. Null when the page never saved, which tests
        /// assert on directly, so it is not declared nullable.
        /// </summary>
        public UpdateProposalRequest SavedRequest =>
            UpdateProposalService.ReceivedCalls()
                .Where(c => c.GetMethodInfo().Name == nameof(IUpdateProposalService.Execute))
                .Select(c => (UpdateProposalRequest)c.GetArguments()[0]!)
                .SingleOrDefault()!;

        public static PageContext BuildPageContext()
        {
            var routeData = new RouteData();
            routeData.Values["projectId"] = ProjectId;
            routeData.Values["rid"] = Rid;

            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                routeData,
                new PageActionDescriptor(),
                new ModelStateDictionary());

            return new PageContext(actionContext);
        }

        /// <summary>
        /// Reads the back link off a page. It is declared "protected internal" on the shared base
        /// model, so it is not directly reachable from this assembly.
        /// </summary>
        public static string? BackLinkOf(UpdateProposalBaseModel page)
        {
            var property = typeof(UpdateProposalBaseModel).GetProperty(
                "BackLink", BindingFlags.Instance | BindingFlags.NonPublic);

            return (string?)property!.GetValue(page);
        }
    }
}
