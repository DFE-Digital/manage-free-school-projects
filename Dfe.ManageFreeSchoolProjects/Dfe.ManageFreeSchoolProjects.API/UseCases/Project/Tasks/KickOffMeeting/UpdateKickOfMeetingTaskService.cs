using Dfe.ManageFreeSchoolProjects.Data;
using Microsoft.EntityFrameworkCore;
using Dfe.ManageFreeSchoolProjects.API.Extensions;
using Dfe.ManageFreeSchoolProjects.API.Constants;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.KickOffMeeting
{
    public class UpdateKickOffMeetingTaskService : IUpdateTaskService
    {
        private readonly MfspContext _context;

        public UpdateKickOffMeetingTaskService(MfspContext context)
        {
            _context = context;
        }

        public async Task Update(UpdateTaskServiceParameters parameters)
        {
            var task = parameters.Request.KickOffMeeting;
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

            dbKpi.ProjectStatusRealisticYearOfOpening = task.RealisticYearOfOpening;
            dbKpi.RyooWd = task.RealisticYearOfOpening.IsNullOrEmpty() ? ProjectConstants.RYOODefaultValue : task.RealisticYearOfOpening;
            db.FsgPreOpeningMilestonesFundingArrangementAgreedBetweenLaAndSponsor = task.FundingArrangementAgreed;
            db.FsgPreOpeningMilestonesDetailsOfFundingArrangementAgreedBetweenLaAndSponsor =
                task.FundingArrangementDetailsAgreed;
            db.FsgPreOpeningMilestonesKickoffMeetingDocumentsSavedInWorkplacesFolder = task.SavedDocumentsInWorkplacesFolder;
        }
    }
}