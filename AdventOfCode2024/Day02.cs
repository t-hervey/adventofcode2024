using Spectre.Console;
using Spectre.Console.Cli;

namespace AdventOfCode2024;

internal sealed class Day02 : AsyncCommand
{
    public Day02()
    {
        
    }
    
    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var data = await File.ReadAllLinesAsync($"./DataFiles/{nameof(Day02)}.txt");

        var safeReports = 0;
        var line = 1;
        foreach (var report in data)
        {
            AnsiConsole.MarkupInterpolated($"{line}: [Gray] {report}[/]");
            var reportValues = report.Split(" ");
            bool isIncreasing;
            bool? previousValue = null;
            bool isSafe = true;
            for (int i = 1; i < reportValues.Length; i++)
            {
                var reportValueA = int.Parse(reportValues[i]);
                var reportValueB = int.Parse(reportValues[i-1]);
             
                isIncreasing = reportValueA < reportValueB;
                
                var diff = Math.Abs(reportValueA - reportValueB);
                if (diff > 3 || (previousValue ?? isIncreasing) != isIncreasing || reportValueA == reportValueB)
                    isSafe = false;
                previousValue = isIncreasing;
            }

            if (isSafe)
            {
                AnsiConsole.MarkupLine($" [bold Green] Passed[/]");
                safeReports++;
            }
            else
            {
                AnsiConsole.MarkupLine($" [bold Red] Failed[/]");
            }
            line++;
        }
        
        AnsiConsole.MarkupLineInterpolated($"[bold red]Part 1:[/] {safeReports}");
        
        return 0;
    }
}