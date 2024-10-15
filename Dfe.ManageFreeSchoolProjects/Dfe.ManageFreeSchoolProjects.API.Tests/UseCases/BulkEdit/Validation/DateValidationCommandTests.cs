﻿using Dfe.ManageFreeSchoolProjects.API.UseCases.BulkEdit.Validations;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.UseCases.BulkEdit.Validation
{
    public class DateValidationCommandTests
    {
        [Theory]
        [InlineData("g", "Enter a valid date. For example, 27/03/2021")]
        [InlineData("12", "Enter a valid date. For example, 27/03/2021")]
        [InlineData("12-13-2025", "Enter a valid date. For example, 27/03/2021")]  // Invalid format (should be dd/MM/yyyy)
        [InlineData("12/10/2051", "Year must be between 2000 and 2050")]          // Year out of range
        [InlineData("12/10/1999", "Year must be between 2000 and 2050")]          // Year out of range
        [InlineData("31/04/2024", "Day must be between 1 and 30 for the given month.")] // Invalid day for April
        [InlineData("29/02/2023", "Day must be between 1 and 28 for the given month.")] // Not a leap year
        [InlineData("00/10/2025", "Day must be a valid number between 1 and 31")]  // Day is zero
        [InlineData("12/00/2025", "Month must be a valid number between 1 and 12")]// Month is zero
        [InlineData("12/13/2025", "Month must be a valid number between 1 and 12")]// Month out of range
        public void DateValidationFails(string date, string error)
        {
            var dateValidation = new DateValidationCommand();
            var validationResult = dateValidation.Execute(new() { Data = null, Value = date });

            validationResult.IsValid.Should().BeFalse();
            validationResult.ErrorMessage.Should().Be(error);
        }

        [Theory]
        [InlineData("12/10/2025")]  // Valid date
        [InlineData("29/02/2024")]  // Leap year valid date
        [InlineData("31/12/2050")]  // Last valid day in the range
        [InlineData("01/01/2000")]  // First valid day in the range
        public void DateValidationPasses(string date)
        {
            var dateValidation = new DateValidationCommand();
            var validationResult = dateValidation.Execute(new() { Data = null, Value = date });

            validationResult.IsValid.Should().BeTrue();
            validationResult.ErrorMessage.Should().BeNull();
        }
    }
}
