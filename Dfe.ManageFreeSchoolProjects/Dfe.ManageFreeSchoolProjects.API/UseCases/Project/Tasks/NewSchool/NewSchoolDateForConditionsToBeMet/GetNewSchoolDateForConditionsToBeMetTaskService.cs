using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolDateForConditionsToBeMet
{
    public class GetNewSchoolDateForConditionsToBeMetTaskService : IGetTaskService
    {
        public async Task<GetProjectByTaskResponse> Get(GetTaskServiceParameters parameters)
        {
            var query = parameters.BaseQuery;

            var result = await query.Select(kpi => new GetProjectByTaskResponse()
            {
                NewSchoolDateForConditionsToBeMet = new NewSchoolDateForConditionsToBeMetTask
                {
                    NewSchoolDateForConditionsToBeMet = kpi.NewSchoolDateForConditionsToBeMet
                }
            }).FirstOrDefaultAsync();

            return result;
        }
    }
}
