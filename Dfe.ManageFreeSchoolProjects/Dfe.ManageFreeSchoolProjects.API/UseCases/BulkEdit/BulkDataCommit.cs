using Dfe.ManageFreeSchoolProjects.API.UseCases.Project;
using Dfe.ManageFreeSchoolProjects.Data;
using Microsoft.EntityFrameworkCore;
using Dfe.ManageFreeSchoolProjects.API.Extensions;
using System.Globalization;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.BulkEdit
{
    public class BulkDataCommit : IBulkDataCommit<BulkEditDto>
    {
        private readonly MfspContext _context;

        public BulkDataCommit(MfspContext context)
        {
            _context = context;
        }

        public async Task Save(IEnumerable<BulkEditDto> data)
        {
            foreach (var dto in data)
            {
                var entity = await _context.Kpi.FirstOrDefaultAsync(x => x.ProjectStatusProjectId == dto.ProjectId);

                if(!dto.SchoolName.IsNullOrEmpty())
                {
                    entity.ProjectStatusCurrentFreeSchoolName = dto.SchoolName;
                }

                if(!dto.OpeningDate.IsNullOrEmpty())
                {
                    entity.ProjectStatusActualOpeningDate = DateTime.Parse(dto.OpeningDate, new CultureInfo("en-GB"));
                }

                if (!dto.ProjectStatus.IsNullOrEmpty())
                {
                    entity.ProjectStatusProjectStatus = ProjectMapper.FromProjectStatusType(ProjectMapper.ToProjectStatusType(dto.ProjectStatus));
                }

                if (!dto.LocalAuthorityCode.IsNullOrEmpty())
                {
                    entity.SchoolDetailsLocalAuthority = dto.LocalAuthorityCode;
                    entity.LocalAuthority = dto.LocalAuthorityName;
                    entity.SchoolDetailsGeographicalRegion = dto.Region;
                }

            }

            await _context.SaveChangesAsync();
        }
    }
}
