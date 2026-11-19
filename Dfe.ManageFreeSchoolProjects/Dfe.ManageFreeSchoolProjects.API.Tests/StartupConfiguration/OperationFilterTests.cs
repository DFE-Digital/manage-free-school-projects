using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using Dfe.ManageFreeSchoolProjects.API.StartupConfiguration;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi;
using NSubstitute;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.StartupConfiguration
{
    public class OperationFilterTests
    {
        private static OperationFilterContext CreateContext(OpenApiDocument document) =>
            new(new ApiDescription(), Substitute.For<ISchemaGenerator>(), new SchemaRepository(), document, null);

        [Fact]
        public void AuthenticationHeaderOperationFilter_AddsApiKeySecurityRequirement()
        {
            var document = new OpenApiDocument();
            var operation = new OpenApiOperation();

            new AuthenticationHeaderOperationFilter().Apply(operation, CreateContext(document));

            operation.Security.Should().ContainSingle();
            var requirement = operation.Security.Single();
            requirement.Should().ContainSingle();

            var (scheme, scopes) = requirement.Single();
            scheme.Should().BeOfType<OpenApiSecuritySchemeReference>()
                .Which.Reference.Id.Should().Be("ApiKey");
            scopes.Should().BeEmpty();
        }

        [Fact]
        public void AuthenticationHeaderOperationFilter_KeepsExistingSecurityRequirements()
        {
            var existing = new OpenApiSecurityRequirement();
            var operation = new OpenApiOperation { Security = new List<OpenApiSecurityRequirement> { existing } };

            new AuthenticationHeaderOperationFilter().Apply(operation, CreateContext(new OpenApiDocument()));

            operation.Security.Should().HaveCount(2);
            operation.Security.First().Should().BeSameAs(existing);
        }

        [Fact]
        public void UserContextOperationFilter_AddsUserContextHeaders()
        {
            var operation = new OpenApiOperation();

            new UserContextOperationFilter().Apply(operation, CreateContext(new OpenApiDocument()));

            operation.Parameters.Should().HaveCount(2);

            var name = operation.Parameters[0];
            name.Name.Should().Be("x-user-context-name");
            name.In.Should().Be(ParameterLocation.Header);
            name.Required.Should().BeFalse();

            var role = operation.Parameters[1];
            role.Name.Should().Be("x-user-context-role-0");
            role.In.Should().Be(ParameterLocation.Header);
            role.Required.Should().BeTrue();
            role.Examples.Should().ContainKeys("caseworker", "team-leader", "admin");
            role.Examples["admin"].Value.Should().BeAssignableTo<JsonValue>()
                .Which.GetValue<string>().Should().Be("concerns-casework.admin");
        }

        [Fact]
        public void UserContextOperationFilter_AppendsToExistingParameters()
        {
            var existing = new OpenApiParameter { Name = "id", In = ParameterLocation.Path };
            var operation = new OpenApiOperation { Parameters = new List<IOpenApiParameter> { existing } };

            new UserContextOperationFilter().Apply(operation, CreateContext(new OpenApiDocument()));

            operation.Parameters.Should().HaveCount(3);
            operation.Parameters[0].Should().BeSameAs(existing);
        }
    }
}
