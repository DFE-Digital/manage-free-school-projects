namespace Dfe.ManageFreeSchoolProjects.API.UseCases.BulkEdit.Validations
{
    public class DateValidationCommand : IValidationCommand<BulkEditDto>
    {
        public ValidationResult Execute(ValidationCommandParameters<BulkEditDto> parameters)
        {
            var dateParts = CleanAndSplitDate(parameters.Value);

            if (!IsValidDateFormat(dateParts))
                return CreateValidationResult(false, "Enter a valid date. For example, 27/03/2021");

            var (day, month, year) = (dateParts[0], dateParts[1], dateParts[2]);

            var missingPartsMessage = CheckForMissingDateParts(day, month, year);
            if (missingPartsMessage != null)
                return CreateValidationResult(false, missingPartsMessage);

            if (!IsValidMonth(month, out var monthNumber))
                return CreateValidationResult(false, "Month must be between 1 and 12");

            if (!IsValidYear(year, out var yearNumber))
                return CreateValidationResult(false, "Year must be between 2000 and 2050");

            if (!int.TryParse(day, out _))
                return CreateValidationResult(false, "Day must be a number, like 12");
            
            if (!IsValidDay(day, yearNumber, monthNumber, out _))
                return CreateValidationResult(false,
                    $"Day must be between 1 and {DateTime.DaysInMonth(yearNumber, monthNumber)}");

            return CreateValidationResult(true, null);
        }

        private static string[] CleanAndSplitDate(string date)
        {
            if (!date.Contains('/'))
                return [date];

            var dateParts = date.Split('/');
            if (dateParts[2].EndsWith("00:00:00"))
                dateParts[2] = dateParts[2][..^" 00:00:00".Length];
            
            return dateParts;
        }

        private static bool IsValidDateFormat(string[] dateParts) => dateParts.Length == 3;

        private static string CheckForMissingDateParts(string day, string month, string year)
        {
            var missingParts = new List<string>();
            if (string.IsNullOrEmpty(day)) missingParts.Add("day");
            if (string.IsNullOrEmpty(month)) missingParts.Add("month");
            if (string.IsNullOrEmpty(year)) missingParts.Add("year");

            if (missingParts.Count == 3)
                return "Date must include a day, month, and year"; 
            if (missingParts.Count > 0)
                return $"Date must include a {string.Join(" and ", missingParts)}";
            return null;
        }

        private static bool IsValidMonth(string month, out int monthNumber) =>
            int.TryParse(month, out monthNumber) && monthNumber is >= 1 and <= 12;

        private static bool IsValidYear(string year, out int yearNumber) =>
            int.TryParse(year, out yearNumber) && yearNumber is >= 2000 and <= 2050;

        private static bool IsValidDay(string day, int year, int month, out int dayNumber)
        {
            var isValid = int.TryParse(day, out dayNumber) && dayNumber >= 1 &&
                          dayNumber <= DateTime.DaysInMonth(year, month);
            return isValid;
        }

        private static ValidationResult CreateValidationResult(bool isValid, string message) =>
            new() { IsValid = isValid, ErrorMessage = message };
    }
}