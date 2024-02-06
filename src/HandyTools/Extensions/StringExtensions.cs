namespace HandyTools.Extensions;

public static class StringExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? str)
    {
        return string.IsNullOrEmpty(str);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotEmpty([NotNullWhen(true)] this string? str)
    {
        return string.IsNullOrEmpty(str) == false;
    }
}