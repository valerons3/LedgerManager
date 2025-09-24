FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/LedgerManager.API/*.csproj src/LedgerManager.API/
COPY src/LedgerManager.Application/*.csproj src/LedgerManager.Application/
COPY src/LedgerManager.Infrastructure/*.csproj src/LedgerManager.Infrastructure/
COPY src/LedgerManager.Persistence/*.csproj src/LedgerManager.Persistence/

RUN dotnet restore src/LedgerManager.API/LedgerManager.API.csproj

COPY src/. .

WORKDIR /src/LedgerManager.API
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
EXPOSE 8080
ENTRYPOINT ["dotnet", "LedgerManager.API.dll"]