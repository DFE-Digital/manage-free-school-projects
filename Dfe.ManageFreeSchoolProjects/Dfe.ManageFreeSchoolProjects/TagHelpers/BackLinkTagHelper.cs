using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace Dfe.ManageFreeSchoolProjects.TagHelpers
{
    [HtmlTargetElement("govuk-back-link", TagStructure = TagStructure.WithoutEndTag)]
	public class BackLinkTagHelper : TagHelper
	{
		[HtmlAttributeName("href")]
		public string Href { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHtmlHelper _htmlHelper;

        public BackLinkTagHelper(IHttpContextAccessor httpContextAccessor, IHtmlHelper htmlHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _htmlHelper = htmlHelper;
        }

        public override async void Process(TagHelperContext context, TagHelperOutput output)
		{
            if (_htmlHelper is IViewContextAware viewContextAware)
            {
                viewContextAware.Contextualize(ViewContext);
            }

            var model = new BackLinkViewModel()
            {
                Href = Href,
                BackBehaviour = GetBackBehaviour()
            };

            var content = await _htmlHelper.PartialAsync("_BackLink", model);

            output.TagName = null;
            output.PostContent.AppendHtml(content);
        }

        private BackBehaviour GetBackBehaviour()
        {
            if (_httpContextAccessor.HttpContext.Request.Query.ContainsKey("back"))
            {
                var backQuery = _httpContextAccessor.HttpContext.Request.Query["back"].ToString();

                // Validate the query parameter before parsing to prevent injection
                if (!string.IsNullOrWhiteSpace(backQuery) && 
                    Enum.TryParse<BackBehaviour>(backQuery, true, out BackBehaviour backBehaviourEnum) &&
                    Enum.IsDefined(typeof(BackBehaviour), backBehaviourEnum))
                {
                    return backBehaviourEnum;
                }
                
                // If validation fails, return safe default
                return BackBehaviour.DirectLink;
            }

            return BackBehaviour.DirectLink;
        }
	}

    public class BackLinkViewModel
    {
        public string Href { get; set; }
        public BackBehaviour BackBehaviour { get; set; }
    }

    public enum BackBehaviour
    {
        DirectLink = 0,
        Previous = 1,
    }
}
