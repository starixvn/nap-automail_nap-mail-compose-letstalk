FROM hub.nexdev.net/base/dotnet/core/sdk:9.0-alpine AS builder
WORKDIR /src

COPY ./Fanex.ComposeLetstalkMessage.Service/Fanex.ComposeLetstalkMessage.Business ./Fanex.ComposeLetstalkMessage.Business
COPY ./Fanex.ComposeLetstalkMessage.Service/Fanex.ComposeLetstalkMessage.Service/Fanex.ComposeLetstalkMessage.Service.csproj ./Fanex.ComposeLetstalkMessage.Service/Fanex.ComposeLetstalkMessage.Service.csproj

WORKDIR /src/Fanex.ComposeLetstalkMessage.Service
RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN --mount=type=cache,target=/root/.nuget/packages \
	dotnet restore -v n -r linux-x64

COPY ./Fanex.ComposeLetstalkMessage.Service/Fanex.ComposeLetstalkMessage.Service .

RUN --mount=type=cache,target=/root/.nuget/packages \
	dotnet publish -r linux-x64 --self-contained false -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-nap-mail-core

# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

ENV TZ="America/La_Paz"
ENV COMPlus_EnableDiagnostics=0

WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "Fanex.ComposeLetstalkMessage.Service.dll"]
