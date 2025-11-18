# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# SQLite folder
RUN mkdir -p /tmp
ENV SQLITE_PATH=/tmp/app.db

# Expose port 10000 (Render will map HTTP automatically)
EXPOSE 10000

ENTRYPOINT ["dotnet", "mslearn-blazor-navigation.dll"]
