# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /eCommerceMicroservice

# Copy solution and project files
COPY eCommerce.ApiGatewaySol/eCommerce.ApiGatewaySol.sln ./eCommerce.ApiGatewaySol/
COPY eCommerce.ApiGatewaySol/ApiGateway.Presentation/ApiGateway.Presentation.csproj ./eCommerce.ApiGatewaySol/ApiGateway.Presentation/
COPY eCommerce.SharedLibraSol/eCommerceSharedLibrary/eCommerceSharedLibrary.csproj ./eCommerce.SharedLibraSol/eCommerceSharedLibrary/

# Restore NuGet packages
RUN dotnet restore eCommerce.ApiGatewaySol/eCommerce.ApiGatewaySol.sln

# Copy all project source code
COPY . .

# Build and publish the WebAPI (ApiGateway.Presentation)
WORKDIR /eCommerceMicroservice/eCommerce.ApiGatewaySol/ApiGateway.Presentation
RUN dotnet publish ApiGateway.Presentation.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish . 

# Copy chứng chỉ vào container
COPY eCommerce.ApiGatewaySol/certs/aspnetapp.pfx /https/aspnetapp.pfx

# Thiết lập HTTPS với chứng chỉ .pfx
ENV ASPNETCORE_URLS=https://+:5003
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=123456

EXPOSE 5003

ENTRYPOINT ["dotnet", "ApiGateway.Presentation.dll"]
