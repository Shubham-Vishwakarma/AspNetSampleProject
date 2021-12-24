# AspNetSampleProject
Rest API using ASP .Net Core

Reference: https://www.red-gate.com/simple-talk/development/dotnet-development/build-a-rest-api-in-net-core/


https://www.c-sharpcorner.com/article/tutorial-use-entity-framework-core-5-0-in-net-core-3-1-with-mysql-database-by2/

https://jasonwatmore.com/post/2021/10/26/net-5-connect-to-mysql-database-with-entity-framework-core

```
dotnet ef migrations add InitialCreate --context LibraryContext
dotnet ef database update --context LibraryContext

dotnet ef dbcontext scaffold "server=localhost;port=3306;database=shopbridge;uid=shubham;password=Shubham@123" "Pomelo.EntityFrameworkCore.Mysql" --output-dir Models --force
```