using Spectre.Console;
using Spectre.Console.Cli;

namespace AdventOfCode2024;

internal sealed class Day01 : AsyncCommand
{
    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var data = await File.ReadAllLinesAsync("./DataFiles/Day01.txt");
        var locationListA = new List<int>();
        var locationListB = new List<int>();

        foreach (var line in data)
        {
            var numberPairs = line.Split("   ");
            locationListA.Add(int.Parse(numberPairs[0]));
            locationListB.Add(int.Parse(numberPairs[1]));
        }

        locationListA.Sort();
        locationListB.Sort();

        var listDiff = new List<int>();

        for (int i = 0; i < locationListA.Count; i++)
        {
            listDiff.Add(int.Abs(locationListA[i] - locationListB[i]));
        }

        var part1Answer = listDiff.Sum();
        
        AnsiConsole.MarkupLineInterpolated($"[bold red]Part 1:[/] {part1Answer}");

        var similarityScores = (
            from location in locationListA 
            let occurrencesCnt = locationListB.Count(x => x == location) 
            select location * occurrencesCnt
        ).ToList();

        var part2Answer = similarityScores.Sum();
        
        AnsiConsole.MarkupLineInterpolated($"[bold green]Part 2:[/] {part2Answer}");
        
        return 0;
    }
}