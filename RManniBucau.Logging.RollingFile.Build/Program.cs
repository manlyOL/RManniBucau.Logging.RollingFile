using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using RManniBucau.Logging.RollingFile;

var optionsClass = typeof(RollingFileOptions);
var projectBase = Path.Combine(
    new DirectoryInfo(optionsClass.Assembly.Location)
        .Parent!
        .Parent!
        .Parent!
        .Parent!
        .Parent!
        .FullName,
    "RManniBucau.Logging.RollingFile"
);
var readme = Path.Combine(projectBase, "README.md");
var xmlDoc = Path.Combine(projectBase, "obj/doc/RManniBucau.Logging.RollingFile.xml");

var content = File.ReadAllText(readme);
var start = content.IndexOf("<!-- Start Generated Content -->");
var end = content.IndexOf("<!-- End Generated Content -->");
if (start < 0 || end < 0)
{
    throw new InvalidOperationException("missing placeholders for the readme content");
}

var instance = Activator.CreateInstance(optionsClass)!;
Dictionary<string, string> defaultValues = [];
foreach (var prop in optionsClass.GetProperties())
{
    defaultValues[prop.Name] = prop.GetValue(instance)?.ToString() ?? "null";
}
// override time provider which is a lambda
defaultValues["TimeProvider"] = "() => DateTime.Now";

var doc = XDocument.Load(xmlDoc);
var propPrefix = $"P:{optionsClass.FullName}.";
var properties = doc.Descendants("member")
    .Where(it =>
        (it.Attribute("name")?.Value.StartsWith(propPrefix) ?? false)
        && it.Element("summary") is not null
    )
    .Select(it =>
    {
        var name = it.Attribute("name")!.Value[propPrefix.Length..];
        return (
            Name: name,
            Summary: it.Element("summary")?.Value.Trim().Replace('\n', ' '),
            Default: defaultValues[name]
        );
    })
    .OrderBy(it => it.Name)
    .Select(it => $"{it.Name} | {it.Summary} | `{it.Default}`");

var prefix = content[0..(content.IndexOf('>', start) + 1)];
var suffix = content[end..];
content =
    $"{prefix}\nName | Summary | Default\n| :----- | :----: | -----:\n{string.Join('\n', properties)}\n{suffix}";

File.WriteAllText(readme, content);
