using Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create;
using Dfe.ManageFreeSchoolProjects.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Proposals
{
    internal sealed class CreateProposalPageHarness
    {
        public const string ProjectId = "NEW-SCHOOL-1";

        public ICreateProposalCache Cache { get; } = Substitute.For<ICreateProposalCache>();
        public ErrorService ErrorService { get; } = new ErrorService();

        public CreateProposalCacheItem CacheItem { get; private set; } = new();

        public CreateProposalPageHarness()
        {
            Cache.Get().Returns(_ => CacheItem);
        }

        public CreateProposalPageHarness With(CreateProposalCacheItem cacheItem)
        {
            CacheItem = cacheItem;
            return this;
        }

        public static PageContext BuildPageContext()
        {
            var routeData = new RouteData();
            routeData.Values["projectId"] = ProjectId;

            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                routeData,
                new PageActionDescriptor(),
                new ModelStateDictionary());

            return new PageContext(actionContext);
        }
    }
}
