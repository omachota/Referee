using System;
using System.Globalization;
using System.Windows.Controls;

namespace Referee.Infrastructure.Validation
{
    public class DateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return DateTime.TryParse((value ?? "").ToString(), CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces, out _) ? ValidationResult.ValidResult
                : new ValidationResult(false, "Neplatné datum!");
        }
    }
}
