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
    }
}
