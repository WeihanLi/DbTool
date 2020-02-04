[string]$SCRIPT       = '.\build.cake'
 
# Install cake.tool
dotnet tool install --global cake.tool

# Start Cake
[string]$CAKE_ARGS = "-verbosity=diagnostic"

Write-Host "dotnet cake $SCRIPT $CAKE_ARGS $ARGS" -ForegroundColor GREEN
 
dotnet cake $SCRIPT $CAKE_ARGS $ARGS