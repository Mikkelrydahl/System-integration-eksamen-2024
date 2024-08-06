namespace DataParsingServer.Models
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<string> Hobbies { get; set; }

        public Person()
        {
            //parameterless constructor 
        }
    }
}
