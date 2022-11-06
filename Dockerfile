FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PickemBot/PickemBot.csproj", "PickemBot/"]
RUN dotnet restore "PickemBot/PickemBot.csproj"
COPY . .
WORKDIR "/src/PickemBot"
RUN dotnet build "PickemBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PickemBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PickemBot.dll"]
