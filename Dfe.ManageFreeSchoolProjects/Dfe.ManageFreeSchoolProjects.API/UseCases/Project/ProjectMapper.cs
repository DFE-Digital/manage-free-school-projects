using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using System.Diagnostics.Eventing.Reader;
using ProjectCancelledReasonType = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ProjectCancelledReason;
using ProjectStatusType = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ProjectStatus;
using ProjectWithdrawnReasonType = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ProjectWithdrawnReason;
using SchoolType = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.SchoolType;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project
{
    public static class ProjectMapper
    {
        private const string NotSet = "NotSet";
        private const string ChurchOfEngland = "Church of England";
        private const string RomanCatholic = "Roman Catholic";

        public static SchoolType ToSchoolType(string schoolType)
        {
            return schoolType switch
            {
                "FS - AP" => SchoolType.AlternativeProvision,
                "FS - AP/PRU" => SchoolType.AlternativeProvisionPRU,
                "FS - Special" => SchoolType.Special,
                "SS" => SchoolType.StudioSchool,
                "FS - Mainstream" => SchoolType.Mainstream,
                "UTC" => SchoolType.UniversityTechnicalCollege,
                "FE" => SchoolType.FurtherEducation,
                "VA" => SchoolType.VoluntaryAided,
                _ => SchoolType.NotSet
            };
        }

        public static string ToSchoolType(SchoolType? schoolType)
        {
            return schoolType switch
            {
                SchoolType.AlternativeProvision => "FS - AP",
                SchoolType.AlternativeProvisionPRU => "FS - AP/PRU",
                SchoolType.Special => "FS - Special",
                SchoolType.StudioSchool => "SS",
                SchoolType.Mainstream => "FS - Mainstream",
                SchoolType.UniversityTechnicalCollege => "UTC",
                SchoolType.FurtherEducation => "FE",
                SchoolType.VoluntaryAided => "VA",
                _ => NotSet
            };
        }

        public static SchoolPhase ToSchoolPhase(string schoolPhase)
        {
            return schoolPhase switch
            {
                "Primary" => SchoolPhase.Primary,
                "Secondary" => SchoolPhase.Secondary,
                "16-19" => SchoolPhase.SixteenToNineteen,
                "16 to 19" => SchoolPhase.SixteenToNineteen,
                "All-Through" => SchoolPhase.AllThrough,
                "All-through" => SchoolPhase.AllThrough,
                _ => SchoolPhase.NotSet
            };
        }

        public static string ToSchoolPhase(SchoolPhase schoolPhase)
        {
            return schoolPhase switch
            {
                SchoolPhase.Primary => "Primary",
                SchoolPhase.Secondary => "Secondary",
                SchoolPhase.SixteenToNineteen => "16-19",
                SchoolPhase.AllThrough => "All-Through",
                _ => NotSet
            };
        }

        public static string ToFaithType(FaithType faithType)
        {
            return faithType switch
            {
                FaithType.ChurchOfEngland => ChurchOfEngland,
                FaithType.Christian => "Christian",
                FaithType.GreekOrthodox => "Greek Orthodox",
                FaithType.Hindu => "Hindu",
                FaithType.Jewish => "Jewish",
                FaithType.Methodist => "Methodist",
                FaithType.Muslim => "Muslim",
                FaithType.RomanCatholic => RomanCatholic,
                FaithType.Sikh => "Sikh",
                FaithType.Other => "Other",
                FaithType.None => "None",
                _ => null
            };
        }

        public static FaithType ToFaithType(string faithTypeDescription)
        {
            return faithTypeDescription switch
            {
                ChurchOfEngland => FaithType.ChurchOfEngland,
                RomanCatholic => FaithType.RomanCatholic,
                "Greek Orthodox" => FaithType.GreekOrthodox,
                "Hindu" => FaithType.Hindu,
                "Jewish" => FaithType.Jewish,
                "Methodist" => FaithType.Methodist,
                "Muslim" => FaithType.Muslim,
                "Christian" => FaithType.Christian,
                "Sikh" => FaithType.Sikh,
                "Other" => FaithType.Other,
                "None" => FaithType.None,
                _ => EnumParsers.ParseFaithType(faithTypeDescription)
            };
        }
        
        public static TrustType ToTrustType(string trustTypeDescription)
        {
            return trustTypeDescription switch
            {
                "Standalone" => TrustType.SingleAcademyTrust,
                "MAT" => TrustType.MultiAcademyTrust,
                _ => EnumParsers.ParseTrustType(trustTypeDescription)
            };
        }

        public static string ToTrustType(TrustType trustType)
        {
            return trustType switch
            {
                TrustType.SingleAcademyTrust => "Standalone",
                TrustType.MultiAcademyTrust => "MAT",
                _ => throw new ArgumentOutOfRangeException(nameof(trustType), trustType, null)
            };
        }

        public static ProjectStatusType ToProjectStatusType(string projectStatus)
        {
            return projectStatus?.ToLower() switch
            {
                "pre-opening" => ProjectStatusType.Preopening,
                "pre-pipeline" => ProjectStatusType.Prepipeline,
                "open" => ProjectStatusType.Open,
                "closed" => ProjectStatusType.Closed,
                "cancelled during pre-opening" => ProjectStatusType.Cancelled,
                "cancelled" => ProjectStatusType.Cancelled,
                "cancelled trust competition" => ProjectStatusType.CancelledTrustCompetition,
                "withdrawn during pre-opening" => ProjectStatusType.WithdrawnInPreOpening,
                "withdrawn in pre-opening" => ProjectStatusType.WithdrawnInPreOpening,
                "rejected at application stage" => ProjectStatusType.Rejected,
                "application competition stage" => ProjectStatusType.ApplicationCompetitionStage,
                "application stage" => ProjectStatusType.ApplicationStage,
                "open free school - not included in figures" => ProjectStatusType.OpenNotIncludedInFigures,
                "pre-opening - not included in the figures" => ProjectStatusType.PreopeningNotIncludedInFigures,
                "withdrawn at application stage" => ProjectStatusType.WithdrawnDuringApplication,
                _ => ProjectStatusType.Preopening,
            };
        }

        public static string FromProjectStatusType(ProjectStatusType projectStatus)
        {
            return projectStatus switch
            {
                ProjectStatusType.Preopening => "Pre-opening",
                ProjectStatusType.Prepipeline => "Pre-pipeline",
                ProjectStatusType.Open => "Open",
                ProjectStatusType.Closed => "Closed",
                ProjectStatusType.Cancelled => "Cancelled during pre-opening",
                ProjectStatusType.CancelledTrustCompetition => "Cancelled trust competition",
                ProjectStatusType.WithdrawnInPreOpening => "Withdrawn during pre-opening",
                ProjectStatusType.Rejected => "Rejected at application stage",
                ProjectStatusType.ApplicationCompetitionStage => "Application Competition stage",
                ProjectStatusType.ApplicationStage => "Application stage",
                ProjectStatusType.OpenNotIncludedInFigures => "Open free school - Not included in figures", 
                ProjectStatusType.PreopeningNotIncludedInFigures => "Pre-opening - Not included in the figures",
                ProjectStatusType.WithdrawnDuringApplication => "Withdrawn at application stage",
                _ => throw new ArgumentOutOfRangeException(nameof(projectStatus), projectStatus, null)
            };
        }

        public static ProjectCancelledReasonType ToProjectCancelledReasonType(string projectCancelledReason)
        {
            return projectCancelledReason?.ToLower() switch
            {
                "education quality" => ProjectCancelledReasonType.EducationQuality,
                "governance" => ProjectCancelledReasonType.Governance,
                "site and planning issues" => ProjectCancelledReasonType.SiteAndPlanningIssues,
                "pupil numbers" => ProjectCancelledReasonType.PupilNumbers,
                _ => ProjectCancelledReasonType.NotSet
            };
        }

        public static string FromProjectCancelledReasonType(ProjectCancelledReasonType projectCancelledReason)
        {
            return projectCancelledReason switch
            {
                ProjectCancelledReasonType.EducationQuality => "education quality",
                ProjectCancelledReasonType.Governance => "governance",
                ProjectCancelledReasonType.SiteAndPlanningIssues => "site and planning issues",
                ProjectCancelledReasonType.PupilNumbers => "pupil numbers",
                _ => ""
            };
        }

        public static ProjectWithdrawnReasonType ToProjectWithdrawnReasonType(string projectWithdrawnReason)
        {
            return projectWithdrawnReason?.ToLower() switch
            {
                "education quality" => ProjectWithdrawnReasonType.EducationQuality,
                "governance" => ProjectWithdrawnReasonType.Governance,
                "site and planning issues" => ProjectWithdrawnReasonType.SiteAndPlanningIssues,
                "pupil numbers" => ProjectWithdrawnReasonType.PupilNumbers,
                _ => ProjectWithdrawnReasonType.NotSet
            };
        }

        public static string FromProjectWithdrawnReasonType(ProjectWithdrawnReasonType projectWithdrawnReason)
        {
            return projectWithdrawnReason switch
            {
                ProjectWithdrawnReasonType.EducationQuality => "education quality",
                ProjectWithdrawnReasonType.Governance => "governance",
                ProjectWithdrawnReasonType.SiteAndPlanningIssues => "site and planning issues",
                ProjectWithdrawnReasonType.PupilNumbers => "pupil numbers",
                _ => NotSet
            };
        }

        public static string ToProposer(ProposalProposer proposer)
        {
            return proposer switch
            {
                ProposalProposer.AcademyTrust => "Academy trust (including Diocese academy trust)",
                ProposalProposer.Diocese => "Diocese",
                ProposalProposer.AnotherReligiousOrganisation => "Another religious organisation",
                ProposalProposer.LocalAuthorityThatPushedSpecification => "Local authority that published the specification",
                ProposalProposer.AnotherLocalAuthority => "Another local authority",
                ProposalProposer.JointProposal => "Joint proposal between the local authority that published the specification and another local authority",
                _ => string.Empty
            };
        }

        public static ProposalProposer ToProposer(string strProposer)
        {
            return strProposer switch
            {
                "Academy trust (including Diocese academy trust)" => ProposalProposer.AcademyTrust,
                "Diocese" => ProposalProposer.Diocese,
                "Another religious organisation" => ProposalProposer.AnotherReligiousOrganisation,
                "Local authority that published the specification" => ProposalProposer.LocalAuthorityThatPushedSpecification,
                "Another local authority" => ProposalProposer.AnotherLocalAuthority,
                "Joint proposal between the local authority that published the specification and another local authority" => ProposalProposer.JointProposal,
                _ => throw new ArgumentOutOfRangeException(nameof(strProposer), strProposer, null)
            };
        }

        public static string ToDioceseFaithType(FaithOfDiocese faithType)
        {
            return faithType switch
            {
                FaithOfDiocese.ChurchOfEngland => ChurchOfEngland,
                FaithOfDiocese.RomanCatholic => RomanCatholic,
                _ => string.Empty
            };
        }

        public static FaithOfDiocese ToDioceseFaithType(string faithType)
        {
            return faithType switch
            {
                ChurchOfEngland => FaithOfDiocese.ChurchOfEngland,
                RomanCatholic => FaithOfDiocese.RomanCatholic,
                _ => throw new ArgumentOutOfRangeException(nameof(faithType), faithType, null)
            };
        }

        public static FaithStatus ToFaithStatus(string status)
        {
            return status switch
            {
                "Designation" => FaithStatus.Designation,
                "Ethos" => FaithStatus.Ethos,
                "None" => FaithStatus.None,
                NotSet => FaithStatus.NotSet,
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
            };
        }

        public static string ToFaithStatus(FaithStatus status)
        {
            return status switch
            {
                FaithStatus.Designation => "Designation",
                FaithStatus.Ethos => "Ethos",
                FaithStatus.None => "None",
                FaithStatus.NotSet => NotSet,
                _ => string.Empty
            };
        }
    }
}