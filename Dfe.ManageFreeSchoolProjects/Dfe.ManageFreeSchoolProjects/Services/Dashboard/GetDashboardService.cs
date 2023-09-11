﻿using ConcernsCaseWork.Service.Base;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Dashboard;
using Dfe.ManageFreeSchoolProjects.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Dashboard
{
    public interface IGetDashboardService
    {
        public Task<List<GetDashboardResponse>> Execute(GetDashboardServiceParameters parameters);
    }

    public record GetDashboardServiceParameters
    {
        public string UserId { get; set; }
        public string Project { get; set; }
        public List<string> Regions { get; set; }
        public List<string> LocalAuthorities { get; set; }
    }

    public class GetDashboardService : IGetDashboardService
    {
        private readonly MfspApiClient _apiClient;

        public GetDashboardService(MfspApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<GetDashboardResponse>> Execute(GetDashboardServiceParameters parameters)
        {
            var endpoint = $"/api/v1/client/dashboard";

            QueryString query = new QueryString("");

            if (!string.IsNullOrEmpty(parameters.UserId))
            {
                query = query.Add("userId", parameters.UserId);
            }

            if (!string.IsNullOrEmpty(parameters.Project))
            {
                query = query.Add("project", parameters.Project);
            }

            if (parameters.Regions.Any())
            {
                query = query.Add("regions", string.Join(",", parameters.Regions));
            }

            if (parameters.LocalAuthorities.Any())
            {
                query = query.Add("localAuthorities", string.Join(",", parameters.LocalAuthorities));
            }

            endpoint += query.ToString();

            var result = await _apiClient.Get<ApiListWrapper<GetDashboardResponse>>(endpoint);

            return result.Data.ToList();
        }

    }
}
