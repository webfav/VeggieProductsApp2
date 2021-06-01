# VeggieProductsApp2
Schoolproject for "Web-development - backend" at Smartlearning https://www.smartlearning.dk/

### A webapplication made in Visual Studio 2019 as a ASP.net Core MVC project with Entity Framework i .NET core 3.1. 

### Make a local database on localhost. So first time run the migrations updating db.

1. Have the project open in Visual studio

2. To run on local pc, you should have a sql server installed on localhost.
Maybe you have to change the first part of the connectionstring in appsettings.json, if your localdb or server has a different name
"Server=(localdb)\\mssqllocaldb;Database=VeggieProductsApp2;Trusted_Connection=True;MultipleActiveResultSets=true"

3. open Package Manager Console
4. Write: update-database.
```bash
update-database
```

5. Check to see if database "VeggieProductsApp2" is made by opening in SQL Server Object Explorer.
If db is made, you can run the project.

6. For admin-login:
UserName: admin@gmail.com
Password: Admin!0

7. As an admin you can then input new Endusers, new categories and new products. 
There isn't made any mock-data, so database is empty at first.
