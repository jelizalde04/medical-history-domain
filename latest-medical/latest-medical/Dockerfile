FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . ./

RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

RUN adduser --disabled-password --gecos "" appuser

WORKDIR /app

COPY --from=build /app/publish ./

RUN chown -R appuser /app

USER appuser

EXPOSE 5004

ENV ASPNETCORE_URLS=http://+:5004
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "latest-medical.dll"]
