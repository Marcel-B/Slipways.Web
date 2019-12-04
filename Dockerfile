FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Slipways.Web/Slipways.Web.csproj", "Slipways.Web/"]
RUN dotnet restore "Slipways.Web/Slipways.Web.csproj"
COPY . .
WORKDIR "/src/Slipways.Web"
RUN dotnet build "Slipways.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Slipways.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Slipways.Web.dll"]