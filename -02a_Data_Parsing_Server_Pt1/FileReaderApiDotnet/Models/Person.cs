using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml.Serialization;
using CsvHelper;
using CsvHelper.Configuration;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

[XmlRoot("me")]
public class Person
{
    [XmlElement("name")]
    public required string name { get; set; }
    [XmlElement("age")]
    public int age { get; set; }
    [XmlArray("hobbies")]
    [XmlArrayItem("hobby")]
    public required List<string> hobbies { get; set; }
    private string hobbiesToString => string.Join(", ", hobbies);
    public override string ToString()
    {
        return $"Name:{name}, Age:{age}, Hobbies:{hobbiesToString}";
    }

    public string ToCsv()
    {
        return $"{nameof(name)},{nameof(age)},{nameof(hobbies)}\n{name},{age},\"{string.Join(",", hobbies)}\"";

        // IEnumerable<Person> persons = new List<Person> { this };

        // using (var writer = new StringWriter())
        // using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        // {
        //     csv.WriteRecords(persons);

        //     return writer.ToString();
        // }
    }

    public string ToYaml()
    {
        //return $"name: {name}\nage: {age}\nhobbies:\n  - {string.Join("\n  - ", hobbies)}";

        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        return serializer.Serialize(this);
    }

    public string ToXml()
    {
        var stringwriter = new StringWriter();
        var serializer = new XmlSerializer(typeof(Person));
        serializer.Serialize(stringwriter, this);
        return stringwriter.ToString();
    }
}

public class PersonMap : ClassMap<Person>
{
    public PersonMap()
    {
        Map(p => p.name).Name("name");
        Map(p => p.age).Name("age");
        Map(p => p.hobbies).Name("hobbies");
    }
}