﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Reports
{
    public interface IAllProjectsReportService
    {
        public Task<Stream> Execute();
        
        public Task<Stream> ExecuteWithFilter(string projectIds);
    }

    public class AllProjectsReportService : IAllProjectsReportService
    {
        private readonly MfspApiClient _apiClient;

        public AllProjectsReportService(MfspApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<Stream> Execute()
        {
            var endpoint = $"/api/v1/client/reports/all-projects-export";

            var result = await _apiClient.GetStream(endpoint);

            return result;
        }
        
        public async Task<Stream> ExecuteWithFilter(string projectIds)
        {
            var endpoint = $"/api/v1/client/reports/filtered-projects-export?projectids=" + projectIds.ToString();

            var result = await _apiClient.GetStream(endpoint);

            return result;
        }
    }
}
