using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ReferenceNumbers;
using Dfe.ManageFreeSchoolProjects.API.Exceptions;
using Dfe.ManageFreeSchoolProjects.Data;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.ReferenceNumbers
{
    public interface IGetProjectReferenceNumbersService
    {
        public Task<GetProjectReferenceNumbersResponse> Execute(string projectId);
    }

    public class GetProjectReferenceNumbersService : IGetProjectReferenceNumbersService
    {
        private readonly MfspContext _context;

        public GetProjectReferenceNumbersService(MfspContext context)
        {
            _context = context;
        }

        public async Task<GetProjectReferenceNumbersResponse> Execute(string projectId)
        {
            var project = await _context.Kpi.FirstOrDefaultAsync(x => x.ProjectStatusProjectId == projectId);

            if (project == null)
            {
                throw new NotFoundException($"Project with id {projectId} not found");
            }

            var result = new GetProjectReferenceNumbersResponse()
            {
                ProjectId = projectId,
                Urn = project.ProjectStatusUrnWhenGivenOne,
                SchoolName = project.ProjectStatusCurrentFreeSchoolName,
            };

            return result;
        }
    }
}
