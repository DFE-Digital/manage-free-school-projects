﻿using Dfe.ManageFreeSchoolProjects.API.Contracts.Common;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Risk;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;

namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project
{
    public record ProjectOverviewResponse
    {
        public ProjectStatusResponse ProjectStatus { get; set; }
        public SchoolDetailsResponse SchoolDetails { get; set; }
        public KeyContactsResponse KeyContacts { get; set; }
        public ProjectRiskOverviewResponse Risk { get; set; }
        public PupilNumbersOverviewResponse PupilNumbers { get; set; }

        public string ProjectType { get; set; }
    }

    public record ProjectStatusResponse
    {
        public string CurrentFreeSchoolName { get; set; }
        public ProjectStatus ProjectStatus { get; set; }

        public ProjectCancelledReason ProjectCancelledReason { get; set; }

        public ProjectWithdrawnReason ProjectWithdrawnReason { get; set; }

        public DateTime? ProjectClosedDate { get; set; }
        
        public DateTime? ProjectCancelledDate { get; set; }
        
        public DateTime? ProjectWithdrawnDate { get; set; }

        public YesNo? ProjectCancelledDueToNationalReviewOfPipelineProjects { get; set; }
        public YesNo? ProjectWithdrawnDueToNationalReviewOfPipelineProjects { get; set; }
        public string CommentaryForCancellation {  get; set; }
        public string CommentaryForWithdrawal { get; set; }

        public string FreeSchoolsApplicationNumber { get; set; }
        public string ProjectId { get; set; }
        public string Urn { get; set; }
        public string ApplicationWave { get; set; }
        public string RealisticYearOfOpening { get; set; }
        public DateTime? DateOfEntryIntoPreopening { get; set; }
        public DateTime? ProvisionalOpeningDateAgreedWithTrust { get; set; }
        public DateTime? ActualOpeningDate { get; set; }
        public string OpeningAcademicYear { get; set; }
        public DateTime? DateSchoolClosed { get; set; }
    }

    public record SchoolDetailsResponse
    {
        public string LocalAuthority { get; set; }
        public string Region { get; set; }
        public string Constituency { get; set; }
        public string ConstituencyMp { get; set; }
        public string NumberOfEntryForms { get; set; }
        public SchoolType? SchoolType { get; set; }
        public SchoolPhase SchoolPhase { get; set; }
        public string AgeRange { get; set; }
        public string Gender { get; set; }
        public string Nursery { get; set; }
        public string SixthForm { get; set; }
        public string AlternativeProvision { get; set; }
        public string SpecialEducationNeeds { get; set; }
        public string IndependentConverter { get; set; }
        public string SpecialistResourceProvision { get; set; }
        public string FaithStatus { get; set; }
        public FaithType FaithType { get; set; }
        public string TrustId { get; set; }
        public string TrustName { get; set; }
        public TrustType TrustType { get; set; }
    }

    public record KeyContactsResponse
    {
        public string TeamLeader { get; set; }
        public string Grade6 { get; set; }
        public string ProjectDirector { get; set; }
        public string ProjectManager { get; set; }
        public string ChairOfGovernors { get; set; } 
        public string SchoolChairOfGovernors { get; set; }
    }

    public record ProjectRiskOverviewResponse
    {
        public DateTime? Date { get; set; }
        public ProjectRiskRating? RiskRating { get; set; }
        public string Summary { get; set; }
    }

    public record PupilNumbersOverviewResponse
    {
        public int TotalCapacity { get; set; }
        public int Pre16PublishedAdmissionNumber { get; set; }
        public int Post16PublishedAdmissionNumber { get; set; }
        public int MinimumViableNumberForFirstYear { get; set; }
        public int ApplicationsReceived { get; set; }
        public int AcceptedOffers { get; set; }
    }
}
