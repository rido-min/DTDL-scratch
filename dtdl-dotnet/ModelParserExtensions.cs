
namespace DTDL_scratch
{
    using Microsoft.Azure.DigitalTwins.Parser;
    using Microsoft.Azure.DigitalTwins.Parser.Models;
    public static class DtmiExtensions
    {
        public static string ToPath(this Dtmi dtmi) => $"{dtmi.ToString().ToLowerInvariant().Replace(":", "/").Replace(";", "-")}.json";
    }

    public static class ModelParserExtensions
    {
        public class InterfaceInfo
        {
            DTEntityInfo root;
            public InterfaceInfo(IReadOnlyDictionary<Dtmi, DTEntityInfo> m) => root = m.Values.Where(v => v.EntityKind == DTEntityKind.Interface).First(e => e.ChildOf == null);
            public string Id => root.Id.ToString();

            public IEnumerable<DTTelemetryInfo> Telemetries =>
                ((DTInterfaceInfo)root).Contents
                    .Where(c => c.Value.EntityKind == DTEntityKind.Telemetry)
                    .Select(t => (DTTelemetryInfo)t.Value);

            public IEnumerable<DTPropertyInfo> Properties =>
                ((DTInterfaceInfo)root).Contents
                    .Where(c => c.Value.EntityKind == DTEntityKind.Property)
                    .Select(p => (DTPropertyInfo)p.Value);

            public IEnumerable<DTCommandInfo> Commands =>
                ((DTInterfaceInfo)root).Contents
                    .Where(c => c.Value.EntityKind == DTEntityKind.Command)
                    .Select(c => (DTCommandInfo)c.Value);

            public IEnumerable<DTComponentInfo> Components =>
                ((DTInterfaceInfo)root).Contents
                    .Where(c => c.Value.EntityKind == DTEntityKind.Component)
                    .Select(c => (DTComponentInfo)c.Value);

            public IEnumerable<DTRelationshipInfo> Relationships =>
                ((DTInterfaceInfo)root).Contents
                    .Where(c => c.Value.EntityKind == DTEntityKind.Relationship)
                    .Select(r => (DTRelationshipInfo)r.Value);
        }

        public static async Task<InterfaceInfo> ParseAsync(this ModelParser parser, string jsonContent)
            => new InterfaceInfo(await parser.ParseAsync(new string[] { jsonContent }));
    }
}