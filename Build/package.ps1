$packageId = "TypeScripter"
$baseDir  = resolve-path ..
$buildDir = "$baseDir\Build"
$sourceDir = "$baseDir\Src"
$toolsDir = "$baseDir\Tools"
$packageDir = "$baseDir\Package"
$builds = @(
    @{Name = "$packageId.Core"; BinDir="netstandard1.6"},
    @{Name = "$packageId"; BinDir="net45"}
)

Set-Location $baseDir
 
# Delete Package Folder
if (Test-Path -path $packageDir)
{
    & del $packageDir -Recurse -Force
}
 
# Create Package Folder
Write-Host "Creating package directory $packageDir"
New-Item -Path $packageDir -ItemType Directory

#Package 
$nuspecPath = "$packageDir\$packageId.nuspec"
Copy-Item -Path "$buildDir\$packageId.nuspec" -Destination $nuspecPath -recurse
    
foreach ($build in $builds)
{
    if ($build.BinDir)
    {
        $finalDir = $build.BinDir
        robocopy "$sourceDir\$packageId\bin\Release\$finalDir" $packageDir\lib\$finalDir *.dll *.pdb *.xml /NFL /NDL /NJS /NC /NS /NP /XO | Out-Default
    }
}
    
Write-Host "Building NuGet package $packageId" -ForegroundColor Green
Write-Host

& "$toolsDir\NuGet\NuGet.exe" pack $nuspecPath -Symbols
move -Path .\*.nupkg -Destination $packageDir