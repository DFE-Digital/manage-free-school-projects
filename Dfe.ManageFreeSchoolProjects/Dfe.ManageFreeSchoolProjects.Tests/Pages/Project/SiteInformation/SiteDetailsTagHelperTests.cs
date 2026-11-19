using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites;
using Dfe.ManageFreeSchoolProjects.Pages.Project.SiteInformation;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.SiteInformation
{
    public class SiteDetailsTagHelperTests
    {
        private readonly IHtmlHelper _htmlHelper = Substitute.For<IHtmlHelper, IViewContextAware>();
        private SiteDetailsViewModel? _capturedModel;

        public SiteDetailsTagHelperTests()
        {
            _htmlHelper.PartialAsync(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<ViewDataDictionary>())
                .Returns(call =>
                {
                    _capturedModel = call.ArgAt<object>(1) as SiteDetailsViewModel;
                    return Task.FromResult<IHtmlContent>(new HtmlString("<div>site</div>"));
                });
        }

        [Fact]
        public async Task ProcessAsync_RendersTheSiteDetailsPartialInPlaceOfTheTag()
        {
            var site = new ProjectSite();
            var viewContext = new ViewContext();
            var helper = new SiteDetailsTagHelper(_htmlHelper)
            {
                Site = site,
                SiteType = "Temporary",
                EditLink = "/projects/FS-1/site-information/newschool/temporary/edit",
                ViewContext = viewContext
            };
            var output = BuildOutput();

            await helper.ProcessAsync(BuildContext(), output);

            output.TagName.Should().BeNull();
            output.PostContent.GetContent().Should().Be("<div>site</div>");

            await _htmlHelper.Received(1).PartialAsync("_SiteDetails", Arg.Any<object>(), Arg.Any<ViewDataDictionary>());
            ((IViewContextAware)_htmlHelper).Received(1).Contextualize(viewContext);

            _capturedModel!.Site.Should().BeSameAs(site);
            _capturedModel.SiteType.Should().Be("Temporary");
            _capturedModel.IdPrefix.Should().Be("temporary");
            _capturedModel.EditLink.Should().Be("/projects/FS-1/site-information/newschool/temporary/edit");
        }

        [Fact]
        public async Task ProcessAsync_WhenNoSiteTypeIsGiven_HasNoIdPrefix()
        {
            var helper = new SiteDetailsTagHelper(_htmlHelper) { Site = new ProjectSite(), ViewContext = new ViewContext() };

            await helper.ProcessAsync(BuildContext(), BuildOutput());

            _capturedModel!.IdPrefix.Should().BeNull();
        }

        private static TagHelperContext BuildContext() =>
            new(new TagHelperAttributeList(), new Dictionary<object, object>(), "test");

        private static TagHelperOutput BuildOutput() =>
            new("govuk-site-details", new TagHelperAttributeList(), (_, _) =>
                Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
    }
}
