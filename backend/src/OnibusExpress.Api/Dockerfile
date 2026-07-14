FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["OnibusExpress.sln", "."]
COPY ["src/OnibusExpress.Api/OnibusExpress.Api.csproj", "src/OnibusExpress.Api/"]
COPY ["src/OnibusExpress.Application/OnibusExpress.Application.csproj", "src/OnibusExpress.Application/"]
COPY ["src/OnibusExpress.Domain/OnibusExpress.Domain.csproj", "src/OnibusExpress.Domain/"]
COPY ["src/OnibusExpress.Infrastructure/OnibusExpress.Infrastructure.csproj", "src/OnibusExpress.Infrastructure/"]
RUN dotnet restore "src/OnibusExpress.Api/OnibusExpress.Api.csproj"

COPY . .
WORKDIR "/src/src/OnibusExpress.Api"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OnibusExpress.Api.dll"]