using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dfe.ManageFreeSchoolProjects.TagHelpers
{
    [HtmlTargetElement("mfsp-nav-item", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class NavItemHelper : TagHelper

    {
        private readonly IHtmlHelper _htmlHelper;
        
        [HtmlAttributeName("href")]
        public string Href { get; set; }

        [HtmlAttributeName("current")]
        public bool Current { get; set; }

        [HtmlAttributeName("description")]
        public string Description { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }
        
        public NavItemHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public async override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (_htmlHelper is IViewContextAware viewContextAware)
            {
                viewContextAware.Contextualize(ViewContext);
            }

            var model = new NavItemViewModel()
            {
                Href = Href,
                Current = Current,
                Description = Description
            };
            
            var content = await _htmlHelper.PartialAsync("_NavItem", model);

            output.TagName = null;
            output.PostContent.AppendHtml(content);
           
        }
    }
    
    public class NavItemViewModel
    {
        public string Href { get; set; }

        public bool Current { get; set; }
        public string Description { get; set; }
       
    }
}
