using Azure.IoT.ModelsRepository;
using dtdl_dotnet;
using DTDLParser;
using static dtdl_dotnet.ModelParserExtensions;

string basePath = Path.Join(System.Reflection.Assembly.GetExecutingAssembly().Location + @"./../../../../../");
var dmrClient = new ModelsRepositoryClient(new Uri(basePath));

var parser = new ModelParser(new ClientOptions()
{
    DtmiResolverAsync = dmrClient.ParserDtmiResolverAsync
});

string dtmi = "dtmi:azureiot:PaaD:IoTPhone;1";
Console.WriteLine($"Parser version: {parser.GetType().Assembly.FullName}");
Console.WriteLine($"MaxDtdlVersion:  \n");

var model = await dmrClient.GetModelAsync(dtmi, ModelDependencyResolution.Disabled);

try
{
    var dictParsed = await parser.ParseAsync(model.Content[dtmi]);
    var iinfo = new InterfaceInfo(dictParsed, new Dtmi(dtmi));

    Console.WriteLine($"Root {iinfo.Id}");
    iinfo.Print();
    iinfo.Components.ToList().ForEach(co => {
        Console.WriteLine($"[Co] {co.Name} ({co.Schema.Id})");
        var coInfo = new InterfaceInfo(dictParsed, co.Schema.Id);
        coInfo.Print();
    });
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

