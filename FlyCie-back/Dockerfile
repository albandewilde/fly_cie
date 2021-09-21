FROM mcr.microsoft.com/dotnet/sdk as builder

WORKDIR /app

COPY . .

WORKDIR /app/FlyCie.Webhost/

RUN dotnet restore

RUN dotnet publish -c Release -o /app/out


FROM mcr.microsoft.com/dotnet/aspnet:3.1

WORKDIR /app

COPY --from=builder /app/out .

CMD ["dotnet", "FlyCie.Webhost.dll"]
