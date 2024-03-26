using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Projects;
using Dfe.ManageFreeSchoolProjects.API.Exceptions;
using Dfe.ManageFreeSchoolProjects.API.Extensions;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Tasks;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project
{
    public interface ICreateProjectService
    {
        Task<CreateProjectResponse> Execute(CreateProjectRequest createProjectRequest);
    }

    public class CreateProject : ICreateProjectService
    {
        private readonly MfspContext _context;

        public CreateProject(MfspContext context)
        {
            _context = context;
        }

        public async Task<CreateProjectResponse> Execute(CreateProjectRequest createProjectRequest)
        {
            var result = new CreateProjectResponse();

            await ThrowIfProjectExists(createProjectRequest);

            foreach (ProjectDetails proj in createProjectRequest.Projects)
            {
                result.Projects.Add(new ProjectResponseDetails
                {
                    ProjectId = proj.ProjectId,
                    ProjectCreateState = ProjectCreateState.New
                });

                var rid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 11);
                var trust = await GetTrust(proj.TRN);

                Kpi kpi = MapToKpi(proj, rid, trust);

                var nurseryCapacity = proj.Nursery == ClassType.Nursery.Yes ? proj.NurseryCapacity : 0;
                var po = MapToPo(proj, rid, nurseryCapacity);

                _context.Kpi.Add(kpi);
                _context.Tasks.AddRange(ProjectTaskBuilder.BuildTasks(rid));
                _context.RiskAppraisalMeetingTask.Add(new Data.Entities.RiskAppraisalMeetingTask() { RID = rid });
                _context.Po.Add(po);
            }

            await _context.SaveChangesAsync();

            return result;
        }

        private static Po MapToPo(ProjectDetails proj, string rid, int nurseryCapacity)
        {
            return new Po()
            {
                Rid = rid,
                PupilNumbersAndCapacityNurseryUnder5s = nurseryCapacity.ToString(),
                PupilNumbersAndCapacityYrY6Capacity = proj.YRY6Capacity.ToString(),
                PupilNumbersAndCapacityY7Y11Capacity = proj.Y7Y11Capacity.ToString(),
                PupilNumbersAndCapacityYrY11Pre16Capacity = (proj.YRY6Capacity + proj.Y7Y11Capacity).ToString(),
                PupilNumbersAndCapacityY12Y14Post16Capacity = proj.Y12Y14Capacity.ToString(),
                PupilNumbersAndCapacityTotalOfCapacityTotals = (nurseryCapacity + proj.YRY6Capacity + proj.Y7Y11Capacity + proj.Y12Y14Capacity).ToString()
            };
        }

        private static Kpi MapToKpi(ProjectDetails proj, string rid, Trust trust)
        {
            return new Kpi
            {
                Rid = rid,
                ProjectStatusProjectId = proj.ProjectId,
                ProjectStatusCurrentFreeSchoolName = proj.SchoolName,
                ProjectStatusFreeSchoolApplicationWave = "",
                ProjectStatusFreeSchoolsApplicationNumber = "",
                AprilIndicator = "",
                Wave = "",
                UpperStatus = "",
                FsType = "",
                FsType1 = "",
                MatUnitProjects = "",
                SponsorUnitProjects = "",
                SchoolDetailsGeographicalRegion = proj.Region,
                SchoolDetailsLocalAuthority = proj.LocalAuthorityCode,
                LocalAuthority = proj.LocalAuthority,
                SchoolDetailsSchoolTypeMainstreamApEtc = ProjectMapper.ToSchoolType(proj.SchoolType),
                SchoolDetailsSchoolPhasePrimarySecondary = ProjectMapper.ToSchoolPhase(proj.SchoolPhase),
                TrustId = trust.TrustRef,
                TrustName = trust.TrustsTrustName,
                TrustType = trust.TrustsTrustType,
                SchoolDetailsTrustId = trust.TrustsTrustRef,
                SchoolDetailsTrustName = trust.TrustsTrustName,
                SchoolDetailsTrustType = trust.TrustsTrustType,
                SchoolDetailsSixthForm = proj.SixthForm.ToString(),
                SchoolDetailsNursery = proj.Nursery.ToString(),
                SchoolDetailsAgeRange = proj.AgeRange,
                SchoolDetailsNumberOfFormsOfEntry = proj.FormsOfEntry,
                SchoolDetailsFaithStatus = proj.FaithStatus.ToString(),
                SchoolDetailsFaithType = proj.FaithType.ToDescription(),
                SchoolDetailsPleaseSpecifyOtherFaithType = proj.OtherFaithType,
                ProjectStatusProvisionalOpeningDateAgreedWithTrust = proj.ProvisionalOpeningDate,
                KeyContactsFsgLeadContact = proj.ProjectLead
            };
        }

        private async Task ThrowIfProjectExists(CreateProjectRequest createProjectRequest)
        {
            var projectIds = createProjectRequest.Projects.Select(p => p.ProjectId);

            var existingProjectIds = await _context.Kpi
                .Where(k => projectIds.Contains(k.ProjectStatusProjectId))
                .Select(p => p.ProjectStatusProjectId)
                .ToListAsync();

            if (existingProjectIds.Any())
            {
                throw new UnprocessableContentException($"The following project(s) already exist: {string.Join(",", existingProjectIds)}");
            }
        }

        private async Task<Trust> GetTrust(string trustRef)
        {
            var result = await _context.Trust.FirstOrDefaultAsync(e => e.TrustRef == trustRef);

            if (result == null)
            {
                throw new UnprocessableContentException($"The trust does not exist: {trustRef}");
            }

            return result;
        }
    }
}