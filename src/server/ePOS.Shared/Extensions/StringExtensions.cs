namespace ePOS.Shared.Extensions;

public static class StringExtensions
{
    public static Guid ToGuid(this string input) => !Guid.TryParse(input, out var result) ? Guid.Empty : result;
    
    public static int ToInt(this string str) => !int.TryParse(str, out var result) ? 0 : result;
    
    public static double ToDouble(this string str) => !double.TryParse(str, out var result) ? 0.0 : result;

    public static long ToLong(this string str) => !long.TryParse(str, out var result) ? 0L : result;
}