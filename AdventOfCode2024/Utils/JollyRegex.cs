using System.Text.RegularExpressions;

namespace AdventOfCode2024.Utils;

internal static partial class JollyRegex
{
    [GeneratedRegex(@"mul\(\d+,\d+\)", RegexOptions.Multiline)]
    public static partial Regex MultiplyOperation();
    
    [GeneratedRegex(@"\d+", RegexOptions.Multiline)]
    public static partial Regex ExtractNumbers();
    
    [GeneratedRegex(@"don\'t\(\)|mul\(\d+,\d+\)|do\(\)", RegexOptions.Multiline)]
    public static partial Regex Day3Part2Operations();
    
    [GeneratedRegex(@"do\(\)", RegexOptions.Singleline)]
    public static partial Regex DoOperation();
    
    [GeneratedRegex(@"don\'t\(\)", RegexOptions.Singleline)]
    public static partial Regex DontOperation();
}