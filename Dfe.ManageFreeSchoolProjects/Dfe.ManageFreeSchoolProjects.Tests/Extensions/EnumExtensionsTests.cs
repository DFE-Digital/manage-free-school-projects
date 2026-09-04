using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Extensions;
using FluentAssertions;

namespace Dfe.ManageFreeSchoolProjects.Tests.Extensions
{
    public class EnumExtensionsTests
    {
        [Fact]
        public void ToDescriptionOrEmpty_WhenDescribed_ReturnsDescription()
        {
            FaithStatus.Designation.ToDescriptionOrEmpty()
                .Should().Be("This is also known as character.");
        }

        [Theory]
        [InlineData(FaithStatus.Ethos)]
        [InlineData(FaithStatus.None)]
        public void ToDescriptionOrEmpty_WhenNotDescribed_ReturnsEmpty(FaithStatus faithStatus)
        {
            faithStatus.ToDescriptionOrEmpty().Should().BeEmpty();
        }

        /// <summary>
        /// The contrast with ToDescription is the point of the separate helper - falling back to
        /// the member name would render a hint reading "Ethos" underneath the Ethos radio.
        /// </summary>
        [Fact]
        public void ToDescription_WhenNotDescribed_FallsBackToMemberName()
        {
            FaithStatus.Ethos.ToDescription().Should().Be("Ethos");
        }

        /// <summary>
        /// Both helpers are called on nullable enums straight out of the project cache, so the null
        /// guard has to survive - without it the reflection below throws.
        /// </summary>
        [Fact]
        public void ToDescription_WhenNullableEnumHasNoValue_ReturnsEmpty()
        {
            FaithStatus? faithStatus = null;

            faithStatus.ToDescription().Should().BeEmpty();
        }

        [Fact]
        public void ToDescriptionOrEmpty_WhenNullableEnumHasNoValue_ReturnsEmpty()
        {
            FaithStatus? faithStatus = null;

            faithStatus.ToDescriptionOrEmpty().Should().BeEmpty();
        }

        [Fact]
        public void ToDescription_WhenNullableEnumHasValue_ReturnsDescription()
        {
            FaithStatus? faithStatus = FaithStatus.Designation;

            faithStatus.ToDescription().Should().Be("This is also known as character.");
        }

        /// <summary>
        /// Regions arrive as a number in the URL, so a value outside the enum can reach here. There
        /// is no member to read an attribute from, and it must not throw.
        /// </summary>
        [Fact]
        public void ToDescription_WhenTheValueIsNotAMemberOfTheEnum_FallsBackToTheNumber()
        {
            var region = (ProjectRegion)0;

            region.ToDescription().Should().Be("0");
        }

        [Fact]
        public void ToDescriptionOrEmpty_WhenTheValueIsNotAMemberOfTheEnum_ReturnsEmpty()
        {
            var region = (ProjectRegion)99;

            region.ToDescriptionOrEmpty().Should().BeEmpty();
        }

        [Theory]
        [InlineData("North West", ProjectRegion.NorthWest)]
        [InlineData("Yorkshire and the Humber", ProjectRegion.YorkshireAndHumber)]
        [InlineData("London", ProjectRegion.London)]
        public void FromDescription_ReturnsTheMemberWithThatDescription(string description, ProjectRegion expected)
        {
            description.FromDescription<ProjectRegion>().Should().Be(expected);
        }

        [Fact]
        public void FromDescription_RoundTripsEveryRegion()
        {
            foreach (var region in Enum.GetValues<ProjectRegion>())
            {
                region.ToDescription().FromDescription<ProjectRegion>().Should().Be(region);
            }
        }

        [Fact]
        public void FromDescription_WhenNothingMatches_Throws()
        {
            var act = () => "Narnia".FromDescription<ProjectRegion>();

            act.Should().Throw<ArgumentException>().WithMessage("*ProjectRegion*Narnia*");
        }
    }
}
