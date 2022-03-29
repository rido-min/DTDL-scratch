const { createParser } = require('@azure/dtdl-parser')
const fs = require('fs')

const basePath = './../'

async function main() {
    const dtdlRaw = fs.readFileSync(basePath + 'dtmi/samplesv2/aninterface-1.json', 'utf-8')
    const modelParser = createParser()
    const modelDict = await modelParser.parse([dtdlRaw])
    Object.entries(modelDict).forEach(([dtmi, dtinfo]) => {
        console.log(dtmi, dtinfo.entityKind)
    })
}

main().catch((err) => {
    console.error("The sample encountered an error:", err);
})