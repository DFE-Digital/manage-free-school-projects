using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.Project
{
    public class EnumParsersTests
    {
        [Theory]
        [InlineData("Designation", FaithStatus.Designation)]
        [InlineData("Ethos", FaithStatus.Ethos)]
        [InlineData("None", FaithStatus.None)]
        [InlineData("NotSet", FaithStatus.NotSet)]
        [InlineData("InvalidStatus", FaithStatus.NotSet)]
        [InlineData("", FaithStatus.NotSet)]
        [InlineData(null, FaithStatus.NotSet)]
        public void ParseFaithStatus_Returns_ExpectedEnum(string input, FaithStatus expectedResult)
        {
            var result = EnumParsers.ParseFaithStatus(input);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ParseFaithStatus_WhenTheStatusWasSavedAsADescription_Returns_ExpectedEnum()
        {
            var result = EnumParsers.ParseFaithStatus("This is also known as character.");

            Assert.Equal(FaithStatus.Designation, result);
        }
    }
}
