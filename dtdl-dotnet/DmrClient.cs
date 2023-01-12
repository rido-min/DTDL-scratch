using DTDLParser;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace dtdl_dotnet;

internal class DmrClient
{
    internal static string DmrBasePath = "https://devicemodels.azure.com";
    static readonly Dictionary<string, string> modelDefinitions = new();

    static Task<string> ResolveDtmiAsync(string dtmi, string dmrBasePath, CancellationToken ct = default) =>
       new HttpClient().GetStringAsync($"{dmrBasePath}/{dtmi.Replace(':', '/').Replace(';', '-').ToLowerInvariant()}.json", ct);

    //internal static IEnumerable<string> DtmiResolver(IReadOnlyCollection<Dtmi> dtmis) => DtmiResolverAsync(dtmis).ToEnumerable();

    internal static async IAsyncEnumerable<string> DtmiResolverAsync(IReadOnlyCollection<Dtmi> dtmis, [EnumeratorCancellation]CancellationToken ct = default)
    {
        IEnumerable<string> dtmiStrings = dtmis.Select(s => s.AbsoluteUri);
        
        foreach (var dtmi in dtmiStrings)
        {
            yield return await ResolveDtmiAsync(dtmi, DmrBasePath, ct);
        }
        
    }
}