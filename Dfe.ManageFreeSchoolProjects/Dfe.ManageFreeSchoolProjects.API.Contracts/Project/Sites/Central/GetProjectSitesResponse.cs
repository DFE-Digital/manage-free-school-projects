namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites
{
    public class GetProjectSitesCentralResponse
    {
        public string SchoolName { get; set; }
        public string ProjectId { get; set; }



        public DateOnly? HoTAgreedForSiteForMainSchoolBuildingActual { get; set; }
        public DateOnly? TemporaryAccommodationFirstReadyForOccupationForecast { get; set; }
        public DateOnly? TemporaryAccommodationFirstReadyForOccupationActual { get; set; }
        public DateOnly? MainSchoolBuildingFirstReadyForOccupationForecast { get; set; }
        public DateOnly? MainSchoolBuildingFirstReadyForOccupationActual { get; set; }
        public DateOnly? SiteIdentifiedForMainSchoolBuildingActual { get; set; }
        public string CapitalProjectRag { get; set; }
        public string PlanningSiteId { get; set; }
        public string IsThisTheMainPlanningRecord { get; set; }
        public string PlanningRisk { get; set; }
        public string PlanningDecision { get; set; }
        public string SiteId { get; set; }
        public string TypeOfSite { get; set; }
        public string SiteStatus { get; set; }
        public string PostcodeOfSite { get; set; }
        public DateOnly? PracticalCompletionCertificateIssuedDateA { get; set; }
        public string RegionalHead { get; set; }
        public string ProjectDirector { get; set; }
        public string ProjectManager { get; set; }
        public string TemporaryRagRating { get; set; }
        public string CapitalProjectRagRatingCommentary { get; set; }
        public string TemporaryRagRatingCommentary { get; set; }
        public DateOnly? DateOfHoTSecuredOnTemporaryAccommodationSiteIfRequired { get; set; }
        public DateOnly? LastRefreshDate { get; set; }
        public string WillTheProjectOpenInTemporaryAccommodation { get; set; }
        public DateOnly? HoTsAgreedForTemporarySiteForecast { get; set; }
        public DateOnly? ContractorForTemporarySiteAppointedForecast { get; set; }
        public DateOnly? ContractorForTemporarySiteAppointedActual { get; set; }
        public DateOnly? DateOfPlanningDecisionForTemporarySiteMainPlanningRecordForecast { get; set; }
        public DateOnly? DateOfPlanningDecisionForTemporarySiteMainPlanningRecordActual { get; set; }
        public string TemporarySitePlanningDecision { get; set; }
        public DateOnly? HoTsAgreedForSiteForMainSchoolBuildingForecast { get; set; }
        public DateOnly? ContractorForSiteForMainSchoolBuildingAppointedForecast { get; set; }
        public DateOnly? ContractorForSiteForMainSchoolBuildingAppointedActual { get; set; }
        public DateOnly? DateOfPlanningDecisionForMainSiteMainPlanningRecordForecast { get; set; }
        public DateOnly? DateOfPlanningDecisionForMainSiteMainPlanningRecordActual { get; set; }
        public string TemporarySiteAddress { get; set; }
        public string TemporarySitePostcode { get; set; }
        public string TemporarySitePlanningRisk { get; set; }
        public DateOnly? DateTemporarySitePlanningApprovalGranted { get; set; }
        public string MainSiteAddress { get; set; }
        public DateOnly? DateMainSitePlanningApprovalGranted { get; set; }
    }
}
