using Dfe.ManageFreeSchoolProjects.TagHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Xunit;

namespace Dfe.ManageFreeSchoolProjects.Tests.TagHelpers
{
    public class RadiosListItemBuilderTests
    {
        [Fact]
        public void BuildHint_ReturnsCorrectHtml()
        {
            // Arrange
            var id = "TestId";
            var hint = "This is a hint";

            // Act
            var result = RadiosListItemBuilder.BuildHint(id, hint);

            // Assert
            result.Should().Contain("id=\"testid-hint\"");
            result.Should().Contain(hint);
        }

        [Fact]
        public void BuildLabel_ReturnsCorrectHtml()
        {
            // Arrange
            var id = "TestId";
            var description = "This is a description";

            // Act
            var result = RadiosListItemBuilder.BuildLabel(id, description);

            // Assert
            result.Should().Contain("for=\"TestId\"");
            result.Should().Contain(description);
        }

        [Fact]
        public void BuildRadioInput_WithMinimumOptions_ReturnsCorrectHtml()
        {
            // Arrange
            var options = new RadioInputOptions
            {
                Id = "TestId",
                Value = "Value1",
                For = CreateModelExpression("Value2")
            };

            // Act
            var result = RadiosListItemBuilder.BuildRadioInput(options);

            // Assert
            result.Should().Contain("id=\"TestId\"");
            result.Should().Contain("value=\"Value1\"");
            result.Should().NotContain("checked");
        }

        [Fact]
        public void BuildRadioInput_Checked_ReturnsCorrectHtml()
        {
            // Arrange
            var options = new RadioInputOptions
            {
                Id = "TestId",
                Value = "Value1",
                For = CreateModelExpression("Value1")
            };

            // Act
            var result = RadiosListItemBuilder.BuildRadioInput(options);

            // Assert
            result.Should().Contain("checked=\"checked\"");
        }

        [Fact]
        public void BuildRadioInput_WithHint_ReturnsAriaDescribedBy()
        {
            // Arrange
            var options = new RadioInputOptions
            {
                Id = "TestId",
                Value = "Value1",
                For = CreateModelExpression("Value2"),
                HasHint = true
            };

            // Act
            var result = RadiosListItemBuilder.BuildRadioInput(options);

            // Assert
            result.Should().Contain("aria-describedby=\"testid-hint\"");
        }

        [Fact]
        public void BuildRadioInput_WithAdditionalAriaDescribedBy_ReturnsAriaDescribedBy()
        {
            // Arrange
            var options = new RadioInputOptions
            {
                Id = "TestId",
                Value = "Value1",
                For = CreateModelExpression("Value2"),
                AdditionalAriaDescribedBy = "additional-id"
            };

            // Act
            var result = RadiosListItemBuilder.BuildRadioInput(options);

            // Assert
            result.Should().Contain("aria-describedby=\"additional-id\"");
        }

        [Fact]
        public void BuildRadioInput_WithHintAndAdditionalAriaDescribedBy_ReturnsBothIds()
        {
            // Arrange
            var options = new RadioInputOptions
            {
                Id = "TestId",
                Value = "Value1",
                For = CreateModelExpression("Value2"),
                HasHint = true,
                AdditionalAriaDescribedBy = "additional-id"
            };

            // Act
            var result = RadiosListItemBuilder.BuildRadioInput(options);

            // Assert
            result.Should().Contain("aria-describedby=\"testid-hint additional-id\"");
        }

        private static ModelExpression CreateModelExpression(object model)
        {
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var modelMetadata = modelMetadataProvider.GetMetadataForType(typeof(string));
            var modelExplorer = new ModelExplorer(modelMetadataProvider, modelMetadata, model);
            return new ModelExpression("name", modelExplorer);
        }
    }
}
