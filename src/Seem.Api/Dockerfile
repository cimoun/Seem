FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Seem.Api/Seem.Api.csproj", "Seem.Api/"]
COPY ["Seem.Application/Seem.Application.csproj", "Seem.Application/"]
COPY ["Seem.Infrastructure/Seem.Infrastructure.csproj", "Seem.Infrastructure/"]
COPY ["Seem.Domain/Seem.Domain.csproj", "Seem.Domain/"]
RUN dotnet restore "Seem.Api/Seem.Api.csproj"
COPY . .
WORKDIR "/src/Seem.Api"
RUN dotnet build "Seem.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Seem.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Seem.Api.dll"]
