# ============================================================
# Contoso Support API — Multi-stage Dockerfile
# ============================================================

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files first (layer caching)
COPY Contoso.Support.slnx ./
COPY src/Contoso.Support.Api/Contoso.Support.Api.csproj src/Contoso.Support.Api/
COPY src/Contoso.Support.Agents/Contoso.Support.Agents.csproj src/Contoso.Support.Agents/
COPY src/Contoso.Support.Application/Contoso.Support.Application.csproj src/Contoso.Support.Application/
COPY src/Contoso.Support.Domain/Contoso.Support.Domain.csproj src/Contoso.Support.Domain/
COPY src/Contoso.Support.Infrastructure/Contoso.Support.Infrastructure.csproj src/Contoso.Support.Infrastructure/
COPY src/Contoso.Support.RAG/Contoso.Support.RAG.csproj src/Contoso.Support.RAG/
COPY src/Contoso.Support.Tools/Contoso.Support.Tools.csproj src/Contoso.Support.Tools/
COPY src/Contoso.Support.Workers/Contoso.Support.Workers.csproj src/Contoso.Support.Workers/
COPY src/Contoso.Support.ServiceDefaults/Contoso.Support.ServiceDefaults.csproj src/Contoso.Support.ServiceDefaults/
COPY src/Contoso.Support.AppHost/Contoso.Support.AppHost.csproj src/Contoso.Support.AppHost/

# Restore (cached unless .csproj files change)
RUN dotnet restore src/Contoso.Support.Api/Contoso.Support.Api.csproj

# Copy everything else and build
COPY src/ src/
RUN dotnet publish src/Contoso.Support.Api/Contoso.Support.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080

# Non-root user for security
USER $APP_UID

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Contoso.Support.Api.dll"]
