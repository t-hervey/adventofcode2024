using Spectre.Console;
using Spectre.Console.Cli;

namespace AdventOfCode2024;

internal sealed class Day05 : AsyncCommand
{
    private bool _useSampleData = true;

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var data = !_useSampleData
            ? await File.ReadAllLinesAsync($"./DataFiles/{nameof(Day05)}.txt")
            : await File.ReadAllLinesAsync($"./DataFiles/Sample.txt");
        
        var rules = new List<Rule>();
        var lineNumber = 0;
        var line = data[lineNumber];
        
        var revisions = new List<List<int>>();
        while(line != string.Empty)
        {
            rules.Add(new Rule(){ StartPage = int.Parse(line.Split("|")[0]), EndPage = int.Parse(line.Split("|")[1])});
            lineNumber++;
            line = data[lineNumber];
        }
        while (lineNumber < data.Length - 1)
        {
            lineNumber++;
            
            revisions.Add([..data[lineNumber].Split(',').Select(int.Parse)]);
        }

        var centerPages = new List<int>();
        foreach (var revision in revisions)
        {
            var rulesAdheredTo = rules.TrueForAll(rule => RevisionMeetsRule(rule, revision));
            if(rulesAdheredTo)
            {
                centerPages.Add(revision[CenterPage(revision.Count) - 1]);
            }
        }

        AnsiConsole.MarkupLineInterpolated($"[bold red]Part 1:[/] {centerPages.Sum()}");
        return 0;
    }

    int CenterPage(int pages) => (int) Math.Round(pages / 2.0, MidpointRounding.AwayFromZero) ;
    

    private bool RevisionMeetsRule(Rule rule, List<int> revisionPages)
    {
        return !revisionPages.Contains(rule.StartPage) || 
               !revisionPages.Contains(rule.EndPage) || 
               (revisionPages.Contains(rule.StartPage) && revisionPages.Contains(rule.EndPage) && revisionPages.IndexOf(rule.StartPage) < revisionPages.IndexOf(rule.EndPage));
    }

    private class Rule
    {
        public int StartPage { get; set; }
        public int EndPage { get; set; }
    }
}