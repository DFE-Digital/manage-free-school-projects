using Dfe.ManageFreeSchoolProjects.Data;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolConditions
{
    public class UpdateGetNewSchoolConditionsTaskService : IUpdateTaskService
    {
        private readonly MfspContext _context;

        public UpdateGetNewSchoolConditionsTaskService(MfspContext context)
        {
            _context = context;
        }

        public async Task Update(UpdateTaskServiceParameters parameters)
        {
            var task = parameters.Request.NewSchoolConditions;
            var dbKpi = parameters.Kpi;

            if (task is null)
            {
                return;
            }

            dbKpi.NewSchoolConditions = task.NewSchoolConditions;
            dbKpi.NewSchoolConditionsDescription = task.NewSchoolConditionsDescription;
        }
    }
}