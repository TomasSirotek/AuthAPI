FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app
EXPOSE 80

#Copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

#Copy all else and build 
COPY . ./
RUN dotnet publish -c Release -o out

##Gen image
FROM mcr.microsoft.com/dotnet/core/sdk:2.2
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet","ProductAPI.dll"]

