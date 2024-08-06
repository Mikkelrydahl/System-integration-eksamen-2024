using Microsoft.AspNetCore.Mvc;
using DataParsingServer.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text.Json;
using YamlDotNet.Serialization;
using CsvHelper;
using System.Globalization;
using System.Text;

namespace DataParsingServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {

        private static List<Person> persons = new List<Person>
        {
            new Person
            {
                Name = "Mikkel",
                Age = 27,
                Hobbies = new List<string> { "running", "gaming", "cooking" }
            }
        };

        [HttpGet]
        public ActionResult<List<Person>> GetPersons()
        {
            return Ok(persons);
        }

        [HttpGet("serialize/xml")]
        public ActionResult<string> SerializePersonToXml()
        {
            if (persons.Count == 0)
            {
                return NotFound("No persons available to serialize.");
            }

            Person person = persons[0];

            XmlSerializer serializer = new XmlSerializer(typeof(Person));
            using (StringWriter stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, person);
                return Ok(stringWriter.ToString());
            }
        }

        [HttpGet("serialize/json")]
        public ActionResult<string> SerializePersonToJson()
        {
            if (persons.Count == 0)
            {
                return NotFound("No persons available to serialize.");
            }

            Person person = persons[0];
            string json = JsonSerializer.Serialize(person);
            return Ok(json);
        }

        [HttpGet("serialize/yaml")]
        public ActionResult<string> SerializePersonToYaml()
        {
            if (persons.Count == 0)
            {
                return NotFound("No persons available to serialize.");
            }

            Person person = persons[0];

            var serializer = new Serializer();
            string yaml = serializer.Serialize(person);
            return Ok(yaml);
        }

        [HttpGet("serialize/csv")]
        public ActionResult<string> SerializePersonToCsv()
        {
            if (persons.Count == 0)
            {
                return NotFound("No persons available to serialize.");
            }

            Person person = persons[0];

            using (var writer = new StringWriter())
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecord(person);
                writer.Flush();
                return Ok(writer.ToString());
            }
        }

        [HttpGet("serialize/text")]
        public ActionResult<string> SerializePersonToText()
        {
            if (persons.Count == 0)
            {
                return NotFound("No persons available to serialize.");
            }

            Person person = persons[0];

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Name: {person.Name}");
            stringBuilder.AppendLine($"Age: {person.Age}");
            stringBuilder.AppendLine($"Hobbies: {string.Join(", ", person.Hobbies)}");

            return Ok(stringBuilder.ToString());
        }
    }
}