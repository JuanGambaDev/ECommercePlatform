# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar el archivo de proyecto y restaurar las dependencias
COPY UserService.csproj .
RUN dotnet restore

# Copiar el resto de los archivos y compilar el proyecto
COPY . .

# Ejecutar la publicación del proyecto y almacenar los archivos en '/app/publish'
RUN dotnet publish -c Release -o /app/publish

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados desde la etapa de construcción
COPY --from=build /app/publish .

# Exponer los puertos
EXPOSE 5240
EXPOSE 5241

# Configurar el comando de inicio
ENTRYPOINT ["dotnet", "UserService.dll"]
