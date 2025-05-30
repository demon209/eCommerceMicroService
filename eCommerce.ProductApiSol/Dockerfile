# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /eCommerceMicroservice

# Copy solution and project files
COPY eCommerce.ProductApiSol/eCommerce.ProductApiSol.sln ./eCommerce.ProductApiSol/
COPY eCommerce.ProductApiSol/ProductApi.Application/ProductApi.Application.csproj ./eCommerce.ProductApiSol/ProductApi.Application/
COPY eCommerce.ProductApiSol/ProductApi.Domain/ProductApi.Domain.csproj ./eCommerce.ProductApiSol/ProductApi.Domain/
COPY eCommerce.ProductApiSol/ProductApi.Infrastructure/ProductApi.Infrastructure.csproj ./eCommerce.ProductApiSol/ProductApi.Infrastructure/
COPY eCommerce.ProductApiSol/ProductApi.Presentation/ProductApi.Presentation.csproj ./eCommerce.ProductApiSol/ProductApi.Presentation/
COPY eCommerce.ProductApiSol/UnitTest.ProductApi/UnitTest.ProductApi.csproj ./eCommerce.ProductApiSol/UnitTest.ProductApi/
COPY eCommerce.SharedLibraSol/eCommerceSharedLibrary/eCommerceSharedLibrary.csproj ./eCommerce.SharedLibraSol/eCommerceSharedLibrary/


# Restore NuGet packages
RUN dotnet restore eCommerce.ProductApiSol/eCommerce.ProductApiSol.sln


# Copy all project source code
COPY . .

# Build and publish the WebAPI (ProductApi.Presentation)
WORKDIR /eCommerceMicroservice/eCommerce.ProductApiSol/ProductApi.Presentation
RUN dotnet publish ProductApi.Presentation.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5001
ENTRYPOINT ["dotnet", "ProductApi.Presentation.dll"]
