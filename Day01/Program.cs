// See https://aka.ms/new-console-template for more information

var data = await File.ReadAllLinesAsync("input.txt");
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

var sum = listDiff.Sum();

Console.WriteLine(sum);