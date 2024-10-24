using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dfe.ManageFreeSchoolProjects.ViewModels;

namespace Dfe.ManageFreeSchoolProjects.TagHelpers
{
    [HtmlTargetElement("mfsp-project-header", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class ProjectHeaderTagHelper : TagHelper

    {
        private readonly IHtmlHelper _htmlHelper;
        
        [HtmlAttributeName("project-id")]
        public string ProjectId { get; set; }

        [HtmlAttributeName("school-name")]
        public string SchoolName { get; set; }

        [HtmlAttributeName("back-link")]
        public string BackLink { get; set; }

        [HtmlAttributeName("back-text")]
        public string BackText { get; set; }

        public string PageTitle { get; set; }
        public ProjectStatusViewModel ProjectStatus { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }
        
        public ProjectHeaderTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public async override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (_htmlHelper is IViewContextAware viewContextAware)
            {
                viewContextAware.Contextualize(ViewContext);
            }

            var model = new ProjectHeaderViewModel()
            {
                ProjectId = ProjectId,
                SchoolName = SchoolName,
                BackLink = BackLink,
                BackText = BackText,
                PageTitle = PageTitle,
                ProjectStatus = ProjectStatus
            };
            
            var content = await _htmlHelper.PartialAsync("_ProjectHeader", model);

            output.TagName = null;
            output.PostContent.AppendHtml(content);
           
        }
    }
    
    public class ProjectHeaderViewModel
    {
        public string ProjectId { get; set; }
        public string SchoolName { get; set; }
        public string BackLink { get; set; }
        public string BackText { get; set; }
        public string PageTitle { get; set; }
        public ProjectStatusViewModel ProjectStatus { get; set; }
    }
}
