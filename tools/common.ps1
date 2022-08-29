function UpdatePackageJson {
    param($packagePath, $version)
    $packageJson = Get-Content $packagePath -Raw
    $packageJson = [Regex]::Replace($packageJson, '("version":\s+".+")', """version"": ""$($version)""")
    [System.IO.File]::WriteAllLines($packagePath, $packageJson)
    #$packageJson | Set-Content $packagePath -Force -Encoding UTF8 ==> OUTPUT AS BOM GIVES ERROR
}