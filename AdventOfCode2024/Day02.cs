using Spectre.Console;
using Spectre.Console.Cli;

namespace AdventOfCode2024;

internal sealed class Day02 : AsyncCommand
{
    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var data = await File.ReadAllLinesAsync($"./DataFiles/{nameof(Day02)}.txt");

        var safeReports = 0;
        var line = 1;
        var problemReports = new List<string>();
        foreach (var report in data)
        {
            AnsiConsole.MarkupInterpolated($"{line}: [Gray] {report}[/]");
            var reportValues = report.Split(" ").ToList();
            
            if (IsReportSafe(reportValues))
            {
                AnsiConsole.MarkupLine($" [bold Green] Passed[/]");
                safeReports++;
            }
            else
            {
                problemReports.Add(report);
                AnsiConsole.MarkupLine($" [Yellow] Problem Detected[/]");
                //AnsiConsole.MarkupLine($" [bold Red] Failed[/]");
            }
            line++;
        }
        
        AnsiConsole.MarkupLineInterpolated($"[bold red]Part 1:[/] {safeReports}");

        foreach (var report in problemReports)
        {
            var reportValues = report.Split(" ").ToList();
            for (int i = 0; i < reportValues.Count; i++)
            {
                var temp = new string[reportValues.Count];
                reportValues.CopyTo(0, temp,0, reportValues.Count);
                var list = temp.ToList();
                list.RemoveAt(i);
                if (IsReportSafe(list))
                {
                    AnsiConsole.MarkupLineInterpolated($"{report} made safe by removing report {i+1}");
                    safeReports++;
                    break;
                }
            }
        }
        
        AnsiConsole.MarkupLineInterpolated($"[bold Green]Part 2:[/] {safeReports}");
        
        return 0;
    }

    private bool IsReportSafe(List<string> reportValues)
    {
        bool isIncreasing;
        bool? previousValue = null;
        bool isSafe = true;
        for (int i = 1; i < reportValues.Count; i++)
        {
            var reportValueA = int.Parse(reportValues[i]);
            var reportValueB = int.Parse(reportValues[i-1]);
             
            isIncreasing = reportValueA < reportValueB;
                
            var diff = Math.Abs(reportValueA - reportValueB);
            if (diff > 3 || (previousValue ?? isIncreasing) != isIncreasing || reportValueA == reportValueB)
                isSafe = false;
            previousValue = isIncreasing;
        }
        return isSafe;
    }
}