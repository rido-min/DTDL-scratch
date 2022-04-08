import * as fs from 'fs'
import { createParser }  from '@azure/dtdl-parser'
import * as path from 'path';
import { fileURLToPath } from 'url';

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const basePath =  path.join(__dirname,'./../')

const dtmiToPath = dtmi => `/${dtmi.toLowerCase().replace(/:/g, '/').replace(';', '-')}.json`

const resolveDtmi = dtmis => {
    const result = []
    dtmis.forEach(dtmi => {
        const resolvedDtdl =  fs.readFileSync(path.join(basePath,dtmiToPath(dtmi)), 'utf-8')
        result.push(JSON.parse(resolvedDtdl))
    })
    return result
}

async function main() {
    const dtdlRaw = fs.readFileSync(basePath + 'dtmi/samplesv2/anextendedinterface-1.json', 'utf-8')
    const modelParser = createParser()
    modelParser.getModels = resolveDtmi.bind(resolveDtmi);
    const modelDict = await modelParser.parse([dtdlRaw])
    Object.entries(modelDict).forEach(([dtmi, entity]) => {
        //console.log(dtmi, dtinfo.entityKind)
        switch (entity.entityKind) {
            case 'property': {
                console.log(` [P] ${entity.name} ${entity.schema?.id} ${!!entity.writable}`)
                break;
            }
            case 'telemetry': {
                console.log(` [T] ${entity.name} ${entity.schema?.id}`)
                break;
            }
        }
    })
}

main().catch((err) => {
    console.error("The sample encountered an error:", err);
})