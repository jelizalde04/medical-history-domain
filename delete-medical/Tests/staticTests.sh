#!/bin/bash

echo "==> Analizando sintaxis y formato del microservicio..."

# Ir al directorio raíz del proyecto
cd "$(dirname "$0")/.."

# Buscar y formatear todos los archivos .csproj encontrados
for proj in $(find . -name "*.csproj"); do
    echo "📁 Formateando proyecto: $proj"
    dotnet format "$proj"
done

echo "==> Análisis completado ✅"
