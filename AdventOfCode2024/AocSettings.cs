using System.ComponentModel;
using Spectre.Console.Cli;

namespace AdventOfCode2024;

public class AocSettings : CommandSettings
{
    [Description("Whether to use sample data or not")]
    [CommandArgument(0, "<use-sample-data>")]
    public bool UseSampleData { get; set; }
}