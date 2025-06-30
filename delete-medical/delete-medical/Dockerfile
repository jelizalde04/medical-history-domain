FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./
WORKDIR /app/delete-medical
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/delete-medical/out ./
EXPOSE 5005
ENV ASPNETCORE_URLS=http://+:5005
ENTRYPOINT ["dotnet", "delete-medical.dll"]
