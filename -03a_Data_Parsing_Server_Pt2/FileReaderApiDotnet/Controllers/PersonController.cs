using System.Runtime.Serialization.DataContracts;
using System.Runtime.Serialization.DataContracts;
using Microsoft.AspNetCore.Mvc;

namespace FileReaderApiDotnet.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private static readonly Person person = new Person
    {
        name = "CasperDotnetGenerated",
        age = 24,
        hobbies = new List<string> { "coding", "eating", "gaming" }
    };

    private readonly ILogger<PersonController> _logger;

    public PersonController(ILogger<PersonController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<Person>> Get()
    {
        string? contentType = Request.ContentType;
        if (String.IsNullOrEmpty(contentType))
        {
            contentType = "application/json";
        }

        switch (contentType)
        {
            case "application/xml":
                return Content(person.ToXml(), "application/xml");

            case "application/json":
                return person;

            case "application/yaml" or "application/x-yaml":
                return Content(person.ToYaml(), "application/yaml");

            case "text/csv":
                return Content(person.ToCsv(), "text/csv");

            default:
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    private async Task<String> Fetch(string endUrl)
    {
        string baseUrl = "http://localhost:8080/";
        string rData = "";
        try
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(baseUrl + endUrl))
                {
                    using (HttpContent content = res.Content)
                    {
                        var data = await content.ReadAsStringAsync();
                        if (data != null)
                        {
                            Console.WriteLine(data);
                            rData = data;
                        }
                        else
                        {
                            Console.WriteLine("NO DATA");
                        }
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine("Exception Hit------------");
            Console.WriteLine(exception);
        }
        return rData;
    }

    [HttpGet("External")]
    public async Task<ActionResult<String>> GetExternal()
    {
        string? contentType = Request.ContentType;
        if (String.IsNullOrEmpty(contentType))
        {
            contentType = "application/json";
        }

        switch (contentType)
        {
            case "application/xml":
                Response.ContentType = "application/xml";
                string xmlData = Fetch("xml").Result;
                if (String.IsNullOrEmpty(xmlData))
                    return StatusCode(StatusCodes.Status500InternalServerError, "No Data Found");
                return Content(xmlData);

            case "application/json":
                Response.ContentType = "application/json";
                string jsonData = Fetch("json").Result;
                if (String.IsNullOrEmpty(jsonData))
                    return StatusCode(StatusCodes.Status500InternalServerError, "No Data Found");
                return Content(jsonData);

            case "application/yaml" or "application/x-yaml":
                Response.ContentType = "application/yaml";
                string yamlData = Fetch("yaml").Result;
                if (String.IsNullOrEmpty(yamlData))
                    return StatusCode(StatusCodes.Status500InternalServerError, "No Data Found");
                return Content(yamlData);

            case "text/csv":
                Response.ContentType = "text/csv";
                string csvData = Fetch("csv").Result;
                if (String.IsNullOrEmpty(csvData))
                    return StatusCode(StatusCodes.Status500InternalServerError, "No Data Found");
                return Content(csvData);

            default:
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}