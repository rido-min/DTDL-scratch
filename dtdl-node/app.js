import * as fs from 'fs'
import { createParser }  from '@azure/dtdl-parser'

const basePath = './../'
const dtmiToPath = dtmi => `/${dtmi.toLowerCase().replace(/:/g, '/').replace(';', '-')}.json`

const resolveDtmi = dtmi1 => {
    console.log('resolving ' + dtmi1)
    console.log(dtmiToPath(dtmi1))
    // const resolvedDtdl =  fs.readFileSync(basePath + dtmiToPath(dtmi), 'utf-8')
    // console.log(resolvedDtdl)
   // return [resolvedDtdl]
}

async function main() {
    const dtdlRaw = fs.readFileSync(basePath + 'dtmi/samplesv3/anextendedinterface-1.json', 'utf-8')
    const modelParser = createParser()
    modelParser.getModels = resolveDtmi;
    const modelDict = await modelParser.parse([dtdlRaw])
    Object.entries(modelDict).forEach(([dtmi, dtinfo]) => {
        console.log(dtmi, dtinfo.entityKind)
    })
}

main().catch((err) => {
    console.error("The sample encountered an error:", err);
})