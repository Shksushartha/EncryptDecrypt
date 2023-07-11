FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./encryptDecryptMVC/encryptDecryptMVC.csproj", "encryptDecryptMVC/"]
RUN dotnet restore "encryptDecryptMVC/encryptDecryptMVC.csproj"
COPY . .
WORKDIR "/src/encryptDecryptMVC"
RUN dotnet build "encryptDecryptMVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "encryptDecryptMVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "encryptDecryptMVC.dll"]