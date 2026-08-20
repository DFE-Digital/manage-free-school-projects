namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolDateOfDecision
{
    public class UpdateNewSchoolDateOfDecisionTaskService : IUpdateTaskService
    {
        public async Task Update(UpdateTaskServiceParameters parameters)
        {
            var task = parameters.Request.NewSchoolDateOfDecision;
            var dbKpi = parameters.Kpi;

            if (task is null)
            {
                return;
            }

            dbKpi.NewSchoolDateOfDecision = task.NewSchoolDateOfDecision;
        }
    }
}
