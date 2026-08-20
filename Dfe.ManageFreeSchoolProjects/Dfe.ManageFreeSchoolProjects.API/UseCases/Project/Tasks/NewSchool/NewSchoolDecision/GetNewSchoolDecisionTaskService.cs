using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Data;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolDecision
{
    public class GetNewSchoolDecisionTaskService : IGetTaskService
    {
        private readonly MfspContext _context;

        public GetNewSchoolDecisionTaskService(MfspContext context)
        {
            _context = context;
        }

        public async Task<GetProjectByTaskResponse> Get(GetTaskServiceParameters parameters)
        {
            var query = parameters.BaseQuery;

            var result = await query.Select(kpi => new GetProjectByTaskResponse()
            {
                NewSchoolDecision = new NewSchoolDecisionTask
                {
                    NewSchoolDecision = kpi.NewSchoolDecision
                }
            }).FirstOrDefaultAsync();

            return result;
        }
    }
}
