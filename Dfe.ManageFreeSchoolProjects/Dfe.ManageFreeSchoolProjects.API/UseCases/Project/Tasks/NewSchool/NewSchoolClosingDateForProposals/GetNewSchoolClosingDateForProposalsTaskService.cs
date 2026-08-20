using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolClosingDateForProposals
{
    public class GetNewSchoolClosingDateForProposalsTaskService() : IGetTaskService
    {
        public async Task<GetProjectByTaskResponse> Get(GetTaskServiceParameters parameters)
        {
            var query = parameters.BaseQuery;

            var result = await query.Select(kpi => new GetProjectByTaskResponse()
            {
                NewSchoolClosingDateForProposals = new NewSchoolClosingDateForProposalsTask
                {
                    NewSchoolClosingDateForProposals = kpi.NewSchoolClosingDateForProposals
                }
            }).FirstOrDefaultAsync();

            return result;
        }
    }
}
