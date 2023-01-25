using DTDLParser;
using DTDLParser.Models;
using System.Text;

namespace dtdl_dotnet
{
    public static class DtmiExtensions
    {
        public static string ToPath(this Dtmi dtmi) => $"{dtmi.ToString().ToLowerInvariant().Replace(":", "/").Replace(";", "-")}.json";
    }

    public static class ModelParserExtensions
    {
        public static string Print(this DTTelemetryInfo t, int pad = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" ".PadRight(pad));
            sb.Append($"[T] {t.Name} ");
            sb.Append(ModelParser.GetTermOrUri(t.Schema.Id));
            t.SupplementalTypes.ToList().ForEach(t => sb.Append(" " + ModelParser.GetTermOrUri(t)));
            if (t.LanguageMajorVersion == 2)
            {
                t.SupplementalProperties.ToList().ForEach(t => sb.Append(" " + ModelParser.GetTermOrUri(((DTUnitInfo)t.Value).Id)));
            }
            else
            {
                t.SupplementalProperties.ToList().ForEach(t => sb.Append(" " + ((DTEnumValueInfo)t.Value).Name));
            }
            return sb.ToString();
        }

        public static string Print(this DTPropertyInfo p, int pad = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" ".PadRight(pad));
            sb.Append($"[P] {p.Name} ");
            sb.Append(ModelParser.GetTermOrUri(p.Schema.Id));
            p.SupplementalTypes.ToList().ForEach(t => sb.Append(" " + ModelParser.GetTermOrUri(t)));
            if (p.LanguageMajorVersion == 2)
            {
                p.SupplementalProperties.ToList().ForEach(p => sb.Append(" " + ModelParser.GetTermOrUri(((DTUnitInfo)p.Value).Id)));
            }
            else
            {
                p.SupplementalProperties.ToList().ForEach(p => sb.Append(" " + ((DTEnumValueInfo)p.Value).Name));
            }
            return sb.ToString();
        }

        public static string Print(this DTCommandInfo c, int pad = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" ".PadRight(pad));
            sb.Append($"[C] {c.Name}");
            if (c.Request != null)
            {
                var req = c.Request;
                sb.Append($" req: {ModelParser.GetTermOrUri(req.Schema.Id)} ");
                req.SupplementalTypes.ToList().ForEach(t => sb.Append(" " + ModelParser.GetTermOrUri(t)));
                req.SupplementalProperties.ToList().ForEach(p => sb.Append(" " + ((DTEnumValueInfo)p.Value).Name));
            }
            if (c.Response != null)
            {
                var resp = c.Response;
                sb.Append($" resp: {ModelParser.GetTermOrUri(resp.Schema.Id)}");
                resp.SupplementalTypes.ToList().ForEach(t => sb.Append(" " + ModelParser.GetTermOrUri(t)));
                resp.SupplementalProperties.ToList().ForEach(p => sb.Append(" " + ((DTEnumValueInfo)p.Value).Name));
            }
            return sb.ToString();
        }

        public static string Print(this DTComponentInfo compo, DTInterfaceInfo compoDef)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"[Co] {compo.Name} ");
            var compoTels = compoDef.Contents.Where(c => c.Value.EntityKind == DTEntityKind.Telemetry).Select(t => (DTTelemetryInfo)t.Value);
            compoTels.ToList().ForEach(t => sb.AppendLine(t.Print(2)));
            var compoProps = compoDef.Contents.Where(c => c.Value.EntityKind == DTEntityKind.Property).Select(p => (DTPropertyInfo)p.Value);
            compoProps.ToList().ForEach(p => sb.AppendLine(p.Print(2)));
            var compoCmds = compoDef.Contents.Where(c => c.Value.EntityKind == DTEntityKind.Command).Select(cmd => (DTCommandInfo)cmd.Value);
            compoCmds.ToList().ForEach(cmd => sb.AppendLine(cmd.Print(2)));
            return sb.ToString();
        }

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