using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolDateOfDecision
{
    public class GetNewSchoolDateOfDecisionTaskService : IGetTaskService
    {
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
