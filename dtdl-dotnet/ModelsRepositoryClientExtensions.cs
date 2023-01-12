namespace dtdl_dotnet
{
    using Azure.IoT.ModelsRepository;
    using DTDLParser;
    using System.Runtime.CompilerServices;

    internal static class ModelsRepositoryClientExtensions
    {
        //public static IEnumerable<string> ParserDtmiResolver(this ModelsRepositoryClient client, IReadOnlyCollection<Dtmi> dtmis) => client.ParserDtmiResolverAsync(dtmis).Result.ToEnumerable();

        public static async IAsyncEnumerable<string> ParserDtmiResolverAsync(this ModelsRepositoryClient client, IReadOnlyCollection<Dtmi> dtmis, 
            [EnumeratorCancellation] CancellationToken ct = default)
        {
            IEnumerable<string> dtmiStrings = dtmis.Select(s => s.AbsoluteUri);
            List<string> modelDefinitions = new();
            foreach (var dtmi in dtmiStrings)
            {
                ModelResult result = await client.GetModelAsync(dtmi, ModelDependencyResolution.Disabled, ct);
                yield return result.Content[dtmi];
            }
        }
    }
}
