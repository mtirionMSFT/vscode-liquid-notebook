function UpdatePackageJson {
    param($packagePath, $version)
    $packageJson = Get-Content $packagePath -Raw
    $packageJson = [Regex]::Replace($packageJson, '("version"\s+:\s+"[\d.]+")', '"version": "$version"')
    [System.IO.File]::WriteAllLines($packagePath, $packageJson)
    #$packageJson | Set-Content $packagePath -Force -Encoding UTF8
}