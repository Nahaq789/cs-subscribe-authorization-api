FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app/src/Authorization.API
COPY ["src/Authorization.API/Authorization.API.csproj", "./"]
RUN dotnet restore "Authorization.API.csproj"
COPY src/Authorization.API .
RUN dotnet build "Authorization.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Authorization.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authorization.API.dll"]