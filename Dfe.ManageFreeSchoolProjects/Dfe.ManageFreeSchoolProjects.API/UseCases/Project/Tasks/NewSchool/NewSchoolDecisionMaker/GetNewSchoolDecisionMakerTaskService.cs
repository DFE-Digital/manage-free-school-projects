using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolDecisionMaker
{
    public class GetNewSchoolDecisionMakerTaskService : IGetTaskService
    {
        public async Task<GetProjectByTaskResponse> Get(GetTaskServiceParameters parameters)
        {
            var query = parameters.BaseQuery;

            var result = await query.Select(kpi => new GetProjectByTaskResponse()
            {
                NewSchoolDecisionMaker = new NewSchoolDecisionMakerTask
                {
                    NewSchoolDecisionMaker = kpi.NewSchoolDecisionMaker
                }
            }).FirstOrDefaultAsync();

            return result;
        }
    }
}
