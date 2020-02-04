#!/bin/sh
SCRIPT='./build.cake'

# Install  cake.tool
dotnet tool install --global cake.tool
export PATH="$PATH:$HOME/.dotnet/tools"

# Start Cake
CAKE_ARGS="$SCRIPT -verbosity=diagnostic"

echo "dotnet cake $CAKE_ARGS $@"

dotnet cake $CAKE_ARGS "$@"