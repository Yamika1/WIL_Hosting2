FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["ReCenterHub/ReCenterHub.csproj", "ReCenterHub/"]

RUN dotnet restore "ReCenterHub/ReCenterHub.csproj"

COPY . .

WORKDIR "/src/ReCenterHub"

RUN dotnet publish "ReCenterHub.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "ReCenterHub.dll"]