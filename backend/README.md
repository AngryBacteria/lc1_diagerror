#Run
```sh
dotnet run --urls=https://localhost:7184
```

#Build DB
```sh
dotnet ef migrations add InitialCreate
```

```sh
dotnet ef database update
```