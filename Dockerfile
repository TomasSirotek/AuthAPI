FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

#Copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

#Copy all else and build 
COPY . ./
RUN dotnet publish -c Release -o out

##Gen image
FROM mcr.microsoft.com/dotnet/core/sdk:2.2
WORKDIR /app
EXPOSE 5000
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet","ProductAPI.dll"]