using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Summary
{
    public class GetProjectSummaryResponse
    {
        public string ProjectTitle { get; set; }

        public string ProjectId { get; set; }

        public string TrustName { get; set; }

        public string Region { get; set; }

        public string LocalAuthority { get; set; }

        public string RealisticOpeningYear { get; set; }

        public string ProjectStatus { get; set; }

        public string ProjectManagedBy { get; set; }

        public string ProjectType { get; set; }

        public string ProjectManagedByEmail { get; set; }

        public string SchoolType { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
