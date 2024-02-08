using System.Text.RegularExpressions;

namespace ePOS.Shared.Utilities;

public static class RegexUtils
{
    public static readonly Regex EmailRegex = new(@"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$");
}