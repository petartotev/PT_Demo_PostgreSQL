# PT_Demo_PostgreSQL
PT_Demo_PostgreSQL is a simple .NET 8 Console Application to test connectivity with PostgreSQL database using Npgsql.

## Contents

- [Initial Setup](#initial-setup)
- [Codebase](#codebase)
- [Links](#links)

## Initial Setup

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

## Codebase

In order to connect to the `postgres` database, you need to install the latest [Npgsql NuGet package](https://www.nuget.org/packages/Npgsql/).

## Links
- https://www.docker.com/blog/how-to-use-the-postgres-docker-official-image/
- https://earthly.dev/blog/postgres-docker/ - check 'Run the PostgreSQL Docker Container' section
- https://www.dronahq.com/best-postgresql-guis/
- https://github.com/npgsql/npgsql
- https://stackoverflow.com/questions/7718585/how-to-set-auto-increment-primary-key-in-postgresql - CREATE TABLE using AUTO INCREMENT (SERIAL)