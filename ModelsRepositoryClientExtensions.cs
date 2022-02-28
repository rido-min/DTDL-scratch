using Azure.IoT.ModelsRepository;
using Microsoft.Azure.DigitalTwins.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTDL_scratch
{
    internal static class ModelsRepositoryClientExtensions
    {
        public static async Task<IEnumerable<string>> ParserDtmiResolver(this ModelsRepositoryClient client, IReadOnlyCollection<Dtmi> dtmis)
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
    }
}
