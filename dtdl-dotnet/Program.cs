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

ModelParserExtensions.InterfaceInfo parserResult = null;

try
{
    parserResult = await parser.ParseModelAsync(readFile(fileName));
}
catch (DTDLParser.ParsingException pex)
{
    Console.WriteLine();
    Console.Error.WriteLine(pex.Message);
    Console.WriteLine();
    foreach (var err in pex.Errors)
    {
        Console.WriteLine($"{err.ValidationID}");
        Console.WriteLine(err.Cause);
        Console.WriteLine(err.Action);
        Console.WriteLine(  );
    }
    Environment.Exit(1);
}


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
    Console.WriteLine();
}

var batCo = parserResult.ObjectModel.First(x => x.Key == new Dtmi(new Uri("dtmi:azureiot:PaaD:Sensors:Battery;1")));



foreach (var compo in parserResult.Components)
{
    Console.WriteLine($"[Co] {compo.Name} {compo.Id}");

    DTInterfaceInfo compoDef = (DTInterfaceInfo)parserResult.ObjectModel.First(o => o.Key == compo.Schema.Id).Value;
    var compoTels = compoDef.Contents.Where(c => c.Value.EntityKind == DTEntityKind.Telemetry).Select(t => (DTTelemetryInfo)t.Value);
    
    foreach (var t in compoTels)
    {
        Console.Write($"    [CoT] {t.Name} {ModelParser.GetTermOrUri(t.Schema.Id)}");
        t.SupplementalProperties.ToList().ForEach(p => Console.Write(" " + ((DTEnumValueInfo)p.Value).Name));
        t.SupplementalTypes.ToList().ForEach(t => Console.Write("   " + ModelParser.GetTermOrUri(t)));
        Console.WriteLine();
    }

    var compProps = compoDef.Contents
                    .Where(c => c.Value.EntityKind == DTEntityKind.Property)
                    .Select(t => (DTPropertyInfo)t.Value);

    foreach(var p in compProps)
    {
        Console.Write($"    [CoP] {p.Name} {ModelParser.GetTermOrUri(p.Schema.Id)}");
        p.SupplementalProperties.ToList().ForEach(p => Console.Write("    " + ((DTEnumValueInfo)p.Value).Name));
        p.SupplementalTypes.ToList().ForEach(t => Console.Write("     " + ModelParser.GetTermOrUri(t)));
        Console.WriteLine();
        p.UndefinedTypes.ToList().ForEach(u => Console.Write(u));
    }
}