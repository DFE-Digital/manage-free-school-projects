using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.ManageFreeSchoolProjects.TagHelpers
{
    [HtmlTargetElement("govuk-radios-list-conditional-item", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class RadiosListConditionalItemTagHelper : TagHelper
    {

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeName("id")]
        public string Id { get; set; }

        [HtmlAttributeName("value")]
        public string Value { get; set; }

        [HtmlAttributeName("description")]
        public string Description { get; set; }

        [HtmlAttributeName("hint")]
        public string Hint { get; set; }

        [HtmlAttributeName("name")]
        public string Name { get; set; }

        [HtmlAttributeName("revealed-field-id")]
        public string RevealedFieldId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var conditionallink = Id + "-conditional";

            output.TagName = null;

            var additionalAriaDescribedBy = !string.IsNullOrEmpty(RevealedFieldId) ? $"{RevealedFieldId}-label" : null;

            output.PreContent.SetHtmlContent($@"<div class=""govuk-radios__item"">");
            output.PreContent.AppendHtml(RadiosListItemBuilder.BuildRadioInput(Id, Value, For, Name, conditionallink, !string.IsNullOrEmpty(Hint), additionalAriaDescribedBy));
            output.PreContent.AppendHtml(RadiosListItemBuilder.BuildLabel(Id, Description));
            output.PreContent.AppendHtml(RadiosListItemBuilder.BuildHint(Id, Hint));
            output.PreContent.AppendHtml("</div>");
            output.PreContent.AppendHtml(BuildConditionalArea(conditionallink));

            output.PostContent.SetHtmlContent("</div>");

            output.TagMode = TagMode.StartTagAndEndTag;
        }

        private string BuildConditionalArea(string conditionallink)
        {
            return $@"<div class=""govuk-radios__conditional govuk-radios__conditional--hidden"" id=""{conditionallink}"">";
        }

    }
}
