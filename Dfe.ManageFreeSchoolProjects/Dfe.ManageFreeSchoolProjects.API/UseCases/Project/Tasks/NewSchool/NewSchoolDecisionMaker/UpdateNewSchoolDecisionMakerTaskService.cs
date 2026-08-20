using Dfe.ManageFreeSchoolProjects.Data;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolDecisionMaker
{
    public class UpdateNewSchoolDecisionMakerTaskService : IUpdateTaskService
    {
        private readonly MfspContext _context;

        public UpdateNewSchoolDecisionMakerTaskService(MfspContext context)
        {
            _context = context;
        }

        public async Task Update(UpdateTaskServiceParameters parameters)
        {
            var task = parameters.Request.NewSchoolDecisionMaker;
            var dbKpi = parameters.Kpi;

            if (task is null)
            {
                return;
            }

            dbKpi.NewSchoolDecisionMaker = task.NewSchoolDecisionMaker;
        }
    }
}
