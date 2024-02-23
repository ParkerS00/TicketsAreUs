FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y wget
WORKDIR /app 

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebApp/WebApp.csproj", "web/"]
RUN dotnet restore "web/WebApp.csproj"
# COPY ["/RazorClassLib.csproj", "lib/"]
# RUN dotnet restore "lib/RazorClassLib.csproj"

WORKDIR "/src/web"
COPY . .
RUN dotnet build "WebApp/WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApp/WebApp.csproj" -c Release -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "WebApp.dll" ]


