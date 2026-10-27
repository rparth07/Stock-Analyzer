FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Stock Analyzer/", "./"]
RUN dotnet restore "Stock Analyzer/Stock Analyzer.csproj"
RUN dotnet publish "Stock Analyzer/Stock Analyzer.csproj" -c Release --no-restore -o /out
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /out .
ENV ASPNETCORE_HTTP_PORTS=10000
EXPOSE 10000
USER $APP_UID
ENTRYPOINT ["dotnet", "Stock Analyzer.dll"]
