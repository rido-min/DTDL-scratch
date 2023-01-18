using Azure.IoT.ModelsRepository;
using dtdl_dotnet;
using DTDLParser;
using DTDLParser.Models;

string basePath = Path.Join(System.Reflection.Assembly.GetExecutingAssembly().Location + @"./../../../../../");
string readFile (string path) => File.ReadAllText(Path.Join(basePath, path));

var parser = new ModelParser() 
{ 
    //Options = ModelParsingOption.AllowUndefinedExtensions,
    DtmiResolverAsync = new ModelsRepositoryClient(new Uri(basePath)).ParserDtmiResolverAsync
    //DtmiResolverAsync = DmrClient.DtmiResolverAsync
};
Console.WriteLine($"MaxDtdlVersion: {parser.MaxDtdlVersion}");


//string fileName = "dtmi/samplesv3/extensions-1.json";
string fileName = "dtmi/azureiot/paad/iotphone-1.json";
Console.WriteLine(fileName);

var parserResult = await parser.ParseModelAsync(readFile(fileName));

foreach (var item in parserResult.Telemetries)
{
    Console.WriteLine( $"[T] {item.Name } ");
    Console.Write(ModelParser.GetTermOrUri(item.Schema.Id));
    item.SupplementalProperties.ToList().ForEach(p => Console.Write(" " + ((DTEnumValueInfo)p.Value).Name));
    Console.WriteLine("Supplemental Types: ");
    item.SupplementalTypes.ToList().ForEach(t => Console.Write("   " + ModelParser.GetTermOrUri(t)));
    Console.WriteLine();
}

foreach (var p in parserResult.Properties)
{
    Console.WriteLine($"[P] {p.Name } ");
    Console.Write(ModelParser.GetTermOrUri(p.Schema.Id));
    p.SupplementalProperties.ToList().ForEach(p => Console.Write(" " + ((DTEnumValueInfo)p.Value).Name));
    p.SupplementalTypes.ToList().ForEach(t => Console.Write(" " + ModelParser.GetTermOrUri(t)));
    Console.WriteLine();
    p.UndefinedTypes.ToList().ForEach(u => Console.Write(u));
    Console.WriteLine();
}

foreach (var c in parserResult.Commands)
{
    Console.WriteLine($"[C] {c.Name}");
    if (c.Request != null)
    {
        var req = c.Request;
        Console.Write($" req: {ModelParser.GetTermOrUri(req.Schema.Id)} ");
        req.SupplementalTypes.ToList().ForEach(t => Console.Write(" " + ModelParser.GetTermOrUri(t)));
        req.SupplementalProperties.ToList().ForEach(p => Console.Write(" " + ((DTEnumValueInfo)p.Value).Name));
    }    
}

foreach (var compo in parserResult.Components)
{
    Console.WriteLine($"[Co] {compo.Name} {compo.Schema.Id}");
    var compTels = parserResult.ObjectModel
                    .Where(c => c.Value.EntityKind == DTEntityKind.Telemetry && c.Value.ChildOf == compo.Schema.Id)
                    .Select(t => (DTTelemetryInfo)t.Value);
    foreach (var t in compTels)
    {
        Console.WriteLine($"    [CoT] {t.Name} {t.Schema.Id}");
    }

    var compProps = parserResult.ObjectModel
                    .Where(c => c.Value.EntityKind == DTEntityKind.Property && c.Value.ChildOf == compo.Schema.Id)
                    .Select(t => (DTPropertyInfo)t.Value);

    foreach(var p in compProps)
    {
        Console.WriteLine($"    [CoP] {p.Name} {p.Schema.Id}");
    }
}