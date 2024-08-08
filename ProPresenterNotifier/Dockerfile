FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER 1000:1000
ENV ASPNETCORE_URLS=http://*:8080
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .
RUN dotnet restore "ProPresenterNotifier/ProPresenterNotifier.csproj"
WORKDIR "/src/ProPresenterNotifier"
RUN dotnet publish "ProPresenterNotifier.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProPresenterNotifier.dll"]
