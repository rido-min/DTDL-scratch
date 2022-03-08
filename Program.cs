using DTDL_scratch;
using Microsoft.Azure.DigitalTwins.Parser;
using Microsoft.Azure.DigitalTwins.Parser.Models;

string ReadFile()
{
    var basePath = System.Reflection.Assembly.GetExecutingAssembly().Location + @"./../../../../";
    return File.ReadAllText(Path.Join(basePath, "dtmi/samples/aninterface-1.json"));
}


var parser = new ModelParser();

var parserResult = await new ModelParser().ParseAsync(new string[] { ReadFile() });
foreach (var item in parserResult.Values.Where(i => i.EntityKind == DTEntityKind.Telemetry))
{
    var telemetry = (DTTelemetryInfo)item;
    Console.WriteLine($" [T]: {telemetry.Name} {telemetry.Schema.Id}");
}



var root = parserResult.Where(i => i.Value.ChildOf == null).FirstOrDefault();
Console.WriteLine(root.Key);
Console.WriteLine();

foreach (var item in ((DTInterfaceInfo)parserResult[root.Key]).Contents)
{
    Console.WriteLine(item.Value.EntityKind);
    Console.WriteLine(item.Value.Name);
    var pi = (DTPropertyInfo)item.Value;
    Console.WriteLine(pi.Schema.Id);

    Console.WriteLine();
    foreach (var sp in pi.SupplementalProperties)
    {
        Console.WriteLine("sp");
        Console.WriteLine(sp.Key);
        Console.WriteLine(sp.Value.ToString());
    }

    
    foreach (var st in pi.SupplementalTypes)
    {
        Console.WriteLine("st");
        Console.WriteLine(st);
    }

    Console.WriteLine();
    var currentUnit = (DTUnitInfo)pi.SupplementalProperties["dtmi:dtdl:property:unit;2"];
    Console.WriteLine(currentUnit.Id);

}

//var model = await new DtdlModelParser().ParseAsync("dtmi/samples/anextendedinterface-1.json");
var model = await new DtdlModelParser().ParseAsync(ReadFile());
foreach (var t in model.Telemetries)
{
    Console.WriteLine($" [T] {t.Name} {t.DataType}");
}

foreach (var p in model.Properties)
{
    Console.WriteLine($" [P] {p.Name} {p.DataType}");
}

Console.WriteLine(model.Id);