using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolSpecificationPublicationDate
{
    public class GetNewSchoolSpecificationPublicationDateTaskService : IGetTaskService
    {
        public async Task<GetProjectByTaskResponse> Get(GetTaskServiceParameters parameters)
        {
            var query = parameters.BaseQuery;

            var result = await query.Select(kpi => new GetProjectByTaskResponse()
            {
                NewSchoolSpecificationPublicationDate = new NewSchoolSpecificationPublicationDateTask
                {
                    NewSchoolSpecificationPublicationDate = kpi.NewSchoolSpecificationPublicationDate
                }
            }).FirstOrDefaultAsync();

            return result;
        }
    }
}
