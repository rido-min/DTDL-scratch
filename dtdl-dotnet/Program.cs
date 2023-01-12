using Azure.IoT.ModelsRepository;
using dtdl_dotnet;
using DTDLParser;
using DTDLParser.Models;

string basePath = Path.Join(System.Reflection.Assembly.GetExecutingAssembly().Location + @"./../../../../../");
string readFile (string path) => File.ReadAllText(Path.Join(basePath, path));

var parser = new ModelParser() 
{ 
    DtmiResolverAsync = new ModelsRepositoryClient(new Uri(basePath)).ParserDtmiResolverAsync
    //DtmiResolverAsync = DmrClient.DtmiResolverAsync
};
Console.WriteLine($"MaxDtdlVersion: {parser.MaxDtdlVersion}");

//var st = parser.GetSupplementalTypes().Where(t => t.Value.ExtensionKind == DTExtensionKind.SemanticUnit);
//st.ToList().ForEach(t => {
//    Console.WriteLine($"{ModelParser.GetTermOrUri(t.Key)} {t.Key} {t.Value.ContextId}");
//});

string fileName = args.Length>0 ? args[0] : "dtmi/samplesv3/quantitativeTypes-1.json";
Console.WriteLine(fileName);

var parserResult = await parser.ParseModelAsync(readFile(fileName));

foreach (var item in parserResult.Telemetries)
{
    Console.WriteLine( $"[T] {item.Name } ");
    Console.Write(ModelParser.GetTermOrUri(item.Schema.Id));
    item.SupplementalTypes.ToList().ForEach(t => Console.Write(" " + ModelParser.GetTermOrUri(t)));
    item.SupplementalProperties.ToList().ForEach(p => Console.Write(" " + ((DTEnumValueInfo)p.Value).Name));
    Console.WriteLine();
}

foreach (var p in parserResult.Properties)
{
    Console.WriteLine($"[P] {p.Name } ");
    Console.Write(ModelParser.GetTermOrUri(p.Schema.Id));
    p.SupplementalTypes.ToList().ForEach(t => Console.Write(" " + ModelParser.GetTermOrUri(t)));
    p.SupplementalProperties.ToList().ForEach(p => Console.Write(" " + ((DTEnumValueInfo)p.Value).Name));
    Console.WriteLine();
}

foreach (var c in parserResult.Commands)
{
    Console.WriteLine($"[C] {c.Name}");
    var req = c.Request;
    Console.Write($" req: {ModelParser.GetTermOrUri(req.Schema.Id)} ");
    req.SupplementalTypes.ToList().ForEach(t => Console.Write(" " + ModelParser.GetTermOrUri(t)));
    req.SupplementalProperties.ToList().ForEach(p => Console.Write(" " + ((DTEnumValueInfo)p.Value).Name));

}