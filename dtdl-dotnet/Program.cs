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

string dtmi = "dtmi:samplesv3:quantitativeTypes;2";
Console.WriteLine($"MaxDtdlVersion: {parser.MaxDtdlVersion}");

Console.WriteLine();
Console.WriteLine(dtmi);
Console.WriteLine();

var model = await dmrClient.GetModelAsync(dtmi, ModelDependencyResolution.Disabled);

ModelParserExtensions.InterfaceInfo? parserResult = null;


try
{
    var dictParsed = await parser.ParseAsync(model.Content[dtmi]);
    parserResult = new ModelParserExtensions.InterfaceInfo(dictParsed, new Dtmi(dtmi));
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

parserResult.Telemetries.ToList().ForEach(t => Console.WriteLine(t.Print()));
parserResult.Properties.ToList().ForEach(p => Console.WriteLine(p.Print()));
parserResult.Commands.ToList().ForEach(c => Console.WriteLine(c.Print()));
parserResult.Components.ToList().ForEach(co => Console.WriteLine(co.Print((DTInterfaceInfo)parserResult.ObjectModel.First(o => o.Key == co.Schema.Id).Value)));

