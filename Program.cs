using DTDL_scratch;
using Microsoft.Azure.DigitalTwins.Parser;

string ReadFile(string path) => File.ReadAllText(Path.Join(System.Reflection.Assembly.GetExecutingAssembly().Location + @"./../../../../", path));

var model = await new ModelParser().ParseAsync(ReadFile("dtmi/samples/aninterface-1.json"));

Console.WriteLine(model.Id);

foreach (var t in model.Telemetries)
{
    Console.WriteLine($" [T] {t.Name} {t.Schema.Id}");
}

foreach (var p in model.Properties)
{
    Console.WriteLine($" [P] {p.Name} {p.Schema.Id}");
}

foreach (var c in model.Commands)
{
    Console.WriteLine($" [C] {c.Name} {c.Request.Id} {c.Response.Id}");
}




//var parser = new ModelParser();

//var parserResult = await new ModelParser().ParseAsync(new string[] { ReadFile() });
//foreach (var item in parserResult.Values.Where(i => i.EntityKind == DTEntityKind.Telemetry))
//{
//    var telemetry = (DTTelemetryInfo)item;
//    Console.WriteLine($" [T]: {telemetry.Name} {telemetry.Schema.Id}");
//}



//var root = parserResult.Where(i => i.Value.ChildOf == null).FirstOrDefault();
//Console.WriteLine(root.Key);
//Console.WriteLine();

//foreach (var item in ((DTInterfaceInfo)parserResult[root.Key]).Contents)
//{
//    Console.WriteLine(item.Value.EntityKind);
//    Console.WriteLine(item.Value.Name);
//    var pi = (DTPropertyInfo)item.Value;
//    Console.WriteLine(pi.Schema.Id);

//    Console.WriteLine();
//    foreach (var sp in pi.SupplementalProperties)
//    {
//        Console.WriteLine("sp");
//        Console.WriteLine(sp.Key);
//        Console.WriteLine(sp.Value.ToString());
//    }


//    foreach (var st in pi.SupplementalTypes)
//    {
//        Console.WriteLine("st");
//        Console.WriteLine(st);
//    }

//    Console.WriteLine();
//    var currentUnit = (DTUnitInfo)pi.SupplementalProperties["dtmi:dtdl:property:unit;2"];
//    Console.WriteLine(currentUnit.Id);

//}

////var model = await new DtdlModelParser().ParseAsync("dtmi/samples/anextendedinterface-1.json");
