using Dfe.ManageFreeSchoolProjects.Data;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool.NewSchoolSpecificationPublicationDate
{
    public class UpdateNewSchoolSpecificationPublicationDateTaskService : IUpdateTaskService
    {
        private readonly MfspContext _context;

        public UpdateNewSchoolSpecificationPublicationDateTaskService(MfspContext context)
        {
            _context = context;
        }

        public async Task Update(UpdateTaskServiceParameters parameters)
        {
            var task = parameters.Request.NewSchoolSpecificationPublicationDate;
            var dbKpi = parameters.Kpi;

            if (task is null)
            {
                return;
            }

            dbKpi.NewSchoolSpecificationPublicationDate = task.NewSchoolSpecificationPublicationDate;
        }
    }
}
