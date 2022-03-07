using Microsoft.Azure.DigitalTwins.Parser;
using Microsoft.Azure.DigitalTwins.Parser.Models;
//using Microsoft.Azure.DigitalTwins.Parser.Models;
using System.Collections.ObjectModel;

namespace DTDL_scratch
{

    public static class DtmiExtensions
    {
        public static string ToPath(this Dtmi dtmi) => $"{dtmi.ToString().ToLowerInvariant().Replace(":", "/").Replace(";", "-")}.json";
    }


    public class ParserResult : ReadOnlyDictionary<Dtmi, DTEntityInfo>
    {
        public ParserResult(IReadOnlyDictionary<Dtmi, DTEntityInfo> dictionary) 
            : base(dictionary.ToDictionary(k => k.Key, v => v.Value))
        {
        }
    }

    public class ElementInfo
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string DataType { get; set; }
    }

    

    public class InterfaceInfo
    {
        ParserResult model;
        DTEntityInfo root;
        public InterfaceInfo(ParserResult m)
        {
            model = m;
            root = model.Values.Where(v => v.EntityKind == DTEntityKind.Interface).First( e=> e.ChildOf == null);
        }

        public string Id 
        {
            get => root.Id.ToString();
        }

        public IEnumerable<ElementInfo> Telemetries 
        { 
            get
            {
                foreach (var content in ((DTInterfaceInfo)root).Contents.Where(c => c.Value.EntityKind == DTEntityKind.Telemetry))
                {
                    yield return new ElementInfo
                    {
                        Name = content.Value.Name,
                        Id = content.Value.Id.ToString(),
                        //DataType = ModelParser.GetTermOrUri(((DTTelemetryInfo)content.Value).Schema.Id)
                    };
                }
            }
        }

        public IEnumerable<ElementInfo> Properties
        {
            get
            {
                foreach (var content in ((DTInterfaceInfo)root).Contents.Where(c => c.Value.EntityKind == DTEntityKind.Property))
                {
                    yield return new ElementInfo
                    {
                        Name = content.Value.Name,
                        Id = content.Value.Id.ToString(),
                        //DataType = ModelParser.GetTermOrUri(((DTPropertyInfo)content.Value).Schema.Id)
                    };
                }
            }
        }

    }

    public class DtdlModelParser
    {
        string basePath;

        public DtdlModelParser()
        {
            basePath = System.Reflection.Assembly.GetExecutingAssembly().Location + @"./../../../../";
        }
        public DtdlModelParser(string basePath)
        {
            this.basePath = basePath;
        }

        async Task<IEnumerable<string>> Resolver(IReadOnlyCollection<Dtmi> dtmis)
        {
            var result = new List<string>();
            foreach (var dtmi in dtmis)
            {
                result.Add(await File.ReadAllTextAsync(Path.Combine(basePath, dtmi.ToPath())));
            }
            return result;
        }

        ParserResult parserResult;

        public async Task<InterfaceInfo> ParseAsync(string relativePath)
        {
            var json = await File.ReadAllTextAsync(Path.Join(basePath, relativePath));
            ModelParser parser = new ModelParser() { DtmiResolverAsync = Resolver};
            var result = await parser.ParseAsync(new string[] { json });
            parserResult = new ParserResult(result);
            return new InterfaceInfo(parserResult);
        }
    }
}
