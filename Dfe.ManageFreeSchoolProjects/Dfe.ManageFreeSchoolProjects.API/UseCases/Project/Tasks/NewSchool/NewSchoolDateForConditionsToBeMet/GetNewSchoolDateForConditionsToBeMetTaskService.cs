using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Data;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolDateForConditionsToBeMet
{
    public class GetNewSchoolDateForConditionsToBeMetTaskService : IGetTaskService
    {
        private readonly MfspContext _context;

        public GetNewSchoolDateForConditionsToBeMetTaskService(MfspContext context)
        {
            _context = context;
        }

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
