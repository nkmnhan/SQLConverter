// See https://aka.ms/new-console-template for more information
using SQLConverter;


Console.WriteLine("Input workspace:");
var workSpace = Console.ReadLine();
if(string.IsNullOrEmpty(workSpace))
{
    workSpace = @"C:\Users\nhan.nguyen\Desktop";
}

var allFiles = Directory.GetFiles(workSpace).Where(x => x.EndsWith(".data"));

foreach (var file in allFiles)
{
    var data = await File.ReadAllLinesAsync(file);
    var content = TxTToSQL.Convert(data);
    var newPath = file.Replace(".data", ".sql");
    await File.WriteAllLinesAsync(newPath, content);
    Console.WriteLine($"CreateNewFile: {newPath}");
}
Console.WriteLine("--Done!--");

