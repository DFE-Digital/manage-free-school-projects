using Dfe.ManageFreeSchoolProjects.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.TagHelpers
{
    /// <summary>
    /// Renders the page heading for a proposal journey. The create and update journeys share the
    /// same markup and only differ by the caption shown above the heading.
    /// </summary>
    public abstract class ProposalTitleTagHelperBase : TagHelper
    {
        [HtmlAttributeName("id")]
        public string Id { get; set; }

        [HtmlAttributeName("name")]
        public string Name { get; set; }

        [HtmlAttributeName("label")]
        public string Label { get; set; }

        [HtmlAttributeName("for")]
        public string For { get; set; }

        [HtmlAttributeName("test-id-prefix")]
        public string TestIdPrefix { get; set; }

        [HtmlAttributeName("add-margin")]
        public bool AddMargin { get; set; } = false;

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlHelper _htmlHelper;

        protected ProposalTitleTagHelperBase(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        /// <summary>
        /// The caption rendered above the heading, for example "Create a proposal".
        /// </summary>
        protected abstract string Caption { get; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (_htmlHelper is IViewContextAware viewContextAware)
            {
                viewContextAware.Contextualize(ViewContext);
            }

            if (string.IsNullOrWhiteSpace(Id))
            {
                Id = Name;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = Id;
            }

            if (string.IsNullOrWhiteSpace(TestIdPrefix))
            {
                TestIdPrefix = "title";
            }

            var content = await _htmlHelper.PartialAsync("_ProposalTitle", new CreateTitleViewModel
            {
                Id = Id,
                Name = Name,
                Label = Label,
                For = For,
                TestIdPrefix = TestIdPrefix,
                AddMargin = AddMargin,
                Caption = Caption
            });

            output.TagName = null;
            output.PostContent.AppendHtml(content);
        }
    }
}
