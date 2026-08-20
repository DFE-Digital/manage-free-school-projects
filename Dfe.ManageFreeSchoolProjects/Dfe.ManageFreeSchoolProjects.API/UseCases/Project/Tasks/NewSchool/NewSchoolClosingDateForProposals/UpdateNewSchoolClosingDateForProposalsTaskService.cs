using Dfe.ManageFreeSchoolProjects.Data;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolClosingDateForProposals
{
    public class UpdateNewSchoolClosingDateForProposalsTaskService : IUpdateTaskService
    {
        private readonly MfspContext _context;

        public UpdateNewSchoolClosingDateForProposalsTaskService(MfspContext context)
        {
            _context = context;
        }

        public async Task Update(UpdateTaskServiceParameters parameters)
        {
            var task = parameters.Request.NewSchoolClosingDateForProposals;
            var dbKpi = parameters.Kpi;

            if (task is null)
            {
                return;
            }

            dbKpi.NewSchoolClosingDateForProposals = task.NewSchoolClosingDateForProposals;
        }
    }
}
