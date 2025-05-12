# 1. Image .NET SDK để build app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# 2. Copy file .csproj và restore dependencies
COPY DTC_BE.csproj ./
RUN dotnet restore

# 3. Copy toàn bộ source code và publish
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# 4. Image ASP.NET để chạy app
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# 5. Chạy file .dll chính
ENTRYPOINT ["dotnet", "DTC_BE.dll"]