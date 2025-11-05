using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Risk;
using System.Collections.Generic;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Risk
{
    public static class RiskRatingLabelBuilder
    {
        public static List<RiskRatingLabel> Build()
        {
            return new List<RiskRatingLabel>()
            {
                new RiskRatingLabel()
                {
                    RiskRating = ProjectRiskRating.Green,
                    Label = "<strong class=\"govuk-tag govuk-tag--green\">Green</strong>",
                    Tags = new List<RiskTag> { new RiskTag { Text = "Green", CssClass = "govuk-tag--green" } }
                },
                new RiskRatingLabel()
                {
                    RiskRating = ProjectRiskRating.AmberGreen,
                    Label = "<strong class=\"govuk-tag govuk-tag--amber\">Amber</strong>&nbsp;<strong class=\"govuk-tag govuk-tag--green\">Green</strong>",
                    Tags = new List<RiskTag> 
                    { 
                        new RiskTag { Text = "Amber", CssClass = "govuk-tag--amber" },
                        new RiskTag { Text = "Green", CssClass = "govuk-tag--green" }
                    }
                },
                new RiskRatingLabel()
                {
                    RiskRating = ProjectRiskRating.AmberRed,
                    Label = "<strong class=\"govuk-tag govuk-tag--amber\">Amber</strong>&nbsp;<strong class=\"govuk-tag govuk-tag--red\">Red</strong>",
                    Tags = new List<RiskTag> 
                    { 
                        new RiskTag { Text = "Amber", CssClass = "govuk-tag--amber" },
                        new RiskTag { Text = "Red", CssClass = "govuk-tag--red" }
                    }
                },
                new RiskRatingLabel()
                {
                    RiskRating = ProjectRiskRating.Red,
                    Label = "<strong class=\"govuk-tag govuk-tag--red\">Red</strong>",
                    Tags = new List<RiskTag> { new RiskTag { Text = "Red", CssClass = "govuk-tag--red" } }
                }
            };
        }
    }

    public class RiskRatingLabel
    {
        public ProjectRiskRating RiskRating { get; set; }
        
        /// <summary>
        /// Deprecated: Use Tags property instead to avoid XSS risks with Html.Raw()
        /// Kept for backward compatibility
        /// </summary>
        public string Label { get; set; }
        
        /// <summary>
        /// Safe structured representation of risk rating tags
        /// </summary>
        public List<RiskTag> Tags { get; set; }
    }

    public class RiskTag
    {
        public string Text { get; set; }
        public string CssClass { get; set; }
    }
}
