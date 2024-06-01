import { LoadPersonFrom } from "./LoadPersonFrom";
import { Person, PersonToString } from "./Person";
import express, { Express, Request, Response } from "express";
import YAML from "yaml";
import * as CSV from "csv-string";
import xmljs from "xml-js";

// let personJson: Person = LoadPersonFrom.Json("..\\02_Data\\me.json");
// let personYaml: Person = LoadPersonFrom.Yaml("..\\02_Data\\me.yaml");
// let personCsv: Person = LoadPersonFrom.Csv("..\\02_Data\\me.csv");
// let personXml: Person = LoadPersonFrom.Xml("..\\02_Data\\me.xml");

const person: Person = {
  name: "CasperTypescriptGenerated",
  age: 24,
  hobbies: ["coding", "eating", "gaming"],
};

const app: Express = express();
const port = 8080;

app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});

app.get("/json/External", async (req: Request, res: Response) => {
  const response = await fetch("http://localhost:5149/Person");
  const result = await response.json();
  res.send(result);
});

app.get("/json", (req: Request, res: Response) => {
  res.send(person);
});

app.get("/yaml/External", async (req: Request, res: Response) => {
  const response = await fetch("http://localhost:5149/Person", {
    headers: new Headers({
      "content-type": "application/yaml",
    }),
  });
  const result = await response.text();
  res.send(result);
});

app.get("/yaml", async (req: Request, res: Response) => {
  let yamlData = YAML.stringify(person);
  res.send(yamlData);
});

app.get("/csv/External", async (req: Request, res: Response) => {
  const response = await fetch("http://localhost:5149/Person", {
    headers: new Headers({
      "content-type": "text/csv",
    }),
  });
  const result = await response.text();
  res.send(result);
});

app.get("/csv", async (req: Request, res: Response) => {
  let csvData = CSV.stringify([Object.values(person)]);
  res.send(csvData);
});

app.get("/xml/External", async (req: Request, res: Response) => {
  const response = await fetch("http://localhost:5149/Person", {
    headers: new Headers({
      "content-type": "application/xml",
    }),
  });
  const result = await response.text();
  res.send(result);
});

app.get("/xml", async (req: Request, res: Response) => {
  var options = { compact: true, ignoreComment: true, spaces: 4 };
  let xmlData = xmljs.js2xml(person, options);
  res.send(xmlData);
});
