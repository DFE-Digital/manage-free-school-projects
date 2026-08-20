using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolDecision
{
    public class GetNewSchoolDecisionTaskService : IGetTaskService
    {
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
