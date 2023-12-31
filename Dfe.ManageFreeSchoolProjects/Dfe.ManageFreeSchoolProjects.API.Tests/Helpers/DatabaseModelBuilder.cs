﻿using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using Dfe.ManageFreeSchoolProjects.Data.Entities;
using System;
using System.Diagnostics.Contracts;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.Helpers
{
    public class DatabaseModelBuilder
    {
        private static Fixture _fixture = new();

        public static Kpi BuildProject()
        {
            var result = BuildProjectMandatoryFieldsOnly();

            result.ProjectStatusProjectId = CreateProjectId();
            result.ProjectStatusCurrentFreeSchoolName = _fixture.Create<string>();
            result.TrustName = _fixture.Create<string>();

            result.ProjectStatusCurrentFreeSchoolName = _fixture.Create<string>();
            result.ProjectStatusProjectStatus = _fixture.Create<string>();
            result.ProjectStatusFreeSchoolsApplicationNumber = _fixture.Create<string>().Substring(0, 9);
            result.ProjectStatusUrnWhenGivenOne = _fixture.Create<string>();
            result.ProjectStatusFreeSchoolApplicationWave = _fixture.Create<string>();
            result.ProjectStatusRealisticYearOfOpening = _fixture.Create<string>();
            result.ProjectStatusDateOfEntryIntoPreOpening = _fixture.Create<DateTime>();
            result.ProjectStatusProvisionalOpeningDateAgreedWithTrust = _fixture.Create<DateTime>();
            result.ProjectStatusActualOpeningDate = _fixture.Create<DateTime>();
            result.ProjectStatusTrustsPreferredYearOfOpening = _fixture.Create<string>();

            result.LocalAuthority = _fixture.Create<string>();
            result.SchoolDetailsGeographicalRegion = _fixture.Create<string>();
            result.SchoolDetailsConstituency = _fixture.Create<string>();
            result.SchoolDetailsConstituencyMp = _fixture.Create<string>();
            result.SchoolDetailsNumberOfFormsOfEntry = _fixture.Create<string>();
            result.SchoolDetailsSchoolTypeMainstreamApEtc = _fixture.Create<string>();
            result.SchoolDetailsSchoolPhasePrimarySecondary = _fixture.Create<string>();
            result.SchoolDetailsAgeRange = _fixture.Create<string>();
            result.SchoolDetailsGender = _fixture.Create<string>();
            result.SchoolDetailsNursery = _fixture.Create<string>();
            result.SchoolDetailsSixthForm = _fixture.Create<string>();
            result.SchoolDetailsIndependentConverter = _fixture.Create<string>();
            result.SchoolDetailsSpecialistResourceProvision = _fixture.Create<string>();
            result.SchoolDetailsFaithStatus = _fixture.Create<string>();
            result.SchoolDetailsFaithType = _fixture.Create<string>();
            result.SchoolDetailsTrustId = _fixture.Create<string>().Substring(0, 4);
            result.SchoolDetailsTrustName = _fixture.Create<string>();

            result.KeyContactsFsgTeamLeader = _fixture.Create<string>();
            result.KeyContactsFsgGrade6 = _fixture.Create<string>();
            result.KeyContactsEsfaCapitalProjectManager = _fixture.Create<string>();
            result.KeyContactsEsfaCapitalProjectDirector = _fixture.Create<string>();
            result.SchoolDetailsTrustType = _fixture.Create<string>();
            result.KeyContactsFsgLeadContact = _fixture.Create<string>();

            return result;
        }

        public static Kpi BuildProjectMandatoryFieldsOnly()
        {
            var result = new Kpi
            {
                Rid = _fixture.Create<string>().Substring(0, 10),
                AprilIndicator = _fixture.Create<string>().Substring(0, 9),
                Wave = _fixture.Create<string>().Substring(0, 15),
                UpperStatus = _fixture.Create<string>().Substring(0, 10),
                FsType = _fixture.Create<string>().Substring(0, 13),
                FsType1 = _fixture.Create<string>().Substring(0, 15),
                MatUnitProjects = _fixture.Create<string>().Substring(0, 31),
                SponsorUnitProjects = _fixture.Create<string>()
            };

            return result;
        }

        public static Trust BuildTrust()
        {
            var result = new Trust
            {
                Rid = _fixture.Create<string>().Substring(0, 10),
                TrustRef = _fixture.Create<string>().Substring(0, 7),
                TrustsTrustRef = _fixture.Create<string>().Substring(0, 5),
                TrustsTrustName = _fixture.Create<string>(),
                TrustsTrustType = _fixture.Create<string>()
            };

            return result;
        }

        public static Property BuildProperty()
        {
            var result = new Property
            {
                Rid = _fixture.Create<string>().Substring(0, 10),
                SiteNameOfSite = _fixture.Create<string>(),
                SitePostcodeOfSite = _fixture.Create<string>(),
                Tos = _fixture.Create<string>().Substring(0, 8),
            };

            return result;
        }

        public static string CreateProjectId()
        {
            return _fixture.Create<string>().Substring(0, 24);
        }

        public static RiskAppraisalMeetingTask BuildRiskAppraisalMeetingTask(string rid)
        {
            var result = new RiskAppraisalMeetingTask();

            result.RID = rid;

            return result;
        }

    }
}
