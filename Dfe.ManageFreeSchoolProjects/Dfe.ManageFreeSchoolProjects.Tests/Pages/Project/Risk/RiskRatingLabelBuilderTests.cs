using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Risk;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Risk;
using FluentAssertions;
using Xunit;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Risk
{
    public class RiskRatingLabelBuilderTests
    {
        [Fact]
        public void Build_ShouldReturn_AllRiskRatings()
        {
            // Act
            var result = RiskRatingLabelBuilder.Build();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(4, "should include all risk rating types");
        }

        [Fact]
        public void Build_ShouldInclude_GreenRating()
        {
            // Act
            var result = RiskRatingLabelBuilder.Build();

            // Assert
            result.Should().Contain(r => r.RiskRating == ProjectRiskRating.Green);
            
            var greenRating = result.First(r => r.RiskRating == ProjectRiskRating.Green);
            greenRating.Label.Should().Contain("Green");
            greenRating.Tags.Should().NotBeNull();
            greenRating.Tags.Should().HaveCount(1);
            greenRating.Tags[0].Text.Should().Be("Green");
            greenRating.Tags[0].CssClass.Should().Be("govuk-tag--green");
        }

        [Fact]
        public void Build_ShouldInclude_AmberGreenRating()
        {
            // Act
            var result = RiskRatingLabelBuilder.Build();

            // Assert
            result.Should().Contain(r => r.RiskRating == ProjectRiskRating.AmberGreen);
            
            var amberGreenRating = result.First(r => r.RiskRating == ProjectRiskRating.AmberGreen);
            amberGreenRating.Label.Should().Contain("Amber").And.Contain("Green");
            amberGreenRating.Tags.Should().NotBeNull();
            amberGreenRating.Tags.Should().HaveCount(2);
            
            amberGreenRating.Tags[0].Text.Should().Be("Amber");
            amberGreenRating.Tags[0].CssClass.Should().Be("govuk-tag--amber");
            
            amberGreenRating.Tags[1].Text.Should().Be("Green");
            amberGreenRating.Tags[1].CssClass.Should().Be("govuk-tag--green");
        }

        [Fact]
        public void Build_ShouldInclude_AmberRedRating()
        {
            // Act
            var result = RiskRatingLabelBuilder.Build();

            // Assert
            result.Should().Contain(r => r.RiskRating == ProjectRiskRating.AmberRed);
            
            var amberRedRating = result.First(r => r.RiskRating == ProjectRiskRating.AmberRed);
            amberRedRating.Label.Should().Contain("Amber").And.Contain("Red");
            amberRedRating.Tags.Should().NotBeNull();
            amberRedRating.Tags.Should().HaveCount(2);
            
            amberRedRating.Tags[0].Text.Should().Be("Amber");
            amberRedRating.Tags[0].CssClass.Should().Be("govuk-tag--amber");
            
            amberRedRating.Tags[1].Text.Should().Be("Red");
            amberRedRating.Tags[1].CssClass.Should().Be("govuk-tag--red");
        }

        [Fact]
        public void Build_ShouldInclude_RedRating()
        {
            // Act
            var result = RiskRatingLabelBuilder.Build();

            // Assert
            result.Should().Contain(r => r.RiskRating == ProjectRiskRating.Red);
            
            var redRating = result.First(r => r.RiskRating == ProjectRiskRating.Red);
            redRating.Label.Should().Contain("Red");
            redRating.Tags.Should().NotBeNull();
            redRating.Tags.Should().HaveCount(1);
            redRating.Tags[0].Text.Should().Be("Red");
            redRating.Tags[0].CssClass.Should().Be("govuk-tag--red");
        }

        [Fact]
        public void Build_ShouldReturn_OrderedRatings()
        {
            // Act
            var result = RiskRatingLabelBuilder.Build();

            // Assert
            var ratings = result.Select(r => r.RiskRating).ToList();
            ratings.Should().BeInAscendingOrder();
        }

        [Fact]
        public void Build_ShouldHave_TagsForAllRatings()
        {
            // Act
            var result = RiskRatingLabelBuilder.Build();

            // Assert
            result.Should().OnlyContain(r => r.Tags != null && r.Tags.Count > 0, 
                "all risk ratings should have structured tags");
        }

        [Fact]
        public void Build_ShouldHave_BackwardCompatibilityLabelProperty()
        {
            // Act
            var result = RiskRatingLabelBuilder.Build();

            // Assert
            result.Should().OnlyContain(r => !string.IsNullOrEmpty(r.Label),
                "Label property should be populated for backward compatibility");
        }

        [Theory]
        [InlineData(ProjectRiskRating.Green)]
        [InlineData(ProjectRiskRating.AmberGreen)]
        [InlineData(ProjectRiskRating.AmberRed)]
        [InlineData(ProjectRiskRating.Red)]
        public void Build_ShouldMatch_LabelAndTagsContent(ProjectRiskRating rating)
        {
            // Act
            var result = RiskRatingLabelBuilder.Build();
            var ratingData = result.First(r => r.RiskRating == rating);

            // Assert
            // Tags should match the content of the Label property
            foreach (var tag in ratingData.Tags)
            {
                ratingData.Label.Should().Contain(tag.Text, 
                    $"Label should contain tag text '{tag.Text}' for {rating}");
            }
        }

        [Fact]
        public void Build_ShouldHave_SafeCssClasses()
        {
            // Act
            var result = RiskRatingLabelBuilder.Build();

            // Assert
            var allTags = result.SelectMany(r => r.Tags);
            allTags.Should().OnlyContain(tag => 
                tag.CssClass.StartsWith("govuk-tag--"),
                "all CSS classes should be safe govuk-tag classes");
        }

        [Fact]
        public void Build_ShouldBe_Idempotent()
        {
            // Act
            var result1 = RiskRatingLabelBuilder.Build();
            var result2 = RiskRatingLabelBuilder.Build();

            // Assert
            result1.Should().BeEquivalentTo(result2, 
                "Build() should return the same results on subsequent calls");
        }
    }
}

