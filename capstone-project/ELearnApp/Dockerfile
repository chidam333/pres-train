FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:3fcf6f1e809c0553f9feb222369f58749af314af6f063f389cbd2f913b4ad556 AS build
WORKDIR /App

# Install openssl to generate certificates
RUN apt-get update && apt-get install -y openssl

# Generate self-signed certificate
RUN mkdir /https && \
    openssl req -x509 -nodes -days 365 \
      -newkey rsa:2048 \
      -keyout /https/aspnetcore.key \
      -out /https/aspnetcore.crt \
      -subj "/CN=localhost" && \
    openssl pkcs12 -export \
      -out /https/aspnetcore.pfx \
      -inkey /https/aspnetcore.key \
      -in /https/aspnetcore.crt \
      -passout pass:password

# Copy everything
COPY . ./
# Restore as distinct layers - target only the main project instead of the solution
RUN dotnet restore ELearnApp.csproj
# Build and publish a release
RUN dotnet publish ELearnApp.csproj -o out
RUN dotnet dev-certs https --trust

# # Build runtime image
# FROM mcr.microsoft.com/dotnet/aspnet:9.0@sha256:b4bea3a52a0a77317fa93c5bbdb076623f81e3e2f201078d89914da71318b5d8
# WORKDIR /App
# COPY --from=build /App/out .
EXPOSE 5243
# ENTRYPOINT ["dotnet", "ELearnApp.dll"]
CMD ["dotnet", "run", "--urls", "https://*:5243"]