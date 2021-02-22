#!/bin/sh
SCRIPT='./build.cake'

# Install  cake.tool
dotnet tool install --global cake.tool
export PATH="$PATH:$HOME/.dotnet/tools"

# Start Cake
echo "dotnet cake $SCRIPT $@"

dotnet cake $SCRIPT "$@"