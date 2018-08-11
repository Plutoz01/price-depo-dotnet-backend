# Price-depo backend

The main purpose of this project is to get deeper knowledge about ASP.Net Core 2.1, C# and related technologies, libraries, common patterns and best practices.

## Prerequisites
1. Pre-installed dotnet CLI 2.1 or higher
2. Installed MongoDB (configurable in `appsettings.json`)

## Getting started
1. Open project root dir
2. Run `dotnet run` command. Application is now listening on `http://localhost:5000`
3. Create a GET HTTP request to `http://localhost:5000/api/product`

## Progress so far:
1. Generics CrudRepository interface
2. Abstract In-memory implementation  for CrudRepository
3. Example implementation called `InMemoryProductCrudRepository`
4. Generics CrudController abstract class + consuming product controller
5. Generics MongoDB CrudRepository abstract class + consuming product repository

## TODO:
2. Dynamic query building logic

# AWS:
in command line:
1. Run command `aws ecr get-login --no-include-email`
2. Copy and run output of previous command
3. Tag latest built image: `docker tag plutoz/pricedepo-dotnet-backend:latest965295834176.dkr.ecr.eu-central-1.amazonaws.com/plutoz/pricedepo-dotnet-backend:latest`
4. upload tagged image: `docker push 965295834176.dkr.ecr.eu-central-1.amazonaws.com/plutoz/pricedepo-dotnet-backend:latest`