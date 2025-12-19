using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.ManageFreeSchoolProjects.TagHelpers
{

    [HtmlTargetElement("govuk-status-tag", TagStructure = TagStructure.WithoutEndTag)]
    public class StatusTagHelper : TagHelper
    {
        private readonly IHtmlHelper _htmlHelper;

        [HtmlAttributeName("id")]
        public string Id { get; set; }

        [HtmlAttributeName("status")]
        public ProjectStatus Status { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var tagColour = Status switch
            {
                ProjectStatus.ApplicationStage => "purple",
                ProjectStatus.ApplicationCompetitionStage => "purple",
                ProjectStatus.Cancelled => "orange",
                ProjectStatus.CancelledTrustCompetition => "orange",
                ProjectStatus.Closed => "blue",
                ProjectStatus.Open => "turquoise",
                ProjectStatus.OpenNotIncludedInFigures => "turquoise",
                ProjectStatus.Preopening => "yellow",
                ProjectStatus.PreopeningNotIncludedInFigures => "yellow",
                ProjectStatus.Rejected => "orange",
                ProjectStatus.WithdrawnDuringApplication => "orange",
                ProjectStatus.WithdrawnInPreOpening => "orange",
                _ => "grey"
            };

            var tagClass = $"govuk-tag govuk-tag--{tagColour}";

            output.TagName = "strong";
            output.Attributes.SetAttribute("class", tagClass);
            output.Attributes.SetAttribute("id", Id);
            output.Content.SetHtmlContent(Status.ToDescription());

            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
