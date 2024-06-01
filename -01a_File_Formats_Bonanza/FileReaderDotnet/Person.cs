using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using CsvHelper.Configuration;

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