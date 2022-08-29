# General configuration settings for running
# the build and package scripts
$homeDir = (Resolve-Path "$PSScriptRoot\..").Path
$version = (gitversion /showvariable MajorMinorPatch)

$extension = @{
    projDir = "$homeDir\src\extension"
    config = "$homeDir\src\extension\package.json"
    distDir = "$homeDir\src\extension\dist"
    packageName = "vscode-liquid.$version.vsix"
}

$parser = @{
    projDir = "$homeDir\src\LiquidParser\LiquidParser"
    project = "$homeDir\src\LiquidParser\LiquidParser\LiquidParser.csproj"
    outDir = "$homeDir\src\extension\dist\LiquidParser"
    linuxExe = "$homeDir\src\extension\dist\LiquidParser\LiquidParser"
}

#$parserProject = "./src/LiquidParser/LiquidParser/LiquidParser.csproj"
#$extensionFolder = "./src/extension"
#$extesionConfig = (Join-Path $extensionFolder "package.json")
#$extensionDistFolder = (Join-Path $extensionFolder "dist") # distribution folder of the VSCode extension
#$outFolder = (New-Item -Name "$extensionDistFolder/LiquidParser" -ItemType Directory -Force).ToString();
