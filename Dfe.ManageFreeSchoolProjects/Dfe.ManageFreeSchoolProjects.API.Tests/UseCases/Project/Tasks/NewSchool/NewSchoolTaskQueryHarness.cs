using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.UseCases.Project.Tasks.NewSchool
{
    /// <summary>
    /// The new school get task services only ever project from the query they are handed, so they
    /// can be exercised against an in-memory KPI row rather than the SQL Server fixture the task
    /// API tests use.
    /// </summary>
    internal sealed class NewSchoolTaskQueryHarness : IDisposable
    {
        public const string ProjectId = "NEW-SCHOOL-1";

        private readonly MfspContext _context;

        private NewSchoolTaskQueryHarness(MfspContext context)
        {
            _context = context;
        }

        public GetTaskServiceParameters Parameters => new()
        {
            ProjectId = ProjectId,
            BaseQuery = _context.Kpi.Where(kpi => kpi.ProjectStatusProjectId == ProjectId)
        };

        /// <summary>Builds a harness whose single KPI row has been shaped by <paramref name="configure"/>.</summary>
        public static NewSchoolTaskQueryHarness WithKpi(Action<Kpi> configure)
        {
            var kpi = BuildKpi();
            configure(kpi);

            return Seed(kpi);
        }

        /// <summary>Builds a harness with no KPI row matching the project id.</summary>
        public static NewSchoolTaskQueryHarness WithNoMatchingKpi()
        {
            var kpi = BuildKpi();
            kpi.ProjectStatusProjectId = "SOME-OTHER-PROJECT";

            return Seed(kpi);
        }

        private static NewSchoolTaskQueryHarness Seed(Kpi kpi)
        {
            // A fresh database name per harness keeps the seeded rows isolated between tests.
            var options = new DbContextOptionsBuilder<MfspContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new MfspContext(options, null);
            context.Kpi.Add(kpi);
            context.SaveChanges();

            return new NewSchoolTaskQueryHarness(context);
        }

        private static Kpi BuildKpi()
        {
            // Only the key, the project id the query filters on, and the columns the KPI
            // configuration marks as required need values for the row to persist.
            return new Kpi
            {
                Rid = "RID-1",
                ProjectStatusProjectId = ProjectId,
                AprilIndicator = "N",
                FsType = "Free School",
                FsType1 = "Free School",
                MatUnitProjects = "0",
                SponsorUnitProjects = "0",
                UpperStatus = "Open",
                Wave = "Wave 15"
            };
        }

        public void Dispose() => _context.Dispose();
    }
}
