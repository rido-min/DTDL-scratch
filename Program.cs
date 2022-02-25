using Microsoft.Azure.DigitalTwins.Parser;
using Microsoft.Azure.DigitalTwins.Parser.Models;

var parser = new ModelParser();
var basePath = System.Reflection.Assembly.GetExecutingAssembly().Location +  @"./../../../../";
var dtdl = File.ReadAllText(Path.Join(basePath, "dtmi/samples/aninterface-1.json"));
var parserResult = await parser.ParseAsync(new string[] { dtdl });

foreach (var item in parserResult.Values.Where(v=>v.EntityKind == DTEntityKind.Interface))
{
    Console.WriteLine(item.Id);
    foreach  (var content in ((DTInterfaceInfo)item).Contents.Where(c=>c.Value.EntityKind == DTEntityKind.Telemetry))
    {
        Console.Write(" [T]" + content.Value.Name);
        Console.WriteLine(" " + ModelParser.GetTermOrUri(((DTTelemetryInfo)content.Value).Schema.Id));
    }
    foreach (var content in ((DTInterfaceInfo)item).Contents.Where(c => c.Value.EntityKind == DTEntityKind.Property))
    {
        Console.Write(" [P]" + content.Value.Name);
        Console.WriteLine(" " + ModelParser.GetTermOrUri(((DTPropertyInfo)content.Value).Schema.Id));
    }
}

//var telemetries = parserResult.Where(r => r.Value.EntityKind == DTEntityKind.Telemetry);
//foreach (var tel in telemetries)
//{
//    var 
//}