using Azure.IoT.ModelsRepository;
using DTDL_scratch;
using Microsoft.Azure.DigitalTwins.Parser;
using Microsoft.Azure.DigitalTwins.Parser.Models;

var basePath = System.Reflection.Assembly.GetExecutingAssembly().Location +  @"./../../../../";
var dtdl = File.ReadAllText(Path.Join(basePath, "dtmi/samples/anextendedInterface-1.expanded.json"));

var parser = new ModelParser() { DtmiResolverAsync = new ModelsRepositoryClient().ParserDtmiResolver };

var parserResult = await parser.ParseAsync(new string[] { dtdl });

foreach (var item in parserResult.Values.Where(v => v.EntityKind == DTEntityKind.Interface))
{
    Console.WriteLine(item.Id);
    foreach  (var telemetry in ((DTInterfaceInfo)item).Contents.Where(c=>c.Value.EntityKind == DTEntityKind.Telemetry))
    {
        Console.Write($" [T] {telemetry.Value.Name}");
        Console.WriteLine(" " + ModelParser.GetTermOrUri(((DTTelemetryInfo)telemetry.Value).Schema.Id));
    }
    foreach (var property in ((DTInterfaceInfo)item).Contents.Where(c => c.Value.EntityKind == DTEntityKind.Property))
    {
        Console.Write($" [P] {property.Value.Name}" );
        Console.WriteLine(" " + ModelParser.GetTermOrUri(((DTPropertyInfo)property.Value).Schema.Id));
    }

    foreach (var component in ((DTInterfaceInfo)item).Contents.Where(c => c.Value.EntityKind == DTEntityKind.Component))
    {
        Console.Write($" [C] {component.Value.Name}");
        Console.WriteLine(" " + ModelParser.GetTermOrUri(((DTComponentInfo)component.Value).Schema.Id));

        var compDef = parserResult.First(r => r.Key.Equals(((DTComponentInfo)component.Value).Schema.Id));
        foreach (var tel in ((DTInterfaceInfo)compDef.Value).Contents.Where(c => c.Value.EntityKind == DTEntityKind.Telemetry))
        {
            Console.Write($"   [T] {tel.Value.Name}");
            Console.WriteLine("   " + ModelParser.GetTermOrUri(((DTTelemetryInfo)tel.Value).Schema.Id));
        }

        foreach (var prop in ((DTInterfaceInfo)compDef.Value).Contents.Where(c => c.Value.EntityKind == DTEntityKind.Property))
        {
            Console.Write($"   [P] {prop.Value.Name}");
            Console.WriteLine("   " + ModelParser.GetTermOrUri(((DTPropertyInfo)prop.Value).Schema.Id));
        }
    }
}