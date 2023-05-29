// See https://aka.ms/new-console-template for more information
using SQLConverter;


Console.WriteLine("Input workspace:");
var workSpace = Console.ReadLine();
if (string.IsNullOrEmpty(workSpace))
{
    workSpace = @"C:\temp\";
}

var allFiles = Directory.GetFiles(workSpace).Where(x => x.EndsWith(".data"));
var cleanUpContents = new List<string>();
foreach (var file in allFiles)
{
    var data = await File.ReadAllLinesAsync(file);
    var content = TxTToSQL.Convert(data);
    var newPath = file.Replace(".data", ".sql");
    await File.WriteAllLinesAsync(newPath, content);
    Console.WriteLine($"CreateNewFile: {newPath}");

    var cleanupContent = TxTToSQL.Cleanup(data);
    if (!string.IsNullOrEmpty(cleanupContent))
    {
        cleanUpContents.Add(cleanupContent);
    }
}

if (cleanUpContents.Count > 0)
{
    var cleanupPath = $"{workSpace}/cleanup.sql";
    await File.WriteAllLinesAsync(cleanupPath, cleanUpContents);
    Console.WriteLine($"CreateNewFile: {cleanupPath}");
}
Console.WriteLine("--Done!--");

