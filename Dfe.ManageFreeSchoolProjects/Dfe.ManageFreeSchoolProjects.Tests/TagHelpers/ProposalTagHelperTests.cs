using System.Collections.Generic;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.TagHelpers;
using Dfe.ManageFreeSchoolProjects.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.TagHelpers
{
    public class ProposalStatusTagHelperTests
    {
        [Fact]
        public void Process_RendersAnActiveProposalAsAYellowTag()
        {
            var output = BuildOutput("govuk-proposal-status-tag");
            var helper = new ProposalStatusTagHelper { Id = "status-1", Status = ProposalStatus.Active };

            helper.Process(BuildContext(), output);

            output.TagName.Should().Be("strong");
            output.TagMode.Should().Be(TagMode.StartTagAndEndTag);
            output.Attributes["class"].Value.Should().Be("govuk-tag govuk-tag--yellow");
            output.Attributes["id"].Value.Should().Be("status-1");
            output.Content.GetContent().Should().Be("Active");
        }

        [Fact]
        public void Process_SetsTheIdItWasGiven()
        {
            var output = BuildOutput("govuk-proposal-status-tag");
            var helper = new ProposalStatusTagHelper { Id = "proposal-status", Status = ProposalStatus.Active };

            helper.Process(BuildContext(), output);

            output.Attributes["id"].Value.Should().Be("proposal-status");
        }

        private static TagHelperContext BuildContext() =>
            new(new TagHelperAttributeList(), new Dictionary<object, object>(), "test");

        private static TagHelperOutput BuildOutput(string tagName) =>
            new(tagName, new TagHelperAttributeList(), (_, _) =>
                Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
    }

    public class CreateProposalTitleTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_RendersTheTitlePartialInPlaceOfTheTag()
        {
            var helper = BuildHelper(out _, out var captured);
            helper.Id = "proposer";
            helper.Name = "proposer";
            helper.Label = "Who is the proposer?";
            helper.AddMargin = true;

            var output = BuildOutput();

            await helper.ProcessAsync(BuildContext(), output);

            output.TagName.Should().BeNull();
            output.PostContent.GetContent().Should().Be("<h1>Who is the proposer?</h1>");

            captured.Value!.Id.Should().Be("proposer");
            captured.Value!.Name.Should().Be("proposer");
            captured.Value!.Label.Should().Be("Who is the proposer?");
            captured.Value!.AddMargin.Should().BeTrue();
        }

        [Fact]
        public async Task ProcessAsync_WhenOnlyTheNameIsGiven_UsesItAsTheId()
        {
            var helper = BuildHelper(out _, out var captured);
            helper.Name = "proposer";

            await helper.ProcessAsync(BuildContext(), BuildOutput());

            captured.Value!.Id.Should().Be("proposer");
            captured.Value!.Name.Should().Be("proposer");
        }

        [Fact]
        public async Task ProcessAsync_WhenOnlyTheIdIsGiven_UsesItAsTheName()
        {
            var helper = BuildHelper(out _, out var captured);
            helper.Id = "proposer";

            await helper.ProcessAsync(BuildContext(), BuildOutput());

            captured.Value!.Id.Should().Be("proposer");
            captured.Value!.Name.Should().Be("proposer");
        }

        [Fact]
        public async Task ProcessAsync_WhenNoTestIdPrefixIsGiven_DefaultsToTitle()
        {
            var helper = BuildHelper(out _, out var captured);

            await helper.ProcessAsync(BuildContext(), BuildOutput());

            captured.Value!.TestIdPrefix.Should().Be("title");
        }

        [Fact]
        public async Task ProcessAsync_KeepsAnExplicitTestIdPrefix()
        {
            var helper = BuildHelper(out _, out var captured);
            helper.TestIdPrefix = "proposer";

            await helper.ProcessAsync(BuildContext(), BuildOutput());

            captured.Value!.TestIdPrefix.Should().Be("proposer");
        }

        [Fact]
        public async Task ProcessAsync_GivesTheHtmlHelperTheViewContext()
        {
            var helper = BuildHelper(out var htmlHelper, out _);
            var viewContext = new ViewContext();
            helper.ViewContext = viewContext;

            await helper.ProcessAsync(BuildContext(), BuildOutput());

            ((IViewContextAware)htmlHelper).Received(1).Contextualize(viewContext);
        }

        private static CreateProposalTitleTagHelper BuildHelper(
            out IHtmlHelper htmlHelper, out CapturedModel captured)
        {
            var helper = Substitute.For<IHtmlHelper, IViewContextAware>();
            htmlHelper = helper;

            var capturedModel = new CapturedModel();
            captured = capturedModel;

            helper.PartialAsync(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<ViewDataDictionary>())
                .Returns(call =>
                {
                    capturedModel.Value = call.ArgAt<object>(1) as CreateTitleViewModel;
                    return Task.FromResult<IHtmlContent>(
                        new HtmlString($"<h1>{capturedModel.Value?.Label}</h1>"));
                });

            return new CreateProposalTitleTagHelper(helper) { ViewContext = new ViewContext() };
        }

        private static TagHelperContext BuildContext() =>
            new(new TagHelperAttributeList(), new Dictionary<object, object>(), "test");

        private static TagHelperOutput BuildOutput() =>
            new("govuk-create-proposal-title", new TagHelperAttributeList(), (_, _) =>
                Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        internal sealed class CapturedModel
        {
            public CreateTitleViewModel? Value { get; set; }
        }
    }
}
