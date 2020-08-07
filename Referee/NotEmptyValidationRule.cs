using System.Globalization;
using System.Windows.Controls;

namespace Referee
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Pole musí být vyplněno.")
                : ValidationResult.ValidResult;
        }
    }
}
