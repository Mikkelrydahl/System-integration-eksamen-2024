using Newtonsoft.Json;
using CsvHelper;
using System.Globalization;
using System.Xml.Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public static class LoadPersonFrom
{
    public static Person Yaml(string fileLocation)
    {
        var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        using (StreamReader reader = File.OpenText(fileLocation))
        {
            Person person = deserializer.Deserialize<Person>(reader);
            return person;
        }
    }

    public static Person Xml(string fileLocation)
    {
        var serializer = new XmlSerializer(typeof(Person));
        using (var reader = new StreamReader(fileLocation))
        {
            Person person = (Person)serializer.Deserialize(reader)!;
            return person;
        }
    }

    public static Person Csv(string fileLocation)
    {
        using StreamReader reader = new StreamReader(fileLocation);
        using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<PersonMap>();
        var records = csv.GetRecords<Person>();
        Person person = records.First();
        //manually convert string array of hobbies to true list of hobbies
        List<string> hobbies = string.Join(",", person.hobbies).Split(",").ToList();
        person.hobbies = hobbies;
        return person;
    }

    public static Person Json(string fileLocation)
    {
        var serializer = new JsonSerializer();
        using (StreamReader reader = File.OpenText(fileLocation))
        {
            Person person = (Person)serializer.Deserialize(reader, typeof(Person))!;
            return person;
        }
    }
}