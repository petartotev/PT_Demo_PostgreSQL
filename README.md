# PT_Demo_PostgreSQL
PT_Demo_PostgreSQL is a simple .NET 8 Console Application to test connectivity with PostgreSQL database using Npgsql.

## Contents

- [MySQL vs PostgreSQL Comparison](#mysql-vs-postgresql-comparison)
- [PosgreSQL using Docker](#postgresql-using-docker)
    - [Initial Setup (Docker)](#initial-setup-docker)
    - [Codebase (Docker)](#codebase-docker)
        - [Demo Persons](#demo-persons) - create simple table
        - [Demo Employees](#demo-employees) - create table that stores objects as `JSONB`
        - [Demo Students](#demo-students) - create table using `TYPE` that stores objects having a complex property (Address)
- [PostgreSQL using Linux](#postgresql-using-linux)
    - [Initial Setup (Linux)](#initial-setup-linux)
    - [Codebase (Linux)](#codebase-linux)
- [Links](#links)

## MySQL vs PostgreSQL Comparison

| Feature           | MySQL                                            | PostgreSQL                          |
| ----------------- | ------------------------------------------------ | ----------------------------------- |
| ACID compliance   | ❓ Only with InnoDB + NDB Cluster storage engine | ✅ Fully compliant                  |
| Multiversion Concurrency control (MVCC) | ❌                        | ✅                                  |
| Indexes           | B-Tree, R-Tree                                   | B-Tree, R-Tree, expression/partial/hash indexes |
| Data types        |                                                  | Store data as objects (with properties), XML, arrays
| Views             | ✅                                              | ✅ Advanced view options            |
| Stored procedures | ✅ Written in SQL                               | ✅ Written in SQL / other languages |
| Triggers          | BEFORE, AFTER                                    | INSTEAD OF                          |


## PostgreSQL using Docker

### Initial Setup (Docker)

1. Execute the following Docker command in cmd.exe in order to pull the latest `postgres` image:

```
docker pull postgres:latest
```

2. Next, run the following Docker command in cmd.exe in order to run the pulled `postgres` image in a container named `postgrescntr` on port 5432:

```
docker run --name postgrescntr -e POSTGRES_PASSWORD=test1234 -p 5432:5432 -d postgres
```

3. Connect to the database using [DBeaver](https://dbeaver.io/download/) or [pgAdmin](https://www.pgadmin.org/download/):

![dbeaver-scrot](./res/scrot_dbeaver_connect.png)

### Codebase (Docker)

In order to connect to the `postgres` database, you need to install the latest [Npgsql NuGet package](https://www.nuget.org/packages/Npgsql/).

#### Demo Persons

```
CREATE TABLE Persons(
PersonID SERIAL PRIMARY KEY NOT NULL,
FirstName varchar(255),
LastName varchar(255),
Gender varchar(1),
Address varchar(255),
City varchar(255));
```

#### Demo Employees

```
CREATE TABLE Employees (
EmployeeId SERIAL PRIMARY KEY,
EmployeeData JSONB);
```

#### Demo Students

```
CREATE TYPE Address AS (
street VARCHAR,
city VARCHAR,
state VARCHAR,
zip_code VARCHAR);
```

```
 CREATE TABLE Students (
 studentid SERIAL PRIMARY KEY,
 firstname VARCHAR,
 lastname VARCHAR,
 age INT,
 address Address);
```

![type-address-in-students-table](./res/scrot_dbeaver_type-address-in-students-table.png)

## PostgreSQL using Linux

### Initial Setup (Linux)

1. Install PostgreSQL using Terminal on Linux:

```
root@hostname: sudo apt install postgresql
```

2. Switch to User `Postgres`:

```
root@hostname:/var/mydir# sudo -i -u postgres
```

3. Connect to PostgreSQL Server using User `Postgres`:

```
postgres@hostname:~$ psql
	psql (15.6 (Debian 15.6-0+deb12u1))
	Type "help" for help.
```

4. Check PostgreSQL Version (Optional):

```
postgres=# select version();
	PostgreSQL 15.6 (Debian 15.6-0+deb12u1) on x86_64-pc-linux-gnu, compiled by gcc (Debian 12.2.0-14) 12.2.0, 64-bit
	(1 row)
```

5. Get Connection Information (Optional):

```
postgres=# \conninfo
	You are connected to database "postgres" as user "postgres" via socket in "/var/run/postgresql" at port "5432".
```

6. Get List of Databases (Optional):

```
postgres=# \l
													List of databases
	Name    |  Owner   | Encoding |   Collate   |    Ctype    | ICU Locale | Locale Provider |   Access privileges   
	-----------+----------+----------+-------------+-------------+------------+-----------------+-----------------------
	postgres  | postgres | UTF8     | en_US.UTF-8 | en_US.UTF-8 |            | libc            | 
	template0 | postgres | UTF8     | en_US.UTF-8 | en_US.UTF-8 |            | libc            | =c/postgres          +
			|          |          |             |             |            |                 | postgres=CTc/postgres
	template1 | postgres | UTF8     | en_US.UTF-8 | en_US.UTF-8 |            | libc            | =c/postgres          +
			|          |          |             |             |            |                 | postgres=CTc/postgres
	(3 rows)
```

7. Change Password for User `Postgres`:

```
postgres=# alter user postgres with password 'topsecretpassword@github.com';
	ALTER ROLE
```

8. Quit from PostgreSQL Server:

```
postgres-# quit
	Use \q to quit.
postgres-# \q
```

8. Log out as User `Postgres`:

```
postgres@hostname:~$ exit
	logout
```

### Codebase (Linux)

1. Create database `TestDB` using Workbench:

```
CREATE DATABASE testdb
```

2. Create any Table (e.g. `Persons`) in the newly created database `TestDB`:

```
CREATE TABLE Persons (
    Personid SERIAL PRIMARY KEY,
    LastName varchar(255) NOT NULL,
    FirstName varchar(255),
    Age int);

select * from Persons;
```

3. Using Linux Terminal, grant privileges on newly created database `TestDB` to User `Postgres`:

```
postgres=# grant all privileges on database testdb to postgres;
GRANT
postgres=#
```

## Links
- https://www.docker.com/blog/how-to-use-the-postgres-docker-official-image/
- https://earthly.dev/blog/postgres-docker/ - check 'Run the PostgreSQL Docker Container' section
- https://www.dronahq.com/best-postgresql-guis/
- https://github.com/npgsql/npgsql
- https://stackoverflow.com/questions/7718585/how-to-set-auto-increment-primary-key-in-postgresql - CREATE TABLE using AUTO INCREMENT (SERIAL)