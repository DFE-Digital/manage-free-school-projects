using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace Dfe.ManageFreeSchoolProjects.Constants
{
    public class NavConstants
    {

        public class NavItem
        {
            public string Description { get; internal set; }
            public string Link { get; internal set; }
        }

        public static readonly ImmutableList<NavItem> NavItemList = [
            new NavItem { Description = "About the project", Link = RouteConstants.ProjectOverview },
            new NavItem { Description = "Task list", Link = RouteConstants.TaskList },
            new NavItem { Description = "Risk rating and summary", Link = RouteConstants.ProjectRiskRatingAndSummary },
            new NavItem { Description = "Contacts", Link = RouteConstants.Contacts },
            new NavItem { Description = "Site information", Link = RouteConstants.ViewSiteInformation },
            new NavItem { Description = "Pupil numbers", Link = RouteConstants.PupilNumbersSummary }
        ];


    }
}
