# Price-depo backend

The main purpose of this project is to get deeper knowledge about ASP.Net Core 2.1, C# and related technologies, libraries, common patterns and best practices.

## Getting started
1. Install dotnet CLI
2. Open project root dir
3. Run `dotnet run` command. Application is now listening on `http://localhost:5000`
4. Create a GET HTTP request to `http://localhost:5000/api/product`

## Progress so far:
1. Generics CrudRepository interface
2. Abstract In-memory implementation  for CrudRepository
3. Example implementation called `InMemoryProductCrudRepository`

## TODO:
1. Generics abstract CRUD Controller
2. Dynamic query building logic

# AWS:
in command line:
1. Run command `aws ecr get-login --no-include-email`
2. Copy and run output of previous command
3. Tag latest built image: `docker tag plutoz/pricedepo-dotnet-backend:latest965295834176.dkr.ecr.eu-central-1.amazonaws.com/plutoz/pricedepo-dotnet-backend:latest`
4. upload tagged image: `docker push 965295834176.dkr.ecr.eu-central-1.amazonaws.com/plutoz/pricedepo-dotnet-backend:latest`