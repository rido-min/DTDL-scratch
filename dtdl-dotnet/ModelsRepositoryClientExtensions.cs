namespace DTDL_scratch
{
    using Azure.IoT.ModelsRepository;
    using Microsoft.Azure.DigitalTwins.Parser;

    internal static class ModelsRepositoryClientExtensions
    {
        public static async Task<IEnumerable<string>> ParserDtmiResolverAsync(this ModelsRepositoryClient client, IReadOnlyCollection<Dtmi> dtmis)
        {
            IEnumerable<string> dtmiStrings = dtmis.Select(s => s.AbsoluteUri);
            List<string> modelDefinitions = new();
            foreach (var dtmi in dtmiStrings)
            {
                ModelResult result = await client.GetModelAsync(dtmi, ModelDependencyResolution.Disabled);
                modelDefinitions.Add(result.Content[dtmi]);
            }

            return modelDefinitions;
        }

        public static IEnumerable<string> ParserDtmiResolver(this ModelsRepositoryClient client, IReadOnlyCollection<Dtmi> dtmis)
        {
            IEnumerable<string> dtmiStrings = dtmis.Select(s => s.AbsoluteUri);
            List<string> modelDefinitions = new();
            foreach (var dtmi in dtmiStrings)
            {
                ModelResult result = client.GetModel(dtmi, ModelDependencyResolution.Disabled);
                modelDefinitions.Add(result.Content[dtmi]);
            }

            return modelDefinitions;
        }
    }
}
