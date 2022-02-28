﻿using Azure.IoT.ModelsRepository;
using DTDL_scratch;
using Microsoft.Azure.DigitalTwins.Parser;
using Microsoft.Azure.DigitalTwins.Parser.Models;

var basePath = System.Reflection.Assembly.GetExecutingAssembly().Location +  @"./../../../../";
var dtdl = File.ReadAllText(Path.Join(basePath, "dtmi/samples/anextendedinterface-1.json"));


var dmr = new ModelsRepositoryClient(new Uri(basePath));
var parser = new ModelParser() { DtmiResolverAsync = dmr.ParserDtmiResolver };

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