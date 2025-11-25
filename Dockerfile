# ============================
# Stage 1: Build
# ============================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy everything
COPY . .

# Restore dependencies
RUN dotnet restore RestaurantApp.csproj

# Publish project
RUN dotnet publish RestaurantApp.csproj -c Release -o /app

# ============================
# Stage 2: Runtime
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy build output
COPY --from=build /app .

# Expose port
EXPOSE 8080

# Set ASP.NET port
ENV ASPNETCORE_URLS=http://+:8080

# Run application
ENTRYPOINT ["dotnet", "RestaurantApp.dll"]
