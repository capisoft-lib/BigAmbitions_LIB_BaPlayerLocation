param(
    [Parameter(Mandatory = $true)]
    [string] $SdkPath,

    [switch] $Force
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path $PSScriptRoot -Parent
$dest = Join-Path $SdkPath "Assets\Mods\LIB_BaPlayerLocation"

if (-not (Test-Path (Join-Path $SdkPath "Assets\Mods"))) {
    throw "Not a Big Ambitions SDK project: '$SdkPath' (missing Assets\Mods)."
}

if ((Test-Path $dest) -and -not $Force) {
    throw "Destination already exists: $dest. Use -Force to replace."
}

if (Test-Path $dest) {
    Remove-Item $dest -Recurse -Force
}

$exclude = @(".git", "tools", "templates", "Output", "bin", "obj")
Get-ChildItem $repoRoot -Force | Where-Object { $exclude -notcontains $_.Name } | ForEach-Object {
    Copy-Item $_.FullName -Destination $dest -Recurse -Force
}

Write-Host "Installed LIB_BaPlayerLocation -> $dest"
Write-Host "Next: open SDK in Unity 2022.3.62f2, then Mod Builder -> Build & Install."
