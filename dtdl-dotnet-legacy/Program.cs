using Azure.IoT.ModelsRepository;
using Microsoft.Azure.DigitalTwins.Parser;
using dtdl_dotnet_legacy;

string basePath = Path.Join(System.Reflection.Assembly.GetExecutingAssembly().Location + @"./../../../../../");
string readFile(string path) => File.ReadAllText(Path.Join(basePath, path));

var parser = new ModelParser()
{
    //Options = new HashSet<ModelParsingOption> { ModelParsingOption.}
    //DtmiResolverAsync = new ModelsRepositoryClient(new Uri(basePath)).ParserDtmiResolverAsync
};
Console.WriteLine(parser.GetType().Assembly.Location);

var parserResult = await parser.ParseAsync(readFile("dtmi/samplesv2/extensions-1.json"));

foreach (var item in parserResult.Telemetries)
{
    Console.WriteLine($"[T] {item.Name } {item.Schema.Id}");
}

foreach (var p in parserResult.Properties)
{
    Console.WriteLine($"[P] {p.Name } {p.Schema.Id}");
}