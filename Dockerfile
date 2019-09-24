FROM mcr.microsoft.com/dotnet/core/sdk:2.2.402-stretch

ENV DOTNET_CLI_TELEMETRY_OPTOUT=1

RUN mkdir app
WORKDIR /app

# Copy projects files
COPY . .

RUN dotnet build
