using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dfe.ManageFreeSchoolProjects.API.StartupConfiguration
{
    public class AuthenticationHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Security ??= new List<OpenApiSecurityRequirement>();

            var securityScheme = new OpenApiSecuritySchemeReference("ApiKey", context.Document);

            operation.Security.Add(new OpenApiSecurityRequirement { { securityScheme, new List<string>() } });
        }
    }
}
