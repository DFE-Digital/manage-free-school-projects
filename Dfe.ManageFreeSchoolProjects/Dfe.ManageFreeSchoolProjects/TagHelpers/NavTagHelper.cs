using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dfe.ManageFreeSchoolProjects.TagHelpers
{
    [HtmlTargetElement("mfsp-nav", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class NavTagHelper : TagHelper

    {
        private readonly IHtmlHelper _htmlHelper;
        
        [HtmlAttributeName("project-id")]
        public string ProjectId { get; set; }

        [HtmlAttributeName("current-index")]
        public int CurrentIndex { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }
        
        public NavTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public async override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (_htmlHelper is IViewContextAware viewContextAware)
            {
                viewContextAware.Contextualize(ViewContext);
            }

            var model = new NavViewModel()
            {
                ProjectId = ProjectId,
                CurrentIndex= CurrentIndex
            };
            
            var content = await _htmlHelper.PartialAsync("_Nav", model);

            output.TagName = null;
            output.PostContent.AppendHtml(content);
           
        }
    }
    
    public class NavViewModel
    {
        public string ProjectId { get; set; }
        public int CurrentIndex { get; set; }

    }
}
