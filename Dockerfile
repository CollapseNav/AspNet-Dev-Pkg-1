FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS http://+:5000
ENV TZ=Asia/Shanghai

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS publish
WORKDIR /src
COPY ./ ./
RUN dotnet publish AspNet.Dev.Pkg.Demo -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AspNet.Dev.Pkg.Demo.dll"]
