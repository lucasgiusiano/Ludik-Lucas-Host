FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish WebApi/WebApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
# Las imágenes semilla no se publican por defecto (no son Content en el csproj)
COPY --from=build /src/WebApi/SeedArchivos ./SeedArchivos
EXPOSE 8080
ENTRYPOINT ["dotnet", "WebApi.dll"]
