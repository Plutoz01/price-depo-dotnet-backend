FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY ./PriceDepo.csproj ./PriceDepo/
RUN dotnet restore ./PriceDepo/PriceDepo.csproj

# copy everything else and build app
COPY ./. ./PriceDepo/
WORKDIR /app/PriceDepo
RUN dotnet publish -c Release -o out

# create runtime env
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/PriceDepo/out ./
ENTRYPOINT ["dotnet", "PriceDepo.dll"]
