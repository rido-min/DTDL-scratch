import { createParser } from '@azure/dtdl-parser'

const dtdl = {
    '@context': 'dtmi:dtdl:context;3',
    '@id': 'dtmi:test:sample;1',
    '@type': 'Interface',
    displayName: 'onedep',
    contents: [
        {
            '@type': [ 'Property', 'Temperature' ],
            'name': 'interval',
            schema: 'integer',
            unit: 'kelvin',
            writable: true
          },
          {
            "@type": [ "Telemetry" ],
            name: "temperature",
            schema: "double"
          }
    ]
}

async function main() {
    const modelParser = createParser()
    const modelDict = await modelParser.parse([JSON.stringify(dtdl)])
    Object.entries(modelDict).forEach(([dtmi, dtinfo]) => {
        console.log(dtmi, dtinfo.entityKind)
    })

}

main().catch((err) => {
    console.error("The sample encountered an error:", err);
});