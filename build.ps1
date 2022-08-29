# Include settings and common functions
$scriptRoot = $($MyInvocation.MyCommand.Definition) | Split-Path
. "$scriptRoot/tools/config.ps1"
. "$scriptRoot/tools/common.ps1"

Write-Host "Building version $version"

# build the Windows executable
dotnet publish $parser.project -c Release -r win-x64 /p:PublishSingleFile=true /p:CopyOutputSymbolsToPublishDirectory=false --self-contained false -o $parser.outDir
# build the Linux executable
dotnet publish $parser.project -c Release -r linux-x64 /p:PublishSingleFile=true /p:CopyOutputSymbolsToPublishDirectory=false --self-contained true -o $parser.outDir
# set the executable flag for Linux (chmod +x)
icacls $parser.linuxExe /grant:r "users:(RX)" /c
# remove PDF-files from the output folder. Not needed.
Remove-Item "$($parser.outDir)\*.pdb"
Remove-Item "$($parser.outDir)\*.xml"

# now we'll build and package the VSCode extension
Set-Location $extension.projDir

# Change the version in package.json
UpdatePackageJson $extension.config $version

npm install
npm run compile
$package = (Join-Path $homeDir $extension.packageName)
vsce package --out $package
Write-Host "Published package in $($package)"

Set-Location $homeDir