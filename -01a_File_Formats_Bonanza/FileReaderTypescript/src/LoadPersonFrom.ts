import { readFileSync } from "fs";
import { Person } from "./Person";
import YAML from "yaml";
import * as CSV from "csv-string";
import xml2js from "xml2js";

export class LoadPersonFrom {
  static Xml(fileLocation: string): Person {
    var data = readFileSync(fileLocation, "utf8");
    let rData: Person = {
      name: "",
      age: 0,
      hobbies: [],
    };

    xml2js.parseString(data, function (err: any, result: any) {
      rData = {
        name: result.me.name[0],
        age: Number(result.me.age[0]),
        hobbies: result.me.hobbies[0].hobby,
      };
    });

    return rData as Person;
  }

  static Csv(fileLocation: string): Person {
    var data = readFileSync(fileLocation, "utf8");

    let row = CSV.parse(data)[1];

    //static solution written by hand
    let rData: Person = {
      name: row[0],
      age: Number(row[1]),
      hobbies: row[2].split(","),
    };

    return rData as Person;
  }

  static Yaml(fileLocation: string): Person {
    var data = readFileSync(fileLocation, "utf8");
    return YAML.parse(data) as Person;
  }

  static Json(fileLocation: string): Person {
    var data = readFileSync(fileLocation, "utf8");
    return JSON.parse(data) as Person;
  }
}
