using Dfe.ManageFreeSchoolProjects.API.UseCases.BulkEdit;
using Dfe.ManageFreeSchoolProjects.API.UseCases.BulkEdit.Validations;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.UseCases.BulkEdit.Validation
{
    public class DateValidationCommandTests
    {
        [Theory]
        [InlineData("g", "Enter a valid date. For example, 27/03/2021")]
        [InlineData("12", "Enter a valid date. For example, 27/03/2021")]
        [InlineData("12-13-2025", "Enter a valid date. For example, 27/03/2021")] 
        [InlineData("12/10/2051", "Year must be between 2000 and 2050")]         
        [InlineData("12/10/1999", "Year must be between 2000 and 2050")]         
        [InlineData("31/04/2024", "Day must be between 1 and 30")] 
        [InlineData("29/02/2023", "Day must be between 1 and 28")] 
        [InlineData("00/10/2025", "Day must be between 1 and 31")]  
        [InlineData("12/00/2025", "Month must be between 1 and 12")]
        [InlineData("12/13/2025", "Month must be between 1 and 12")]
        [InlineData("hj/12/2050", "Day must be a number, like 12")]
        public void DateValidationFails(string date, string expectedErrorMessage)
        {
            var dateValidation = new DateValidationCommand();
            var validationResult = dateValidation.Execute(new ValidationCommandParameters<BulkEditDto> { Data = null, Value = date });

            validationResult.IsValid.Should().BeFalse();
            validationResult.ErrorMessage.Should().Be(expectedErrorMessage);
        }

        [Theory]
        [InlineData("12/10/2025")]  
        [InlineData("29/02/2024")]  // Leap year test
        [InlineData("31/12/2050")]  
        [InlineData("01/01/2000")]
        public void DateValidationPasses(string date)
        {
            var dateValidation = new DateValidationCommand();
            var validationResult = dateValidation.Execute(new ValidationCommandParameters<BulkEditDto> { Data = null, Value = date });

            validationResult.IsValid.Should().BeTrue();
            validationResult.ErrorMessage.Should().BeNull();
        }
    }
}
