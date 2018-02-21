$ErrorActionPreference = "Stop"
Write-Output "Build: Build started"

Push-Location $PSScriptRoot

if(Test-Path .\artifacts) {
	Write-Output "Build: Cleaning .\artifacts"
	Remove-Item .\artifacts -Force -Recurse
}

& dotnet restore --no-cache

$branch = @{ $true = $env:APPVEYOR_REPO_BRANCH; $false = $(git symbolic-ref --short -q HEAD) }[$env:APPVEYOR_REPO_BRANCH -ne $NULL];
$revision = @{ $true = "{0:00000}" -f [convert]::ToInt32("0" + $env:APPVEYOR_BUILD_NUMBER, 10); $false = "local" }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
$suffix = @{ $true = ""; $false = "$($branch.Substring(0, [math]::Min(10,$branch.Length)))-$revision"}[$branch -eq "master" -and $revision -ne "local"]
$commitHash = $(git rev-parse --short HEAD)
$buildSuffix = @{ $true = "$($suffix)-$($commitHash)"; $false = "$($branch)-$($commitHash)" }[$suffix -ne ""]

Write-Output "Build: Package version suffix is $suffix"
Write-Output "Build: Build version suffix is $buildSuffix" 

foreach ($src in ls src/*) {
    Push-Location $src

	Write-Output "Build: Building project in $src"

    if([string]::IsNullOrWhiteSpace($buildSuffix))
    {
        & dotnet build -c Release
    }
    else
    {
        & dotnet build -c Release --version-suffix=$buildSuffix
    }

    if([string]::IsNullOrWhiteSpace($suffix))
    {
        & dotnet pack -c Release --include-symbols -o ..\..\artifacts --no-build
    }
    else
    {
        & dotnet pack -c Release --include-symbols -o ..\..\artifacts --version-suffix=$suffix --no-build
    }

    if($LASTEXITCODE -ne 0) { exit 1 }    

    Pop-Location
}

foreach ($test in ls test/*.Tests) {
    Push-Location $test

	Write-Output "Build: Testing project in $test"

    & dotnet test -c Release
    if($LASTEXITCODE -ne 0) { exit 3 }

    Pop-Location
}

Write-Output "Build: Build complete"

Pop-Location