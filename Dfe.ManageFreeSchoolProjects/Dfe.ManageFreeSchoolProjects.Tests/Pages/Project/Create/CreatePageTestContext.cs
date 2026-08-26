using Dfe.ManageFreeSchoolProjects.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Create
{
    internal static class CreatePageTestContext
    {
        public static PageContext Build(bool authorised = true)
        {
            var httpContext = new DefaultHttpContext();

            if (authorised)
            {
                httpContext.User = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new[] { new Claim(ClaimTypes.Role, RolesConstants.ProjectRecordCreator) },
                        authenticationType: "Test"));
            }

            var actionContext = new ActionContext(
                httpContext,
                new RouteData(),
                new PageActionDescriptor(),
                new ModelStateDictionary());

            return new PageContext(actionContext);
        }
    }
}
