using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Middleware
{
    public class AdminRouteMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public AdminRouteMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/admin"))
            {
                if (_env.IsProduction())
                {
                    context.Response.Redirect("/error");
                    return;
                }
            }

            await _next(context);
        }
    }
}
