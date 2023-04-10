using System.Text.RegularExpressions;
namespace EfCoreSample.Validation;

public sealed partial class ValidationPatterns
{
    [GeneratedRegex(@"^\+375(29|44|33)\d{7}$", RegexOptions.Compiled)]
    public static partial Regex PhonePattern();

    [GeneratedRegex(@"^\P{P}{1,30}@(gmail|mail|yahoo|yandex)\.(ru|by|ua|com)$", RegexOptions.Compiled)]
    public static partial Regex EmailPattern();
}