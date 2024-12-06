// See https://aka.ms/new-console-template for more information

using AdventOfCode2024;
using Spectre.Console.Cli;

var type = typeof(Day02);

var appType = typeof(CommandApp<>).MakeGenericType(type);
//var app = (ICommandApp)Activator.CreateInstance(appType) ?? throw new InvalidOperationException("Failed to create CommandApp");

var app = new CommandApp<Day03>();


return app.Run(args);