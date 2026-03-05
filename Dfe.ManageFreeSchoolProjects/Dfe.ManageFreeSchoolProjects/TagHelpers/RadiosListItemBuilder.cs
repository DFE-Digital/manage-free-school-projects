using Microsoft.AspNetCore.Mvc.ViewFeatures;

internal static class RadiosListItemBuilder
{

    public static string BuildHint(string id, string hint)
    {
        return $@"<div id=""{id.ToLower()}-hint"" class=""govuk-hint govuk-radios__hint"">
                         {hint}
                     </div>";
    }

    public static string BuildLabel(string id, string description)
    {
        return $@"<label class=""govuk-label govuk-radios__label"" for=""{id}"">
                         {description}
                     </label>";
    }

    public static string BuildRadioInput(string id, string value, ModelExpression aspfor, string name = null, string conditionallink = null, bool hasHint = false)
    {
        var radioChecked = value == aspfor.Model?.ToString() ? "checked=\"checked\"" : "";
        var conditional = conditionallink != null ? $@"data-aria-controls=""{conditionallink}""" : "";
        var nameAttributeAndValue = !string.IsNullOrEmpty(name) ? $@"name=""{name}""" : ""; 
        var ariaDescribedBy = hasHint ? $@"aria-describedby=""{id.ToLower()}-hint""" : "";
        
        return $@"<input class=""govuk-radios__input"" id=""{id}""  type=""radio"" value=""{value}"" {nameAttributeAndValue} {radioChecked} {conditional} {ariaDescribedBy}/>";
    }
}