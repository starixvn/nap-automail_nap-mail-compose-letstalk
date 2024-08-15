FROM hub.nexdev.net/base/dotnet/core/sdk:6.0 AS builder
WORKDIR /src

COPY ./Fanex.ComposeLetstalkMessage.Service/Fanex.ComposeLetstalkMessage.Business ./Fanex.ComposeLetstalkMessage.Business
COPY ./Fanex.ComposeLetstalkMessage.Service/Fanex.ComposeLetstalkMessage.Service/Fanex.ComposeLetstalkMessage.Service.csproj ./Fanex.ComposeLetstalkMessage.Service/Fanex.ComposeLetstalkMessage.Service.csproj

RUN ls /src -l
WORKDIR /src/Fanex.ComposeLetstalkMessage.Service
RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN dotnet restore -v n

COPY ./Fanex.ComposeLetstalkMessage.Service/Fanex.ComposeLetstalkMessage.Service .

RUN dotnet publish -r linux-x64 --self-contained false -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:6.0-alpine

ENV TZ="America/La_Paz" 

WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "Fanex.ComposeLetstalkMessage.Service.dll"]
