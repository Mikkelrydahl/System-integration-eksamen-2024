create database in docker

```
$ docker run --name mysql-system-integration -p 3306:3306 -e MYSQL_ROOT_PASSWORD=password -d mysql
$ docker exec -it mysql-system-integration bash
$ mysql -u root -p
$ password
```

Run the 3 sql files on database to setup mock tables

use MRO to generate HTML docs for MySQL

```
$ npx mro
```
