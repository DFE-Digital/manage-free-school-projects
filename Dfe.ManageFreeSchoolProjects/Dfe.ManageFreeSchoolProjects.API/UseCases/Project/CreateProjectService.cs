using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.PupilNumbers;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Projects;
using Dfe.ManageFreeSchoolProjects.API.Exceptions;
using Dfe.ManageFreeSchoolProjects.API.Extensions;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.PupilNumbers;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Tasks;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using Dfe.ManageFreeSchoolProjects.API.Constants;
using Microsoft.EntityFrameworkCore;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.School;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project
{
    public interface ICreateProjectService
    {
        Task<CreateProjectResponse> Execute(CreateProjectRequest createProjectRequest);
    }

    public class CreateProject : ICreateProjectService
    {
        private readonly MfspContext _context;
        private readonly IUpdateCapacityWhenFullService _updateCapacityWhenFullService;

        public CreateProject(
            MfspContext context,
            IUpdateCapacityWhenFullService updateCapacityWhenFullService)
        {
            _context = context;
            _updateCapacityWhenFullService = updateCapacityWhenFullService;
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

                var kpi = MapToKpi(proj, rid, trust);

                var po = MapToPo(proj, rid);

                _context.Kpi.Add(kpi);
                _context.Tasks.AddRange(ProjectTaskBuilder.BuildTasks(rid));
                _context.RiskAppraisalMeetingTask.Add(new Data.Entities.RiskAppraisalMeetingTask { RID = rid });
                _context.Po.Add(po);
            }

            await _context.SaveChangesAsync();

            return result;
        }

        private Po MapToPo(ProjectDetails proj, string rid)
        {
            var nurseryCapacity = proj.Nursery == ClassType.Nursery.Yes ? proj.NurseryCapacity : 0;

            var result = new Po
            {
                Rid = rid,
            };

            _updateCapacityWhenFullService.Execute(result, new CapacityWhenFull
            {
                Nursery = nurseryCapacity,
                ReceptionToYear6 = proj.YRY6Capacity,
                Year7ToYear11 = proj.Y7Y11Capacity,
                Year12ToYear14 = proj.Y12Y14Capacity,
                APResourcesProvision = proj.APResourcesProvision,
                SENResourcedProvisionSENUnit = proj.SENResourcedProvisionSENUnit
            });

            return result;
        }

        private static Kpi MapToKpi(ProjectDetails proj, string rid, Trust trust)
        {

            return new Kpi
            {
                Rid = rid,
                ProjectStatusProjectStatus = proj.ProjectType == ProjectType.LocalAuthority ? ProjectMapper.FromProjectStatusType(Contracts.Project.ProjectStatus.Prepipeline) : ProjectMapper.FromProjectStatusType(Contracts.Project.ProjectStatus.Preopening),
                ProjectStatusProjectId = proj.ProjectId,
                ProjectStatusCurrentFreeSchoolName = proj.SchoolName,
                ProjectStatusFreeSchoolApplicationWave = proj.ApplicationWave ?? string.Empty,
                ProjectStatusFreeSchoolsApplicationNumber = proj.ApplicationNumber ?? string.Empty,
                AprilIndicator = "",
                Wave = proj.ApplicationWave == "FS - Presumption" ? "FS - Presumption" : "Non-Presumption",
                UpperStatus = "",
                FsType = "",
                FsType1 = "",
                MatUnitProjects = ProjectConstants.MATUnitProjects,
                SponsorUnitProjects = ProjectConstants.SponsorUnitProjects,
                SchoolDetailsGeographicalRegion = proj.Region,
                SchoolDetailsLocalAuthority = proj.LocalAuthorityCode,
                LocalAuthority = proj.LocalAuthority,
                SchoolDetailsSchoolTypeMainstreamApEtc = ProjectMapper.ToSchoolType(proj.SchoolType),
                SchoolDetailsSchoolPhasePrimarySecondary = ProjectMapper.ToSchoolPhase(proj.SchoolPhase),
                TrustId = trust?.TrustRef,
                TrustName = trust?.TrustsTrustName ?? null,
                TrustType = trust?.TrustsTrustType ?? null,
                SchoolDetailsTrustId = trust?.TrustsTrustRef ?? null,
                SchoolDetailsTrustName = trust?.TrustsTrustName ?? null,
                SchoolDetailsTrustType = trust?.TrustsTrustType ?? null,
                SchoolDetailsSixthForm = proj.SixthForm.ToString(),
                SchoolDetailsNursery = proj.Nursery.ToString(),
                SchoolDetailsAlternativeProvision = proj.AlternativeProvision.ToString(),
                SchoolDetailsSpecialEducationNeeds = proj.SpecialEducationNeeds.ToString(),
                SchoolDetailsSpecialistResourceProvision = BuildSpecialistResourceProvision
                                                            .GetLegacySpecialistResourceProvision(proj.SpecialEducationNeeds,
                                                                                                  proj.AlternativeProvision),
                SchoolDetailsResidentialOrBoardingProvision = proj.ResidentialOrBoarding.ToString(),
                SchoolDetailsAgeRange = proj.AgeRange,
                SchoolDetailsNumberOfFormsOfEntry = proj.FormsOfEntry,
                SchoolDetailsFaithStatus = proj.FaithStatus.ToString(),
                SchoolDetailsFaithType = proj.FaithType.ToDescription(),
                SchoolDetailsPleaseSpecifyOtherFaithType = proj.OtherFaithType,
                ProjectStatusProvisionalOpeningDateAgreedWithTrust = proj.ProvisionalOpeningDate,
                KeyContactsFsgLeadContact = proj.ProjectAssignedToName,
                KeyContactsFsgLeadContactEmail = proj.ProjectAssignedToEmail,
                RyooWd = ProjectConstants.RYOODefaultValue

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
                throw new UnprocessableContentException(
                    $"The following project(s) already exist: {string.Join(",", existingProjectIds)}");
            }
        }

        private async Task<Trust> GetTrust(string trustRef)
        {
            var result = await _context.Trust.FirstOrDefaultAsync(e => e.TrustRef == trustRef);

            // Local authority projects are not attached to a trust and supply no TRN, so a missing
            // trust is only an error when a reference was actually given.
            if (!string.IsNullOrEmpty(trustRef) && result == null)
            {
                throw new UnprocessableContentException($"The trust does not exist: {trustRef}");
            }

            return result;
        }


    }
}