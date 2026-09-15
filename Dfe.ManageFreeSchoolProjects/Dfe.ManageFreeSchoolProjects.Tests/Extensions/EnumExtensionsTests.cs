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
    }
}
