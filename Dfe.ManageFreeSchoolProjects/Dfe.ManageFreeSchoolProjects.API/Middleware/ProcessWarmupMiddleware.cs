using Dfe.ManageFreeSchoolProjects.API.Diagnostics;

namespace Dfe.ManageFreeSchoolProjects.API.Middleware
{
    public class ProcessWarmupMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context, IProcessWarmupState warmupState)
        {
            if (!context.Request.Path.StartsWithSegments("/health"))
            {
                context.Items[ProcessWarmupState.HttpContextItemKey] = warmupState.MarkBusinessRequest();
            }

            await next(context);
        }
    }
}
