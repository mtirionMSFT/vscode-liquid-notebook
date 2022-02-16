$project = "./LiquidParser/LiquidParser/LiquidParser.csproj"
$outputDirectory = "./extension/dist" # dist for publishing, out for development
$outFolder = (New-Item -Name "$outputDirectory/LiquidParser" -ItemType Directory -Force).ToString();

dotnet publish $project -c Release -r win-x64 /p:PublishSingleFile=true /p:CopyOutputSymbolsToPublishDirectory=false /p:GenerateDocumentationFile=false --self-contained false -o $outFolder
dotnet publish $project -c Release -r linux-x64 /p:PublishSingleFile=true /p:CopyOutputSymbolsToPublishDirectory=false /p:GenerateDocumentationFile=false --self-contained true -o $outFolder
icacls $outFolder\LiquidParser /grant:r "users:(RX)" /c

Remove-Item $outFolder\*.pdb