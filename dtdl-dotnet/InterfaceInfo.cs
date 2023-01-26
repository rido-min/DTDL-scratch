using DTDLParser;
using DTDLParser.Models;

namespace dtdl_dotnet
{

    public static partial class ModelParserExtensions
    {
        public class InterfaceInfo
        {
            public IReadOnlyDictionary<Dtmi, DTEntityInfo> ObjectModel;
            public readonly DTEntityInfo root;
            public InterfaceInfo(IReadOnlyDictionary<Dtmi, DTEntityInfo> m)
            {
                ObjectModel = m;
                root = m.Values.Where(v => v.EntityKind == DTEntityKind.Interface).First(e => e.ChildOf == null);
            }

            public InterfaceInfo(IReadOnlyDictionary<Dtmi, DTEntityInfo> m, Dtmi dtmi)
            {
                ObjectModel = m;
                root = m[dtmi];
            }

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
    }
}