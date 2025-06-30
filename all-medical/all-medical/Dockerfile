FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./
WORKDIR /app/all-medical
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/all-medical/out ./
EXPOSE 5003
ENV ASPNETCORE_URLS=http://+:5003
ENTRYPOINT ["dotnet", "all-medical.dll"]
