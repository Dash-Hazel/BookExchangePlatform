FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder
WORKDIR /src

COPY BookExchangePlatform/ ./BookExchangePlatform/
WORKDIR /src/BookExchangePlatform
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=builder /app/out ./out
ENTRYPOINT ["dotnet", "out/BookExchangePlatform.dll"]
