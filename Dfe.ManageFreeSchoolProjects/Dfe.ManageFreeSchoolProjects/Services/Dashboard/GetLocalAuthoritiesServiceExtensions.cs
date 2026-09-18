using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Dashboard
{
    public static class GetLocalAuthoritiesServiceExtensions
    {
        public static async Task<Dictionary<string, string>> GetByRegion(
            this IGetLocalAuthoritiesService service, ProjectRegion? region)
        {
            var response = await service.Execute([region.ToDescription()]);

            var authorities = new Dictionary<string, string>();

            response.Regions.ForEach(r =>
            {
                r.LocalAuthorities.ForEach(authority =>
                {
                    authorities.Add(authority.LACode, authority.Name);
                });
            });

            return authorities.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
