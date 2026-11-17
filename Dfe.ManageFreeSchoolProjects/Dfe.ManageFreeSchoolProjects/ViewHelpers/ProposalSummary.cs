using Microsoft.AspNetCore.Html;

namespace Dfe.ManageFreeSchoolProjects.ViewHelpers
{
    public static class ProposalSummary
    {
        public static IHtmlContent RenderSummaryRow(
            string key, string value, string link, bool showChangeLink = true, string visuallyHiddenText = "", string testId = "")
        {
            var renderedValue = string.IsNullOrEmpty(value) || value == "NotSet"
                ? "<span class=\"empty\">Empty</span>"
                : value;

            var rowClass = showChangeLink
                ? "govuk-summary-list__row"
                : "govuk-summary-list__row govuk-summary-list__row--no-actions";

            var actions = showChangeLink
                ? $@"
            <dd class=""govuk-summary-list__actions"">
                <a class=""govuk-link"" href=""{link}"">Change<span class=""govuk-visually-hidden"">{visuallyHiddenText}</span></a>
            </dd>"
                : string.Empty;

            var htmlString = $@"
        <div class=""{rowClass}"">
            <dt class=""govuk-summary-list__key"">{key}</dt>
            <dd class=""govuk-summary-list__value"" data-testid=""{testId}"">{renderedValue}</dd>{actions}
        </div>";

            return new HtmlString(htmlString);
        }
    }
}
