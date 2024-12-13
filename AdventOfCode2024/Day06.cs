using System.Drawing;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AdventOfCode2024;

internal sealed class Day06 : AsyncCommand
{
    private bool _useSampleData = false;

    private enum Direction
    {
        Up = 0, Right = 1, Down = 2, Left = 3
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var data = !_useSampleData
            ? await File.ReadAllLinesAsync($"./DataFiles/{nameof(Day06)}.txt")
            : await File.ReadAllLinesAsync($"./DataFiles/Sample.txt");

        var pointDefinitions = new Dictionary<Point, char>();

        for (var row = 0; row < data.Length; row++)
        {
            for(var col = 0; col < data[row].Length; col++)
            {
                pointDefinitions.Add(new Point(col, row), data[row][col]);
            }
        }

        try
        {
            var currentPostion = pointDefinitions.First(location => location.Value == '^').Key;
            var visitedLocations = new HashSet<Point>();
            var direction = Direction.Up;
            var steps = 0;
            while (IsWithinBounds(currentPostion, data[0].Length - 1, data.Length - 1))
            {
                visitedLocations.Add(currentPostion);
                AnsiConsole.MarkupLineInterpolated($"x: {currentPostion.X + 1}, y: {currentPostion.Y + 1}, direction: {direction.ToString()} steps: {steps}");
                var nextPosition = direction switch
                {
                    Direction.Up => currentPostion with {Y = currentPostion.Y - 1},
                    Direction.Right => currentPostion with {X = currentPostion.X + 1},
                    Direction.Down => currentPostion with {Y = currentPostion.Y + 1},
                    Direction.Left => currentPostion with {X = currentPostion.X - 1},
                    _ => throw new ArgumentOutOfRangeException("direction")
                };

                if (IsWithinBounds(nextPosition, data[0].Length - 1, data.Length - 1) && IsObstacle(nextPosition, pointDefinitions))
                {
                    direction = (Direction) (((int) direction + 1) % 4);
                }
                else
                {
                    currentPostion = nextPosition;
                }
                steps++;
            }

            AnsiConsole.MarkupLineInterpolated($"[bold red]Part 1:[/] {visitedLocations.Count}");
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLineInterpolated($"[bold red] {e.Message}[/]");
        }

        return 0;
    }
    
    bool IsObstacle (Point location, Dictionary<Point, char> pointDefinitions)
    {
        return pointDefinitions[location] == '#';
    }

    bool IsWithinBounds(Point location, int maxWidth, int maxHeight)
    {
        return location.X >= 0 && location.X <= maxWidth && location.Y >= 0 && location.Y <= maxHeight;
    }

}