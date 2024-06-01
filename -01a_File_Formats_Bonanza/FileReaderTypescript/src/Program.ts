import { LoadPersonFrom } from "./LoadPersonFrom";
import { PersonToString } from "./Person";

let personJson = LoadPersonFrom.Json("..\\..\\02_Data\\me.json");
console.log("JSON::: " + PersonToString(personJson));

let personYaml = LoadPersonFrom.Yaml("..\\..\\02_Data\\me.yaml");
console.log("YAML::: " + PersonToString(personYaml));

let personCsv = LoadPersonFrom.Csv("..\\..\\02_Data\\me.csv");
console.log("CSV::: " + PersonToString(personCsv));

let personXml = LoadPersonFrom.Xml("..\\..\\02_Data\\me.xml");
console.log("XML::: " + PersonToString(personXml));
