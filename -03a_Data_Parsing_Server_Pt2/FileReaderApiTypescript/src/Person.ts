export interface Person {
  name: string;
  age: number;
  hobbies: string[];
}

export function PersonToString(person: Person): string {
  return `Name:${person.name}, Age:${person.age}, Hobbies:${person.hobbies}`;
}
