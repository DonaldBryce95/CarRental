FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY CarRental.Api/CarRental.Api.csproj CarRental.Api/
COPY CarRental.Application/CarRental.Application.csproj CarRental.Application/
COPY CarRental.Domain/CarRental.Domain.csproj CarRental.Domain/
COPY CarRental.Infrastructure/CarRental.Infrastructure.csproj CarRental.Infrastructure/

RUN dotnet restore CarRental.Api/CarRental.Api.csproj

COPY . .

RUN dotnet publish CarRental.Api/CarRental.Api.csproj --configuration Release --output /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CarRental.Api.dll"]