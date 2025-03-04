using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using SchoolType = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.SchoolType;
using ProjectStatusType = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ProjectStatus;
using ProjectCancelledReasonType = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ProjectCancelledReason;
using ProjectWithdrawnReasonType = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ProjectWithdrawnReason;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project
{
    public static class ProjectMapper
    {
        public static SchoolType ToSchoolType(string schoolType)
        {
            return schoolType switch
            {
                "FS - AP" => SchoolType.AlternativeProvision,
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
                SchoolType.Special => "FS - Special",
                SchoolType.StudioSchool => "SS",
                SchoolType.Mainstream => "FS - Mainstream",
                SchoolType.UniversityTechnicalCollege => "UTC",
                SchoolType.FurtherEducation => "FE",
                SchoolType.VoluntaryAided => "VA",
                _ => "NotSet"
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
                _ => "NotSet"
            };
        }

        public static FaithType ToFaithType(string faithTypeDescription)
        {
            return faithTypeDescription switch
            {
                "Church of England" => FaithType.ChurchOfEngland,
                "Greek Orthodox" => FaithType.GreekOrthodox,
                "Roman Catholic" => FaithType.RomanCatholic,
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
        
        public static ProjectStatusType ToProjectStatusType(string projectStatus)
        {
            return projectStatus?.ToLower() switch
            {
                "pre-opening" => ProjectStatusType.Preopening,
                "open" => ProjectStatusType.Open,
                "closed" => ProjectStatusType.Closed,
                "cancelled during pre-opening" => ProjectStatusType.Cancelled,
                "cancelled" => ProjectStatusType.Cancelled,
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
                ProjectStatusType.Open => "Open",
                ProjectStatusType.Closed => "Closed",
                ProjectStatusType.Cancelled => "Cancelled during pre-opening",
                ProjectStatusType.WithdrawnInPreOpening => "Withdrawn in pre-opening",
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
                _ => ""
            };
        }
    }
}