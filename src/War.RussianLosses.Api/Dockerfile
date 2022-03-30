#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ARG POSTGRE_CONNECTION
ENV ConnectionStrings__Postgre=$POSTGRE_CONNECTION

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/War.RussianLosses.Api/War.RussianLosses.Api.csproj", "src/War.RussianLosses.Api/"]
RUN dotnet restore "src/War.RussianLosses.Api/War.RussianLosses.Api.csproj"
COPY . .
WORKDIR "/src/src/War.RussianLosses.Api"
RUN dotnet build "War.RussianLosses.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "War.RussianLosses.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "War.RussianLosses.Api.dll"]