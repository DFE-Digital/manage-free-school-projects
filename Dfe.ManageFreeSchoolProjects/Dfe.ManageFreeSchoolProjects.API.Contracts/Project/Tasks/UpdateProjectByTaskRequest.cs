﻿namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks
{
    public class UpdateProjectByTaskRequest
    {
        public DatesTask Dates { get; set; }
        public SchoolTask School { get; set; }
        public TrustTask Trust { get; set; }
        public RegionAndLocalAuthorityTask RegionAndLocalAuthorityTask { get; set; }
        public RiskAppraisalMeetingTask RiskAppraisalMeeting { get; set; }
		public ConstituencyTask Constituency { get; set; }

        public string TaskToUpdate
        {
            get
            {
                if (School != null)
                    return "School";
                if (Dates != null)
                    return "Dates";
                if (Trust != null)
                    return "Trust";
                if (RegionAndLocalAuthorityTask != null)
                    return "RegionAndLocalAuthority";
                if (RiskAppraisalMeeting != null)
                    return "RiskAppraisalMeeting";
				if (Constituency != null)
					return "Constituency";
				return null;
            }
        }
    }
}
