﻿namespace Dfe.ManageFreeSchoolProjects.Constants
{
    public static class RouteConstants
    {
        public const string ProjectOverview = "/projects/{0}/overview";
        public const string TaskList = "/projects/{0}/tasks";
        public const string CreateProject = "/project/create";

        public const string ViewSchoolTask = TaskList + "/school";
        public const string EditSchoolTask = ViewSchoolTask + "/edit";

        public const string ViewDatesTask = TaskList + "/dates";
        public const string EditDatesTask = ViewDatesTask + "/edit";

        public const string ViewTrustTask = TaskList + "/trust";
        public const string SearchTrustTask = ViewTrustTask + "/search";
        public const string EditTrustTask = ViewTrustTask + "/edit/{1}";

        public const string ViewConstituency = TaskList + "/constituency";
        public const string SearchConstituency = ViewConstituency + "/search";
        public const string EditConstituency = ViewConstituency + "/edit?search={1}";

        public const string ViewRegionAndLocalAuthorityTask = TaskList + "/region-and-localauthority";
        public const string EditRegion = ViewRegionAndLocalAuthorityTask + "/region/edit";
        public const string EditLocalAuthority = ViewRegionAndLocalAuthorityTask + "/localauthority/edit?region={1}";

        public const string ViewRiskAppraisalMeetingTask = TaskList + "/risk-appraisal-meeting";
        public const string EditRiskAppraisalMeetingTask = ViewRiskAppraisalMeetingTask + "/edit";
        
        public const string CreateProjectMethod = CreateProject + "/method";
        public const string CreateProjectId = CreateProject + "/projectid";
        public const string CreateProjectSchool = CreateProject + "/school";
        public const string CreateProjectRegion = CreateProject + "/region";
        public const string CreateProjectLocalAuthority = CreateProject + "/localauthority";
        public const string CreateProjectCheckYourAnswers = CreateProject + "/checkyouranswers";
        public const string CreateProjectConfirmation = CreateProject + "/confirmation";
        public const string CreateProjectSchoolType = CreateProject + "/school-type";
        public const string CreateProjectSearchTrust = CreateProject + "/trust/search";
        public const string CreateProjectConfirmTrust = CreateProject + "/trust/confirm/{0}";
        public const string CreateNotifyUser = CreateProject + "/notifyuser";
    }
}