﻿using Dfe.ManageFreeSchoolProjects.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.TagHelpers
{
    [HtmlTargetElement("govuk-checkbox-input", TagStructure = TagStructure.WithoutEndTag)]
    public class CheckboxInputTagHelper : InputTagHelperBase
    {
        [HtmlAttributeName("heading-label")]
        public string HeadingLabel { get; set; }

        [HtmlAttributeName("add-margin")]
        public bool AddMargin { get; set; } = true;

        public CheckboxInputTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

        protected override async Task<IHtmlContent> RenderContentAsync()
        {
            CheckboxInputViewModel model = new() 
            { 
                Id = Id,
                TestId = TestId,
                Name = Name, 
                Label = Label, 
                HeadingLabel = 
                HeadingLabel, 
                BoldLabel = BoldLabel, 
                Value = For.Model?.ToString(),
                AddMargin = AddMargin

            };

            return await _htmlHelper.PartialAsync("_CheckboxInput", model);
        }
    }
}
