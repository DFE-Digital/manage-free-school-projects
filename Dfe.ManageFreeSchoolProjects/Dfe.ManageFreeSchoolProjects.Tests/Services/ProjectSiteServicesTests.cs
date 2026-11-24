using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using System.Net;
using System.Text;

namespace Dfe.ManageFreeSchoolProjects.Tests.Services
{
    public class ProjectSiteServicesTests
    {
        private const string ProjectId = "FS-1";

        private readonly StubHttpMessageHandler _handler = new();

        [Theory]
        [InlineData(SiteInformationWave.NewSchool, "/api/v1/client/projects/FS-1/sites/new-school")]
        [InlineData(SiteInformationWave.Presumption, "/api/v1/client/projects/FS-1/sites/presumption")]
        public async Task GetProjectSitesService_CallsTheEndpointForTheWave(string wave, string expectedPath)
        {
            var expected = new Fixture().Create<GetProjectSitesResponse>();
            _handler.ResponseBody = JsonConvert.SerializeObject(new ApiSingleResponseV2<GetProjectSitesResponse>(expected));

            var service = new GetProjectSitesService(BuildApiClient());

            var result = await service.Execute(ProjectId, wave);

            _handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
            _handler.LastRequest.RequestUri!.AbsolutePath.Should().Be(expectedPath);
            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(ProjectSiteType.Permanent, "/api/v1/client/projects/FS-1/sites/permanent")]
        [InlineData(ProjectSiteType.Temporary, "/api/v1/client/projects/FS-1/sites/temporary")]
        public async Task UpdateProjectSiteService_PatchesTheSiteType(ProjectSiteType siteType, string expectedPath)
        {
            _handler.ResponseBody = JsonConvert.SerializeObject(new ApiSingleResponseV2<object>(new object()));
            var request = new Fixture().Create<UpdateProjectSiteRequest>();

            var service = new UpdateProjectSiteService(BuildApiClient());

            await service.Execute(ProjectId, request, siteType);

            _handler.LastRequest!.Method.Should().Be(HttpMethod.Patch);
            _handler.LastRequest.RequestUri!.AbsolutePath.Should().Be(expectedPath);

            var sent = JsonConvert.DeserializeObject<UpdateProjectSiteRequest>(_handler.LastRequestBody!);
            sent.Should().BeEquivalentTo(request);
        }

        private MfspApiClient BuildApiClient()
        {
            var clientFactory = Substitute.For<IHttpClientFactory>();
            clientFactory.CreateClient(Arg.Any<string>())
                .Returns(_ => new HttpClient(_handler, disposeHandler: false) { BaseAddress = new Uri("https://localhost") });

            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext());

            return new MfspApiClient(clientFactory, Substitute.For<ILogger<ApiClient>>(), httpContextAccessor);
        }

        private sealed class StubHttpMessageHandler : HttpMessageHandler
        {
            public string ResponseBody { get; set; } = "{}";
            public HttpRequestMessage? LastRequest { get; private set; }
            public string? LastRequestBody { get; private set; }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                LastRequest = request;
                LastRequestBody = request.Content == null ? null : await request.Content.ReadAsStringAsync(cancellationToken);

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(ResponseBody, Encoding.UTF8, "application/json")
                };
            }
        }
    }
}
