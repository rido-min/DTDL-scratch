using Azure.IoT.ModelsRepository;
using dtdl_dotnet;
using DTDLParser;

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
    Console.WriteLine( $"[T] {item.Name } {item.Schema.Id}");
}

foreach (var p in parserResult.Properties)
{
    Console.WriteLine($"[P] {p.Name } {p.Schema.Id}");
    Console.WriteLine(string.Join(",", p.SupplementalTypes.Select(t => t.ToString())));
    Console.WriteLine(string.Join(",", p.SupplementalProperties.Select(t => t.ToString())));
    //Console.WriteLine(((DTEnumValueInfo)p.SupplementalProperties["dtmi:dtdl:extension:quantitativeTypes:v1:property:unit"]).Id);
    Console.WriteLine();
}