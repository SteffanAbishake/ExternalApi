# ExternalApi Cash Web API

## OVERVIEW
It wil fetch the data from 3rd partty external ap(JSONPlaceholder), then stores the data into SQL and implement the cashing mechanisam.
1 - Here data will retrive from the database first
2 - If data not found, it will fecth from the external API and it will store locally in the DB.

This dev use raw SQL querrirs.

## Features
- ASP.NET Core Web API
- External API Integration
- SQL Server databse cashing
- Repository pattern
- RAQ SQL querries
- RESTFul API design
- used swagger for UI

## FLOW
1- API request will comes to controller.
2- Then repository will check the database first.
3- If data exists -> return it.
4- If data not available -> call the external API
5- Then save the results to DB.
6- Return the response.

## The following pakages used in the project
Microsoft.Data.SqlClient
Swashbuckle.AspNetCore

How to install -> right click on the solution and got to Manage NuGet package for solution
then browse Microsoft.Data.SqlClient -> select -> Instal
then browse Swashbuckle.AspNetCore -> select -> install (install if not available)



## API end points.
1- Get all the post
	GET/api/posts

2- Get post by ID
	GET/api/posts/{id}

## DATABSE SETUP
1- Install the SQl server: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
2- Install SQL Server Management Studio SSME: https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms (optional - if GUI needed.)

3- Open CMD: Run : sqlcmd -S YOUR_SERVER_NAME -E


4-    CREATE DATABASE ExternalApiDb;
      GO

5-    USE ExternalApiDb;
      GO

6-    CREATE TABLE Posts
        (
            Id INT PRIMARY KEY,
            UserId INT,
            Title NVARCHAR(500),
            Body NVARCHAR(MAX)
        );
        GO
7- QUIT

## Update the appsetting.json file

make sure to add YOUR__SERVER name

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=ExternalApiDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

## How to run this project

1- Clone the project
git clone https://github.com/SteffanAbishake/ExternalApi.git

2- Restore the dependencies
dotnet restore

3-Build Project
dotnet build

4-Run Project
dotnet run

5- Swagger UI will open automatically if not open it https://localhost:{port}/swagger/index.html

6- Once UI open
    a - Click the first API end point -> click try out ->Execute
    b - Click the 3nd api end point -> click try out -> enter ID number ex: 5 - > Execute


