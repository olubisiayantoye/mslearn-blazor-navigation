# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# SQLite folder (Render free tier uses /tmp)
RUN mkdir -p /tmp
ENV SQLITE_PATH=/tmp/pizza.db

EXPOSE 10000
ENTRYPOINT ["dotnet", "BlazingPizza.dll"]
