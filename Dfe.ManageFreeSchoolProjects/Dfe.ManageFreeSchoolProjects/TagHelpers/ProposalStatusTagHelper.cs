using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.ManageFreeSchoolProjects.TagHelpers
{
    [HtmlTargetElement("govuk-proposal-status-tag", TagStructure = TagStructure.WithoutEndTag)]
    public class ProposalStatusTagHelper : TagHelper
    {
        [HtmlAttributeName("id")]
        public string Id { get; set; }

        [HtmlAttributeName("status")]
        public ProposalStatus Status { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var tagColour = Status switch
            {
                ProposalStatus.Active => "yellow",
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
