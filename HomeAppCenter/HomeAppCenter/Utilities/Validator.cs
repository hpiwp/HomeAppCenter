using System.Text.RegularExpressions;

namespace HomeAppCenter.Utilities;

public static class Validator
{
    private static readonly Regex SerialPattern = new(@"^[A-Z]{3}-\d{4}$", RegexOptions.Compiled);

    public static bool IsValidSerial(string? serial) =>
        !string.IsNullOrWhiteSpace(serial) && SerialPattern.IsMatch(serial);

    public static bool IsValidName(string? text) =>
        !string.IsNullOrWhiteSpace(text) && text.Trim().Length >= 2;

    public static bool IsNonNegative(decimal value) => value >= 0;

    public static bool IsValidDueDate(DateTime received, DateTime due) => due.Date >= received.Date;
}
