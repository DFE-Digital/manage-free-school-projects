using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ReferenceNumbers;
using Dfe.ManageFreeSchoolProjects.API.Exceptions;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.ReferenceNumbers
{
    public interface IUpdateProjectReferenceNumbersService
    {
        public Task Execute(string projectId, UpdateProjectReferenceNumbersRequest request);
    }

    public class UpdateProjectReferenceNumbersService : IUpdateProjectReferenceNumbersService
    {
        private readonly MfspContext _context;

        public UpdateProjectReferenceNumbersService(MfspContext context)
        {
            _context = context;
        }

        public async Task Execute(string projectId, UpdateProjectReferenceNumbersRequest request)
        {
            var project = await _context.Kpi.FirstOrDefaultAsync(x => x.ProjectStatusProjectId == projectId);

            if (project == null)
            {
                throw new NotFoundException($"Project with id {projectId} not found");
            }

            project.ProjectStatusProjectId = request.ProjectId;
            project.ProjectStatusUrnWhenGivenOne = request.Urn;

            await _context.SaveChangesAsync();
        }

    }
}
