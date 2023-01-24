using Azure.IoT.ModelsRepository;
using dtdl_dotnet;
using DTDLParser;
using DTDLParser.Models;

string basePath = Path.Join(System.Reflection.Assembly.GetExecutingAssembly().Location + @"./../../../../../");
var dmrClient = new ModelsRepositoryClient(new Uri(basePath));

var parser = new ModelParser() 
{ 
    DtmiResolverAsync = dmrClient.ParserDtmiResolverAsync
    //DtmiResolverAsync = DmrClient.DtmiResolverAsync
};

string dtmi = "dtmi:samplesv3:mixingversions;1";
Console.WriteLine($"MaxDtdlVersion: {parser.MaxDtdlVersion}");

Console.WriteLine();
Console.WriteLine(dtmi);
Console.WriteLine();

var model = await dmrClient.GetModelAsync(dtmi, ModelDependencyResolution.Disabled);

ModelParserExtensions.InterfaceInfo? parserResult = null;


try
{
    parserResult = await parser.ParseModelAsync(model.Content[dtmi]);
}
catch (ParsingException pex)
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
    if (p.LanguageMajorVersion == 2)
    {
        p.SupplementalProperties.ToList().ForEach(p => Console.Write(" " + ModelParser.GetTermOrUri(((DTUnitInfo)p.Value).Id)));
    }
    else
    {
        p.SupplementalProperties.ToList().ForEach(p => Console.Write(" " + ((DTEnumValueInfo)p.Value).Name));
    }
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
    if (c.Response != null)
    {
        var resp = c.Response;
        Console.Write($" resp: {ModelParser.GetTermOrUri(resp.Schema.Id)}");
        resp.SupplementalTypes.ToList().ForEach(t => Console.Write(" " + ModelParser.GetTermOrUri(t)));
        resp.SupplementalProperties.ToList().ForEach(p => Console.Write(" " + ((DTEnumValueInfo)p.Value).Name));
    }
    Console.WriteLine();
}


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
    }

    var compCmds = compoDef.Contents
                   .Where(c => c.Value.EntityKind == DTEntityKind.Command)
                   .Select(t => (DTCommandInfo)t.Value);

    foreach (var c in compCmds)
    {
        Console.Write($"    [CoC] {c.Name}  ");
        if (c.Request != null)
        {
            var req = c.Request;
            Console.Write($"     req: {ModelParser.GetTermOrUri(req.Schema.Id)} ");
            req.SupplementalTypes.ToList().ForEach(t => Console.Write("    " + ModelParser.GetTermOrUri(t)));
            req.SupplementalProperties.ToList().ForEach(p => Console.Write("    " + ((DTEnumValueInfo)p.Value).Name));
        }
        if (c.Response != null)
        {
            var resp = c.Response;
            Console.Write($" resp: {ModelParser.GetTermOrUri(resp.Schema.Id)}");
            resp.SupplementalTypes.ToList().ForEach(t => Console.Write("    " + ModelParser.GetTermOrUri(t)));
            resp.SupplementalProperties.ToList().ForEach(p => Console.Write("     " + ((DTEnumValueInfo)p.Value).Name));
        }
        Console.WriteLine();
    }
}