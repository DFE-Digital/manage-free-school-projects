using System.IO;
using System.Text.Encodings.Web;
using Dfe.ManageFreeSchoolProjects.ViewHelpers;
using FluentAssertions;

namespace Dfe.ManageFreeSchoolProjects.Tests.ViewHelpers
{
    /// <summary>
    /// The summary rows shared by the proposal check your answers and details pages.
    /// </summary>
    public class ProposalSummaryTests
    {
        [Fact]
        public void RenderSummaryRow_ShowsTheAnswerAndLinksToTheChangePage()
        {
            var html = Render("Name of Diocese", "Diocese of London", "/change-me");

            html.Should().Contain("Name of Diocese");
            html.Should().Contain("Diocese of London");
            html.Should().Contain(@"href=""/change-me""");
            html.Should().Contain("Change");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("NotSet")]
        public void RenderSummaryRow_WhenThereIsNoAnswer_ShowsItAsEmpty(string? value)
        {
            var html = Render("Trust name", value, "/change-me");

            html.Should().Contain(@"<span class=""empty"">Empty</span>");
        }

        [Fact]
        public void RenderSummaryRow_TagsTheValueWithTheTestId()
        {
            var html = Render("Trust name", "Test Trust", "/change-me", testId: "trust-name");

            html.Should().Contain(@"data-testid=""trust-name""");
        }

        [Fact]
        public void RenderSummaryRow_AddsTheVisuallyHiddenTextToTheChangeLink()
        {
            var html = Render("Trust name", "Test Trust", "/change-me", visuallyHiddenText: " the trust name");

            html.Should().Contain(@"<span class=""govuk-visually-hidden""> the trust name</span>");
        }

        private static string Render(
            string key, string? value, string link, string visuallyHiddenText = "", string testId = "")
        {
            var content = ProposalSummary.RenderSummaryRow(key, value, link, visuallyHiddenText, testId);

            using var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);

            return writer.ToString();
        }
    }
}
