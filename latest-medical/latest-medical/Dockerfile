
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./
WORKDIR /app/latest-medical
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/latest-medical/out ./
EXPOSE 5004
ENV ASPNETCORE_URLS=http://+:5004
ENTRYPOINT ["dotnet", "latest-medical.dll"]
