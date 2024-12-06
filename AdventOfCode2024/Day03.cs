using System.Text.RegularExpressions;
using AdventOfCode2024.DataFiles.Utils;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AdventOfCode2024;

internal sealed class Day03 : AsyncCommand
{
    enum Operation
    {
        Do, Dont, Multiply
    }
    
    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var data = await File.ReadAllTextAsync("./DataFiles/Day03.txt");

        var matches = JollyRegex.MultiplyOperation().Matches(data);
        
        var part1Answer = matches.Select(x => GetValue(x.Value)).Sum();
        
        AnsiConsole.MarkupLineInterpolated($"[bold red]Part 1:[/] {part1Answer}");
        
        matches = JollyRegex.Day3Part2Operations().Matches(data);

        var enabled = true;
        var part2Answer = 0;
        foreach (Match operation in matches)
        {
            if (operation.Value == "do()")
            {
                enabled = true;
            }
            else if(operation.Value == "don't()")
            {
                enabled = false;
            }
            else if(enabled && operation.Value.StartsWith("mul"))
            {
                part2Answer += GetValue(operation.Value);
            }
        }
        AnsiConsole.MarkupLineInterpolated($"[bold Green]Part 2:[/] {part2Answer}");
        return 0;
    }

    private int GetValue(string command)
    {
        return JollyRegex.ExtractNumbers().Matches(command).Select(x => int.Parse(x.Value)).Aggregate((x, y) => x * y);
    }
}