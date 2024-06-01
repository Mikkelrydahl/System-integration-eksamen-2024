-- Active: 1714921897979@@127.0.0.1@3306@master
CREATE TABLE roles (
    person int NOT NULL AUTO_INCREMENT, 
    job VARCHAR(255) NOT NULL UNIQUE,
    PRIMARY KEY (person)
);

CREATE TABLE jobs (
    job_title VARCHAR(255), 
    start_year DATETIME,
    FOREIGN KEY (job_title) REFERENCES roles(job)
);

CREATE TABLE people (
    id int, 
    name VARCHAR(255) NOT NULL, 
    birth_year DATETIME,    
    FOREIGN KEY (id) REFERENCES roles(person)
);