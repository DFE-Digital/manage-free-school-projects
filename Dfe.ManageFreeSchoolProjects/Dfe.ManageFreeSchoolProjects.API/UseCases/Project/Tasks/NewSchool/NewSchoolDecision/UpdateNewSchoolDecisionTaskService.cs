using Dfe.ManageFreeSchoolProjects.Data;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolDecision
{
    public class UpdateNewSchoolDecisionTaskService : IUpdateTaskService
    {
        private readonly MfspContext _context;

        public UpdateNewSchoolDecisionTaskService(MfspContext context)
        {
            _context = context;
        }

        public async Task Update(UpdateTaskServiceParameters parameters)
        {
            var task = parameters.Request.NewSchoolDecision;
            var dbKpi = parameters.Kpi;

            if (task is null)
            {
                return;
            }

            dbKpi.NewSchoolDecision = task.NewSchoolDecision;
        }
    }
}
