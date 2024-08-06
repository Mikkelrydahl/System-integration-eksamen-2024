using Microsoft.AspNetCore.Mvc;

namespace FileReaderApiDotnet.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private static readonly Person person = new Person
    {
        name = "Mikkel",
        age = 27,
        hobbies = new List<string> { "running", "gaming", "cooking" }
    };


    [HttpGet]
    public ActionResult<Person> Get()
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

}