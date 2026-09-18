using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.ManageFreeSchoolProjects.TagHelpers
{
    [HtmlTargetElement("govuk-create-proposal-title", TagStructure = TagStructure.WithoutEndTag)]
    public class CreateProposalTitleTagHelper(IHtmlHelper htmlHelper) : ProposalTitleTagHelperBase(htmlHelper)
    {
        protected override string Caption => "Create a proposal";
    }
}
