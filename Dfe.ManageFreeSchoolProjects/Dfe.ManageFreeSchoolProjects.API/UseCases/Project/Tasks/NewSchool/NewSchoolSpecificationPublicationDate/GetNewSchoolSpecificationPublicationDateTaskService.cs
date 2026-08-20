using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Data;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolSpecificationPublicationDate
{
    public class GetNewSchoolSpecificationPublicationDateTaskService : IGetTaskService
    {
        private readonly MfspContext _context;

        public GetNewSchoolSpecificationPublicationDateTaskService(MfspContext context)
        {
            _context = context;
        }

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
