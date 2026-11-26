using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;

namespace Dfe.ManageFreeSchoolProjects.Authorization
{
    public static class AutomationHandler
    {
        public static bool ClientSecretHeaderValid(IHostEnvironment hostEnvironment,
            IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            //Header authorisation not applicable for production
            if (hostEnvironment.IsProduction())
            {
                return false;
            }

            //Allow client secret in header
            var authHeader = httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString()?
                .Replace("Bearer ", string.Empty);

            var secret = configuration.GetValue<string>("CypressTestSecret");

            if (string.IsNullOrWhiteSpace(secret))
            {
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(authHeader))
            {
                return false;
            }
            return authHeader == secret;
        }
    }
}
