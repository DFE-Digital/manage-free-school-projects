namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolDateForConditionsToBeMet
{
    public class UpdateNewSchoolDateForConditionsToBeMetTaskService : IUpdateTaskService
    {
        public async Task Update(UpdateTaskServiceParameters parameters)
        {
            var task = parameters.Request.NewSchoolDateForConditionsToBeMet;
            var dbKpi = parameters.Kpi;

            if (task is null)
            {
                return;
            }

            dbKpi.NewSchoolDateForConditionsToBeMet = task.NewSchoolDateForConditionsToBeMet;
        }
    }
}
