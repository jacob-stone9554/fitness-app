FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY fitness.api/fitness.api/fitness.api.csproj fitness.api/fitness.api/
RUN dotnet restore fitness.api/fitness.api/fitness.api.csproj

COPY . ./
  
RUN dotnet publish fitness.api/fitness.api/fitness.api.csproj -c Release -o /app/publish /p:UseAppHost=false
  
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
  
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "fitness.api.dll"]