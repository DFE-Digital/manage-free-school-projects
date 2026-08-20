using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Data;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolConditions
{
    public class GetNewSchoolConditionsTaskService : IGetTaskService
    {
        private readonly MfspContext _context;

        public GetNewSchoolConditionsTaskService(MfspContext context)
        {
            _context = context;
        }

        public async Task<GetProjectByTaskResponse> Get(GetTaskServiceParameters parameters)
        {
            var query = parameters.BaseQuery;

            var result = await query.Select(kpi => new GetProjectByTaskResponse()
            {
                NewSchoolConditions = new NewSchoolConditionsTask
                {
                    NewSchoolConditions = kpi.NewSchoolConditions,
                    NewSchoolConditionsDescription = kpi.NewSchoolConditionsDescription
                }
            }).FirstOrDefaultAsync();

            return result;
        }
    }
}
