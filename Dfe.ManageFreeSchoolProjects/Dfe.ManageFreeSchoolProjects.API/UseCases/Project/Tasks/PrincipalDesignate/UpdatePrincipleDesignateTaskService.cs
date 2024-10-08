using Dfe.ManageFreeSchoolProjects.Data;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.PrincipalDesignate
{
    public class UpdatePrincipleDesignateTaskService : IUpdateTaskService
    {
        private readonly MfspContext _context;

        public UpdatePrincipleDesignateTaskService(MfspContext context)
        {
            _context = context;
        }

        public async Task Update(UpdateTaskServiceParameters parameters)
        {
            var task = parameters.Request.PrincipalDesignate;
            var dbKpi = parameters.Kpi;
            
            if (task is null)
            {
                return;
            }

            var db = await _context.Milestones.FirstOrDefaultAsync(r => r.Rid == dbKpi.Rid);

            if (db == null)
            {
                db = new Data.Entities.Existing.Milestones();
                db.Rid = dbKpi.Rid;
                _context.Add(db);
            }

            db.FsgPreOpeningMilestonesAppointedPrincipalDesignate = task.TrustAppointedPrincipalDesignate;
            db.FsgPreOpeningMilestonesPdappActualDateOfCompletion = task.ActualDatePrincipalDesignateAppointed;
            db.FsgPreOpeningMilestonesCommissionedExternalExpertVisitToSchool = task.CommissionedExternalExpertVisitToSchool;
            db.FsgPreOpeningMilestonesPdappForecastDate = task.ExpectedDatePrincipalDesignateAppointed;
        }
    }
}