using DTDL_scratch;
using Microsoft.Azure.DigitalTwins.Parser;
using Microsoft.Azure.DigitalTwins.Parser.Models;

string readFileFromDtmi()
{
    var basePath = System.Reflection.Assembly.GetExecutingAssembly().Location + @"./../../../../";
    return File.ReadAllText(Path.Join(basePath, "dtmi/samples/aninterface-1.json"));
}


var parser = new ModelParser();
var parserResult = await parser.ParseAsync(new string[] { readFileFromDtmi() });

var root = parserResult.Where(i => i.Value.ChildOf == null).FirstOrDefault();
Console.WriteLine(root.Key);
Console.WriteLine();

foreach (var item in ((DTInterfaceInfo)parserResult[root.Key]).Contents)
{
    Console.WriteLine(item.Value.EntityKind);
    Console.WriteLine(item.Value.Name);
    var pi = (DTPropertyInfo)item.Value;
    Console.WriteLine(pi.Schema.Id);

    foreach (var st in pi.SupplementalTypes)
    {
        Console.WriteLine("st");
        Console.WriteLine(st);
    }

}

//var model = await new DtdlModelParser().ParseAsync("dtmi/samples/anextendedinterface-1.json");

//Console.WriteLine(model.Id);

//foreach  (var t in model.Telemetries)
//{
//    Console.WriteLine($" [T] {t.Name} {t.DataType}");
//}

//foreach (var p in model.Properties)
//{
//    Console.WriteLine($" [P] {p.Name} {p.DataType}");
//}
