# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /eCommerceMicroservice

# Copy solution and project files
COPY eCommerce.OrderApiSol/eCommerce.OrderApiSol.sln ./eCommerce.OrderApiSol/
COPY eCommerce.OrderApiSol/OrderApi.Application/OrderApi.Application.csproj ./eCommerce.OrderApiSol/OrderApi.Application/
COPY eCommerce.OrderApiSol/OrderApi.Domain/OrderApi.Domain.csproj ./eCommerce.OrderApiSol/OrderApi.Domain/
COPY eCommerce.OrderApiSol/OrderApi.Infrastructure/OrderApi.Infrastructure.csproj ./eCommerce.OrderApiSol/OrderApi.Infrastructure/
COPY eCommerce.OrderApiSol/OrderApi.Presentation/OrderApi.Presentation.csproj ./eCommerce.OrderApiSol/OrderApi.Presentation/
COPY eCommerce.OrderApiSol/UnitTest.OrderApi/UnitTest.OrderApi.csproj ./eCommerce.OrderApiSol/UnitTest.OrderApi/
COPY eCommerce.SharedLibraSol/eCommerceSharedLibrary/eCommerceSharedLibrary.csproj ./eCommerce.SharedLibraSol/eCommerceSharedLibrary/


# Restore NuGet packages
RUN dotnet restore eCommerce.OrderApiSol/eCommerce.OrderApiSol.sln


# Copy all project source code
COPY . .

# Build and publish the WebAPI (OrderApi.Presentation)
WORKDIR /eCommerceMicroservice/eCommerce.OrderApiSol/OrderApi.Presentation

RUN dotnet publish OrderApi.Presentation.csproj -c Release -o /app/publish


# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5002
ENTRYPOINT ["dotnet", "OrderApi.Presentation.dll"]
