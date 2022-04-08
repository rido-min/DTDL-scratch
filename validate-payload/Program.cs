using Microsoft.Azure.DigitalTwins.Parser;
using Microsoft.Azure.DigitalTwins.Parser.Models;
using System.Text.Json;

string basePath = Path.Join(System.Reflection.Assembly.GetExecutingAssembly().Location + @"./../../../../../");

string ReadFile(string path) => File.ReadAllText(Path.Join(basePath, path));
string ToJs(object obj) => JsonSerializer.Serialize(obj);


var objectModel = new ModelParser().Parse(new string[] { ReadFile("dtmi/samplesv2/inlinecomponent-1.json") });
var rootInterface = (DTInterfaceInfo)objectModel[new Dtmi("dtmi:samplesv2:inlineComponent;1")];
var myCompo = (DTComponentInfo)rootInterface.Contents["myCompo"];
var validPayload = myCompo.Schema.ValidateInstance(ToJs(new { __t = "c", myProp = "aValue" }));
Console.WriteLine(validPayload);





