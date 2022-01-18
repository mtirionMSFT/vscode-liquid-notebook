$project = "../LiquidParser"
$outputDirectory = "dist" # dist for publishing, out for development
$outFolder = (New-Item -Name "$outputDirectory/LiquidParser" -ItemType Directory -Force).ToString();

dotnet build $project -o $outFolder -c Release
