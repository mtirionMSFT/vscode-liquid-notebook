# build the LiquidParser first
$root = (Resolve-Path .)
$parserProject = "./src/LiquidParser/LiquidParser/LiquidParser.csproj"
$extensionFolder = "./src/extension"
$extensionDistFolder = (Join-Path $extensionFolder "dist") # distribution folder of the VSCode extension
$outFolder = (New-Item -Name "$extensionDistFolder/LiquidParser" -ItemType Directory -Force).ToString();

# build the Windows executable
dotnet publish $parserProject -c Release -r win-x64 /p:PublishSingleFile=true /p:CopyOutputSymbolsToPublishDirectory=false --self-contained false -o $outFolder
# build the Linux executable
dotnet publish $parserProject -c Release -r linux-x64 /p:PublishSingleFile=true /p:CopyOutputSymbolsToPublishDirectory=false --self-contained true -o $outFolder
# set the executable flag for Linux (chmod +x)
icacls $outFolder\LiquidParser /grant:r "users:(RX)" /c
# remove PDF-files from the output folder. Not needed.
Remove-Item $outFolder\*.pdb
Remove-Item $outFolder\*.xml

# now we'll build and package the VSCode extension
Set-Location $extensionFolder
npm install
npm run compile
$packageVersion = (gitversion /showvariable MajorMinorPatch)
$packageName = ("vscode-liquid." + $packageVersion + ".vsix")
$package = (Join-Path $root $packageName)
vsce package --out $package

Set-Location $root