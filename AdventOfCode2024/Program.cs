// See https://aka.ms/new-console-template for more information

using AdventOfCode2024;
using Spectre.Console.Cli;

var type = typeof(Day01);

var appType = typeof(CommandApp<>).MakeGenericType(type);
var app = (ICommandApp)Activator.CreateInstance(appType) ?? throw new InvalidOperationException("Failed to create CommandApp");

return app.Run(args);