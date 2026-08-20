using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Data;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolDecisionMaker
{
    public class GetNewSchoolDecisionMakerTaskService : IGetTaskService
    {
        private readonly MfspContext _context;

        public GetNewSchoolDecisionMakerTaskService(MfspContext context)
        {
            _context = context;
        }

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
