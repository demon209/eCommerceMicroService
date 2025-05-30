# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /eCommerceMicroservice

# Copy solution and project files
COPY eCommerce.AuthenticationApiSol/eCommerce.AuthenticationApiSol.sln ./eCommerce.AuthenticationApiSol/
COPY eCommerce.AuthenticationApiSol/AuthenticationApi.Application/AuthenticationApi.Application.csproj ./eCommerce.AuthenticationApiSol/AuthenticationApi.Application/
COPY eCommerce.AuthenticationApiSol/AuthenticationApi.Domain/AuthenticationApi.Domain.csproj ./eCommerce.AuthenticationApiSol/AuthenticationApi.Domain/
COPY eCommerce.AuthenticationApiSol/AuthenticationApi.Infrastructure/AuthenticationApi.Infrastructure.csproj ./eCommerce.AuthenticationApiSol/AuthenticationApi.Infrastructure/
COPY eCommerce.AuthenticationApiSol/AuthenticationApi.Presentation/AuthenticationApi.Presentation.csproj ./eCommerce.AuthenticationApiSol/AuthenticationApi.Presentation/
COPY eCommerce.SharedLibraSol/eCommerceSharedLibrary/eCommerceSharedLibrary.csproj ./eCommerce.SharedLibraSol/eCommerceSharedLibrary/

# Restore NuGet packages
RUN dotnet restore eCommerce.AuthenticationApiSol/eCommerce.AuthenticationApiSol.sln

# Copy all project source code
COPY . .

# Build and publish the WebAPI (AuthenticationApi.Presentation)
WORKDIR /eCommerceMicroservice/eCommerce.AuthenticationApiSol/AuthenticationApi.Presentation
RUN dotnet publish AuthenticationApi.Presentation.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000
ENTRYPOINT ["dotnet", "AuthenticationApi.Presentation.dll"]
