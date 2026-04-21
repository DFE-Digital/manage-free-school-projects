using Dfe.ManageFreeSchoolProjects.API.Extensions;
using Dfe.ManageFreeSchoolProjects.API.Constants;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.Dates
{
    public class UpdateDatesTaskService : IUpdateTaskService
    {
        public async Task Update(UpdateTaskServiceParameters parameters)
        {
            var dbKpi = parameters.Kpi;
            var task = parameters.Request.Dates;


            if (task == null)
            {
                return;
            }

            dbKpi.ProjectStatusDateOfEntryIntoPreOpening = task.DateOfEntryIntoPreopening;
            dbKpi.ProjectStatusProvisionalOpeningDateAgreedWithTrust = task.ProvisionalOpeningDateAgreedWithTrust;
            dbKpi.ProjectStatusDateClosed = task.ProjectClosedDate;
            dbKpi.ProjectStatusDateCancelled = task.ProjectCancelledDate;
            dbKpi.ProjectStatusDateWithdrawn = task.ProjectWithdrawnDate;
            dbKpi.ProjectStatusRealisticYearOfOpening = task.RealisticYearOfOpening;
            dbKpi.RyooWd = task.RealisticYearOfOpening.IsNullOrEmpty() ? ProjectConstants.RYOODefaultValue : task.RealisticYearOfOpening;
        }
    }
}
