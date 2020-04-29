# How to update the DB

[reference](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

We want to make sure our code models and the DB tables stay in sync. When adding new properties and objects we must update the database through migrations...

* Make sure the migrations assembly has the `Microsoft.EntityFrameworkCore.Tools` nuget package
* Make sure the migrations assembly has a reference to where the DbContext is
* Make sure the default startup project is the `SharedList.API.Presentation`
* Make sure the `SharedList.API.Presentation` references the migrations Assembly
* Make sure the `SharedList.API.Presentation` specifies the migrations assembly in the DbContext options builder
```
options.UseSqlServer(
    Configuration.GetConnectionString("DefaultConnection"), 
    x => x.MigrationsAssembly("SharedList.Migrations")));
```

* In the package manager console create a new migration 
`Add-Migration MigrationName -Project [MigrationsProject]` - best to make sure the name reflects what you're doing the db, such which table or column is being updated or for what feature change
* Running that command should result in a couple of files being added to the Migrations folder
* Then run `Update-Database MigrationName -Project [MigrationsProject]`, this should result in the Database being updated with the required changes
