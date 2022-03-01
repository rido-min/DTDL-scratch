using DTDL_scratch;

var model = await new DtdlModelParser().ParseAsync("dtmi/samples/anextendedinterface-1.json");

Console.WriteLine(model.Id);

foreach  (var t in model.Telemetries)
{
    Console.WriteLine($" [T] {t.Name} {t.DataType}");
}

foreach (var p in model.Properties)
{
    Console.WriteLine($" [P] {p.Name} {p.DataType}");
}
