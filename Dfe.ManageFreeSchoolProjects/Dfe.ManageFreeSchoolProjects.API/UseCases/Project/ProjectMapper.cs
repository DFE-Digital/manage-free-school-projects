﻿using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using SchoolType = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.SchoolType;
using ProjectStatusType = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ProjectStatus;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;

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
                _ => "NotSet"
            };
        }

        public static SchoolPhase ToSchoolPhase(string schoolPhase)
        {
            return schoolPhase switch
            {
                "Primary" => SchoolPhase.Primary,
                "Secondary" => SchoolPhase.Secondary,
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
                SchoolPhase.SixteenToNineteen => "16 to 19",
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
            return projectStatus switch
            {
                "Pre-opening" => ProjectStatusType.Preopening,
                "Open" => ProjectStatusType.Open,
                "Closed" => ProjectStatusType.Closed,
                "Cancelled during pre-opening" => ProjectStatusType.Cancelled,
                "Cancelled" => ProjectStatusType.Cancelled,
                "Withdrawn during pre-opening" => ProjectStatusType.WithdrawnDuringPreOpening,
                "Withdrawn in pre-opening" => ProjectStatusType.WithdrawnDuringPreOpening,
                "Rejected at application stage" => ProjectStatusType.Rejected,
                "Application Competition stage" => ProjectStatusType.ApplicationCompetitionStage,
                "Application stage" => ProjectStatusType.ApplicationStage,
                "Open free school - Not included in figures" => ProjectStatusType.OpenNotIncludedInFigures,
                "Pre-opening - Not included in the figures" => ProjectStatusType.PreopeningNotIncludedInFigures,
                "Withdrawn at application stage" => ProjectStatusType.WithdrawnDuringApplication,
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
                ProjectStatusType.Cancelled => "Cancelled",
                ProjectStatusType.WithdrawnDuringPreOpening => "Withdrawn during pre-opening",
                ProjectStatusType.Rejected => "Rejected at application stage",
                ProjectStatusType.ApplicationCompetitionStage => "Application Competition stage",
                ProjectStatusType.ApplicationStage => "Application stage",
                ProjectStatusType.OpenNotIncludedInFigures => "Open free school - Not included in figures", 
                ProjectStatusType.PreopeningNotIncludedInFigures => "Pre-opening - Not included in the figures",
                ProjectStatusType.WithdrawnDuringApplication => "Withdrawn at application stage",
                _ => throw new ArgumentOutOfRangeException(nameof(projectStatus), projectStatus, null)
            };
        }
    }
}