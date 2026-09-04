using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.ManageFreeSchoolProjects.TagHelpers
{
    [HtmlTargetElement("govuk-update-proposal-title", TagStructure = TagStructure.WithoutEndTag)]
    public class UpdateProposalTitleTagHelper(IHtmlHelper htmlHelper) : ProposalTitleTagHelperBase(htmlHelper)
    {
        protected override string Caption => "Update a proposal";
    }
}
