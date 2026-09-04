using Microsoft.AspNetCore.Html;

namespace Dfe.ManageFreeSchoolProjects.ViewHelpers
{
    /// <summary>
    /// Renders the summary rows shared by the proposal check your answers and details pages. Both
    /// list the same answers and differ only in where the "Change" link points.
    /// </summary>
    public static class ProposalSummary
    {
        public static IHtmlContent RenderSummaryRow(
            string key, string value, string link, string visuallyHiddenText = "", string testId = "")
        {
            var renderedValue = string.IsNullOrEmpty(value) || value == "NotSet"
                ? "<span class=\"empty\">Empty</span>"
                : value;

            var htmlString = $@"
        <div class=""govuk-summary-list__row"">
            <dt class=""govuk-summary-list__key"">{key}</dt>
            <dd class=""govuk-summary-list__value"" data-testid=""{testId}"">{renderedValue}</dd>
            <dd class=""govuk-summary-list__actions"">
                <a class=""govuk-link"" href=""{link}"">Change<span class=""govuk-visually-hidden"">{visuallyHiddenText}</span></a>
            </dd>
        </div>";

            return new HtmlString(htmlString);
        }
    }
}
