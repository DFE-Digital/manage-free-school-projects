using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

public class UserContextOperationFilter : IOperationFilter
{
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		if (operation.Parameters is null)
		{
			operation.Parameters = new List<IOpenApiParameter>();
		}

		operation.Parameters.Add(new OpenApiParameter
		{
			Name = "x-user-context-name",
			In = ParameterLocation.Header,
			Description = "user name",
			Required = false
		});

		operation.Parameters.Add(new OpenApiParameter
		{
			Name = "x-user-context-role-0",
			In = ParameterLocation.Header,
			Description = "user-role. API calls support 1..* users roles in the format x-user-context-role-* where the index starts from zero",
			Examples = new Dictionary<string, IOpenApiExample>()
			{
				{ "caseworker", new OpenApiExample(){ Value = JsonValue.Create("concerns-casework.caseworker") } },
				{ "team-leader", new OpenApiExample(){ Value = JsonValue.Create("concerns-casework.teamleader") } },
				{ "admin", new OpenApiExample(){ Value = JsonValue.Create("concerns-casework.admin") } }
			},
			Required = true,
		});
	}
}
