﻿using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.School
{
    public static class SchoolTaskMapper
    {
        public static SchoolTask Map(Kpi kpi)
        {
            return new SchoolTask()
            {
                CurrentFreeSchoolName = kpi.ProjectStatusCurrentFreeSchoolName,
                SchoolType = ProjectMapper.ToSchoolType(kpi.SchoolDetailsSchoolTypeMainstreamApEtc),
                SchoolPhase = ProjectMapper.ToSchoolPhase(kpi.SchoolDetailsSchoolPhasePrimarySecondary),
                AgeRange = kpi.SchoolDetailsAgeRange,
                Gender = EnumParsers.ParseGender(kpi.SchoolDetailsGender),
                Nursery = EnumParsers.ParseNursery(kpi.SchoolDetailsNursery),
                SixthForm = EnumParsers.ParseSixthForm(kpi.SchoolDetailsSixthForm),
                FaithStatus = EnumParsers.ParseFaithStatus(kpi.SchoolDetailsFaithStatus),
                FaithType = ProjectMapper.ToFaithType(kpi.SchoolDetailsFaithType),
                OtherFaithType = kpi.SchoolDetailsPleaseSpecifyOtherFaithType
            };
        }
    }
}
