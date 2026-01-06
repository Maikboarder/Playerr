#!/bin/bash
set -e

# Define version
VERSION="0.2.2-beta"
OUTPUT_DIR="build_artifacts"

echo "=================================="
echo "Playerr Build Script v$VERSION"
echo "=================================="

# 1. Build Frontend
echo "[1/2] Building Frontend..."
npm install
npm run build

# 2. Build Backend for multiple targets
echo "[2/2] Building Backend executables..."

mkdir -p $OUTPUT_DIR

# Windows x64
echo " -> Building for Windows (x64)..."
dotnet publish src/Playerr.Host/Playerr.Host.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o $OUTPUT_DIR/win-x64
echo "    Copying assets..."
mkdir -p $OUTPUT_DIR/win-x64/_output/UI
cp -R _output/UI/* $OUTPUT_DIR/win-x64/_output/UI/
cp src/Playerr.Host/appsettings.json $OUTPUT_DIR/win-x64/
# Ensure no personal configs are included
rm -f $OUTPUT_DIR/win-x64/config/*.json
rm -f $OUTPUT_DIR/win-x64/settings/*.json


echo "    Packaging Windows..."
cd $OUTPUT_DIR
zip -r -q "Playerr-Windows-x64.zip" "win-x64"
cd ..

# Linux x64
echo " -> Building for Linux (x64)..."
dotnet publish src/Playerr.Host/Playerr.Host.csproj -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -o $OUTPUT_DIR/linux-x64
echo "    Copying assets..."
mkdir -p $OUTPUT_DIR/linux-x64/_output/UI
cp -R _output/UI/* $OUTPUT_DIR/linux-x64/_output/UI/
cp src/Playerr.Host/appsettings.json $OUTPUT_DIR/linux-x64/
# Ensure no personal configs are included
rm -f $OUTPUT_DIR/linux-x64/config/*.json
rm -f $OUTPUT_DIR/linux-x64/settings/*.json

echo "    Packaging Linux..."
cd $OUTPUT_DIR
tar -czf "Playerr-Linux-x64.tar.gz" "linux-x64"
cd ..

# MacOS x64 (Intel) - DISABLED
# echo " -> Building for MacOS (Intel)..."
# dotnet publish src/Playerr.Host/Playerr.Host.csproj -c Release -r osx-x64 --self-contained true -o $OUTPUT_DIR/osx-x64
# echo "    Copying assets..."
# mkdir -p $OUTPUT_DIR/osx-x64/_output/UI
# cp -R _output/UI/* $OUTPUT_DIR/osx-x64/_output/UI/
# cp src/Playerr.Host/appsettings.json $OUTPUT_DIR/osx-x64/
# Ensure no personal configs are included
# rm -f $OUTPUT_DIR/osx-x64/config/*.json
# rm -f $OUTPUT_DIR/osx-x64/settings/*.json

# MacOS arm64 (Apple Silicon)
echo " -> Building for MacOS (Apple Silicon)..."
dotnet publish src/Playerr.Host/Playerr.Host.csproj -c Release -r osx-arm64 --self-contained true -o $OUTPUT_DIR/osx-arm64
echo "    Copying assets..."
mkdir -p $OUTPUT_DIR/osx-arm64/_output/UI
cp -R _output/UI/* $OUTPUT_DIR/osx-arm64/_output/UI/
cp src/Playerr.Host/appsettings.json $OUTPUT_DIR/osx-arm64/
# Ensure no personal configs are included
rm -f $OUTPUT_DIR/osx-arm64/config/*.json
rm -f $OUTPUT_DIR/osx-arm64/settings/*.json

echo "=================================="
echo "Packaging MacOS App..."
./create_mac_app.sh

echo "=================================="
echo "Build Complete!"
echo "Artifacts available in: $OUTPUT_DIR"
echo "=================================="
