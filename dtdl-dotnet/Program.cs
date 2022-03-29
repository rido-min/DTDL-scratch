using Azure.IoT.ModelsRepository;
using DTDL_scratch;
using Microsoft.Azure.DigitalTwins.Parser;

string basePath = Path.Join(System.Reflection.Assembly.GetExecutingAssembly().Location + @"./../../../../../");
string readFile (string path) => File.ReadAllText(Path.Join(basePath, path));

var parser = new ModelParser() 
{ 
    //Options = ModelParsingOption.RejectUndefinedExtensions,
    DtmiResolverAsync = new ModelsRepositoryClient(new Uri(basePath)).ParserDtmiResolver 
};

var parserResult = await parser.ParseAsync(readFile("dtmi/samplesv2/aninterface-1.json"));

foreach (var item in parserResult.Telemetries)
{
    Console.WriteLine( $"[T] {item.Name } {item.Schema.Id}");
}

foreach (var p in parserResult.Properties)
{
    Console.WriteLine($"[P] {p.Name } {p.Schema.Id}");
}