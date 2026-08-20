using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Data;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolDateOfDecision
{
    public class GetNewSchoolDateOfDecisionTaskService : IGetTaskService
    {
        private readonly MfspContext _context;

        public GetNewSchoolDateOfDecisionTaskService(MfspContext context)
        {
            _context = context;
        }

        public async Task<GetProjectByTaskResponse> Get(GetTaskServiceParameters parameters)
        {
            var query = parameters.BaseQuery;

            var result = await query.Select(kpi => new GetProjectByTaskResponse()
            {
                NewSchoolDateOfDecision = new NewSchoolDateOfDecisionTask
                {
                    NewSchoolDateOfDecision = kpi.NewSchoolDateOfDecision
                }
            }).FirstOrDefaultAsync();

            return result;
        }
    }
}
