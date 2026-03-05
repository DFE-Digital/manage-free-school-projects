using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Dfe.ManageFreeSchoolProjects.TagHelpers
{
    public static class RadiosListItemBuilder
    {
        public static string BuildHint(string id, string hint)
        {
            var lowerId = id.ToLower();
            return $@"<div id=""{lowerId}-hint"" class=""govuk-hint govuk-radios__hint"">
                             {hint}
                         </div>";
        }

        public static string BuildLabel(string id, string description)
        {
            return $@"<label class=""govuk-label govuk-radios__label"" for=""{id}"">
                             {description}
                         </label>";
        }

        public static string BuildRadioInput(RadioInputOptions options)
        {
            var radioChecked = options.Value == options.For.Model?.ToString() ? "checked=\"checked\"" : "";
            var conditional = options.ConditionalLink != null ? $@"data-aria-controls=""{options.ConditionalLink}""" : "";
            var nameAttributeAndValue = !string.IsNullOrEmpty(options.Name) ? $@"name=""{options.Name}""" : "";

            var ariaDescribedByList = new List<string>();
            if (options.HasHint) ariaDescribedByList.Add($"{options.Id.ToLower()}-hint");
            if (!string.IsNullOrEmpty(options.AdditionalAriaDescribedBy)) ariaDescribedByList.Add(options.AdditionalAriaDescribedBy);

            var ariaDescribedBy = ariaDescribedByList.Count > 0 ? $@"aria-describedby=""{string.Join(" ", ariaDescribedByList)}""" : "";

            return $@"<input class=""govuk-radios__input"" id=""{options.Id}""  type=""radio"" value=""{options.Value}"" {nameAttributeAndValue} {radioChecked} {conditional} {ariaDescribedBy}/>";
        }
    }

    public class RadioInputOptions
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public ModelExpression For { get; set; }
        public string Name { get; set; }
        public string ConditionalLink { get; set; }
        public bool HasHint { get; set; }
        public string AdditionalAriaDescribedBy { get; set; }
    }
}