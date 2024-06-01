Console.WriteLine("Hello world!");
string dataFolder = getDataFolder();

Person personJson = LoadPersonFrom.Json(dataFolder+"me.json");
Console.WriteLine("JSON::: " + personJson.ToString());

Person personCsv = LoadPersonFrom.Csv(dataFolder+"me.csv");
Console.WriteLine("CSV::: " + personCsv.ToString());

Person personXml = LoadPersonFrom.Xml(dataFolder+"me.xml");
Console.WriteLine("XML::: " + personXml.ToString());

Person personYaml = LoadPersonFrom.Yaml(dataFolder+"me.yaml");
Console.WriteLine("YAML::: " + personYaml.ToString());

string getDataFolder() //hackjob to find folder with data files
{
    String rootFolder = AppDomain.CurrentDomain.BaseDirectory;
    String[] rootFolderArray = rootFolder.Split(@"\");
    int result = Array.FindIndex(rootFolderArray, element => element.Contains("systemintegration2024"));
    Array.Resize(ref rootFolderArray, result + 1);
    string dataFolder = string.Join("/", rootFolderArray);
    dataFolder += "/02_Data/";
    return dataFolder;
}