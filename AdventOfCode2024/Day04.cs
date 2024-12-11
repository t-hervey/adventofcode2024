using Spectre.Console;
using Spectre.Console.Cli;

namespace AdventOfCode2024;

internal sealed class Day04 : AsyncCommand
{
    private bool _useSampleData = false;
    private Dictionary<string, Node> _graph = new Dictionary<string, Node>();
    private int _maxWidth = 0;
    private int _maxHeight = 0;

    private Dictionary<int, char> _depthLookupDictionary = new Dictionary<int, char>()
    {
        {0, 'X'},
        {1, 'M'},
        {2, 'A'},
        {3, 'S'},
    };
    
    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        var data = !_useSampleData
            ? await File.ReadAllLinesAsync($"./DataFiles/{nameof(Day04)}.txt")
            : await File.ReadAllLinesAsync($"./DataFiles/Sample.txt");

        _maxWidth = data[0].Length - 1;
        _maxHeight = data.Length - 1;
        for(var y = 0; y < data.Length; y++)
        {
            for(var x = 0; x < data[y].Length; x++)
            {
                _graph.Add(
                    $"{x},{y}", 
                    new Node
                    {
                        X = x, 
                        Y = y, 
                        Value = data[y][x],
                        //Neighbors = AdjacentNodes(x, y)
                    });
            }
        }
        
        foreach(var node in _graph.Values)
        {
            node.Neighbors = AdjacentNodes(node.X, node.Y);
        }

        var counter = 0;
        foreach (var node in _graph.Values)
        {
            string path = "";
            DepthLimitedDFSVisit(node, 0, 4, path, ref counter, 0);
           
        }
        AnsiConsole.MarkupLineInterpolated($"[bold red]Part 1:[/] {counter}");
        
        return 0;
    }

    private List<Node> AdjacentNodes(int x, int y)
    {
        var nodes = new List<Node>();
        AnsiConsole.WriteLine($"Getting neighbors for {x},{y}");
        // Top left corner
        if (x == 0 && y == 0)
        {
            nodes.Add(_graph[$"{x + 1},{y}"]);      // right
            nodes.Add(_graph[$"{x},{y + 1}"]);      // bottom
            nodes.Add(_graph[$"{x + 1},{y + 1}"]);  // bottom right
        }
        // Top right corner
        else if (x == _maxWidth && y == 0)
        {
            nodes.Add(_graph[$"{x - 1},{y}"]);      // left
            nodes.Add(_graph[$"{x},{y + 1}"]);      // bottom
            nodes.Add(_graph[$"{x - 1},{y + 1}"]);  // bottom left
        }
        // Bottom left corner
        else if (x == 0 && y == _maxHeight)
        {
            nodes.Add(_graph[$"{x},{y - 1}"]);      // top
            nodes.Add(_graph[$"{x + 1},{y - 1}"]);  // top right
            nodes.Add(_graph[$"{x + 1},{y}"]);      // right
        }
        // Bottom right corner
        else if (x == _maxWidth && y == _maxHeight)
        {
            nodes.Add(_graph[$"{x - 1},{y - 1}"]);  // top left
            nodes.Add(_graph[$"{x},{y - 1}"]);      // top
            nodes.Add(_graph[$"{x - 1},{y}"]);      // left
        }
        // Inside the grid
        else if (x > 0 && y > 0 && x < _maxWidth && y < _maxHeight)
        {
            nodes.Add(_graph[$"{x - 1},{y - 1}"]);  // top left
            nodes.Add(_graph[$"{x},{y - 1}"]);      // top
            nodes.Add(_graph[$"{x + 1},{y - 1}"]);  // top right
            nodes.Add(_graph[$"{x - 1},{y}"]);      // left
            nodes.Add(_graph[$"{x + 1},{y}"]);      // right
            nodes.Add(_graph[$"{x - 1},{y + 1}"]);  // bottom left
            nodes.Add(_graph[$"{x},{y + 1}"]);      // bottom
            nodes.Add(_graph[$"{x + 1},{y + 1}"]);  // bottom right
        }
        // First column, inside Y bounds
        else if (x == 0 && y > 0 && y < _maxHeight)
        {
            nodes.Add(_graph[$"{x},{y - 1}"]);      // top
            nodes.Add(_graph[$"{x + 1},{y - 1}"]);  // top right
            nodes.Add(_graph[$"{x + 1},{y}"]);      // right
            nodes.Add(_graph[$"{x},{y + 1}"]);      // bottom
            nodes.Add(_graph[$"{x + 1},{y + 1}"]);  // bottom right
        }
        // Inside the X bounds, first row
        else if (x > 0 && x < _maxWidth && y == 0 )
        {
            nodes.Add(_graph[$"{x - 1},{y}"]);      // left
            nodes.Add(_graph[$"{x + 1},{y}"]);      // right
            nodes.Add(_graph[$"{x - 1},{y + 1}"]);  // bottom left
            nodes.Add(_graph[$"{x},{y + 1}"]);      // bottom
            nodes.Add(_graph[$"{x + 1},{y + 1}"]);  // bottom right
        }
        // Last column, inside Y bounds
        else if (x == _maxWidth && y > 0 && y < _maxHeight)
        {
            nodes.Add(_graph[$"{x - 1},{y - 1}"]);  // top left
            nodes.Add(_graph[$"{x},{y - 1}"]);      // top
            nodes.Add(_graph[$"{x - 1},{y}"]);      // left
            nodes.Add(_graph[$"{x - 1},{y + 1}"]);  // bottom left
            nodes.Add(_graph[$"{x},{y + 1}"]);      // bottom
        }
        // Inside the X bounds, last row
        else if (x > 0 && x < _maxWidth && y == _maxHeight)
        {
            nodes.Add(_graph[$"{x - 1},{y - 1}"]);  // top left
            nodes.Add(_graph[$"{x},{y - 1}"]);      // top
            nodes.Add(_graph[$"{x + 1},{y - 1}"]);  // top right
            nodes.Add(_graph[$"{x - 1},{y}"]);      // left
            nodes.Add(_graph[$"{x + 1},{y}"]);      // right
        }
        else
        {
            throw new NotImplementedException($"No condition for this case: {x},{y}");
        }

        return nodes;
    }

    
    private void DepthLimitedDFSVisit(Node node, int depth, int maxDepth, string path, ref int count, double direction)
    {
        if (depth >= maxDepth || _depthLookupDictionary[depth] != node.Value)
            return;

        AnsiConsole.WriteLine($"Found {node.Value} at depth {depth}: ({node.X},{node.Y})");

        if (!_graph.ContainsKey($"{node.X},{node.Y}"))
            return;

        path += node.Value;
        if (path == "XMAS")
        {
            count++;
            AnsiConsole.MarkupLine($"[bold green] Found one!:[/] {count} Direction: {direction}");
        }
        foreach (var neighbor in _graph[$"{node.X},{node.Y}"].Neighbors)
        {
            var nextDirection = CalculateDirection(node.X, node.Y, neighbor.X, neighbor.Y);
            if(depth > 0 && direction != nextDirection)
                continue;
            DepthLimitedDFSVisit(neighbor, depth + 1, maxDepth, path, ref count, nextDirection);
        }
    }
    
    private static double CalculateDirection(double x1, double y1, double x2, double y2)
    {
        // Calculate differences
        double deltaX = x2 - x1;
        double deltaY = y2 - y1;

        // Calculate angle in radians
        double angleRadians = Math.Atan2(deltaY, deltaX);

        // Convert radians to degrees
        double angleDegrees = angleRadians * (180.0 / Math.PI);

        // Normalize the angle to be between 0 and 360 degrees
        if (angleDegrees < 0)
        {
            angleDegrees += 360;
        }

        return angleDegrees;
    }
}

internal sealed class Node
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Value { get; set; }
    
    public List<Node> Neighbors { get; set; } = [];
}
