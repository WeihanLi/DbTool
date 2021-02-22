[string]$SCRIPT = '.\build.cake'
 
# Install cake.tool
dotnet tool install --global cake.tool

# Start Cake
Write-Host "dotnet cake $SCRIPT $ARGS" -ForegroundColor GREEN

dotnet cake $SCRIPT $ARGS