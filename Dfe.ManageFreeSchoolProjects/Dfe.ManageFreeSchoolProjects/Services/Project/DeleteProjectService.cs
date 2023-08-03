﻿using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Projects;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels.Project;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Project
{
    public class DeleteProjectService : IDeleteProjectService
    {
        //   public ILogger<DeleteProjectService> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public DeleteProjectService(IHttpClientFactory clientFactory)
        { //(ILogger<DeleteProjectService> logger, IHttpClientFactory clientFactory) { 
          //  _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<long> DeleteProject(string ProjectID)
        {
            //   _logger.LogInformation("DeleteProjectService::DeleteProject execution");
            try
            {
                //        _logger.LogInformation("CaseService::PostCase");

                // Create a request
                var request = new StringContent(
                    JsonSerializer.Serialize(new DeleteProjectRequest() { ProjectId = ProjectID}),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json);

                // Create http client
                var client = _clientFactory.CreateClient();

                // Execute request
                var response = await client.DeleteAsync($"https://localhost:3001/api/Project?projectId={ProjectID}");

                // Check status code
                response.EnsureSuccessStatusCode();

                // Read response content
                var content = await response.Content.ReadAsStringAsync();

                // Deserialize content to POCO
                var apiWrapperNewCaseDto = JsonSerializer.Deserialize<ApiSingleResponseV2<ProjectResponse>>(content);

                // Unwrap response
                if (apiWrapperNewCaseDto is { Data: { } })
                {
                    return apiWrapperNewCaseDto.Data.Id;
                }

                throw new Exception("Academies API error unwrap response");
            }
            catch (Exception ex)
            {
                //       _logger.LogError("CaseService::PostCase::Exception message::{Message}", ex.Message);

                throw;
            }
        }
    }
}
