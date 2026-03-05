using System.Collections.Generic;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.TagHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace Dfe.ManageFreeSchoolProjects.Tests.TagHelpers
{
    public class RadiosListConditionalItemTagHelperTests
    {
        [Fact]
        public void Process_GeneratesCorrectHtml()
        {
            // Arrange
            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test");

            var output = new TagHelperOutput(
                "govuk-radios-list-conditional-item",
                new TagHelperAttributeList(),
                (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Inner Content");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var modelMetadata = modelMetadataProvider.GetMetadataForType(typeof(string));
            var modelExplorer = new ModelExplorer(modelMetadataProvider, modelMetadata, "CurrentValue");

            var helper = new RadiosListConditionalItemTagHelper
            {
                Id = "TestId",
                Value = "Value1",
                Description = "Description",
                Hint = "Hint",
                Name = "RadioName",
                For = new ModelExpression("Property", modelExplorer),
                RevealedFieldId = "revealed-id"
            };

            // Act
            helper.Process(context, output);

            // Assert
            output.TagName.Should().BeNull();
            
            var preContent = output.PreContent.GetContent();
            preContent.Should().Contain("id=\"TestId\"");
            preContent.Should().Contain("value=\"Value1\"");
            preContent.Should().Contain("name=\"RadioName\"");
            preContent.Should().Contain("aria-describedby=\"testid-hint revealed-id-label\"");
            preContent.Should().Contain("Description");
            preContent.Should().Contain("Hint");
            preContent.Should().Contain("id=\"TestId-conditional\"");

            var postContent = output.PostContent.GetContent();
            postContent.Should().Be("</div>");
        }
    }
}
