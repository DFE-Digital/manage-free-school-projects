﻿using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Projects;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels.Project;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using ConcernsCaseWork.Service.Base;
using System.Linq;

namespace Dfe.ManageFreeSchoolProjects.Services.Project
{
    public class GetProjectByIdService : IGetProjectByIdService
    {
        private readonly IHttpClientFactory _clientFactory;

		public GetProjectByIdService(IHttpClientFactory clientFactory)
        { //(ILogger<CreateProjectService> logger, IHttpClientFactory clientFactory) { 
          //  _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<ProjectResponse> GetProject(string ProjectId)
        {
            var Url = "https://localhost:3001/api/Project/Id";

                var request = new HttpRequestMessage(HttpMethod.Get, $"{Url}?projectId={ProjectId}");
                var client = _clientFactory.CreateClient();
                try
                {
                    var response = await client.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();

                    var wrapper = JsonSerializer.Deserialize<ApiListWrapper<ProjectResponse>>(content);

                return wrapper.Data.First();
                }
                catch (Exception ex)
                {
                  //  _logger.LogError(ex, $"Error occured while trying to GetSRMAById");
                    throw;
                }
            }
        }
    }
