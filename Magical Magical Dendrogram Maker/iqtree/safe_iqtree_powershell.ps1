#!/usr/bin/env pwsh
# IQ-TREE wrapper for Windows PowerShell
# Author: Henry Tang
# Ported from Bash version
# Date: 2025-10-16


param (
    [Parameter(Position = 0, Mandatory = $true)]
    [string]$InputFile,
    [Parameter(Position = 1, ValueFromRemainingArguments = $true)]
    [string[]]$ExtraArgs
)

# Check input
if (-not (Test-Path $InputFile)) {
    Write-Host "Usage: script.ps1 input.fasta [iqtree-options]" -ForegroundColor Red
    exit 1
}

$BaseName = [System.IO.Path]::GetFileNameWithoutExtension($InputFile)
$WorkDir = "tmp_$BaseName"
New-Item -ItemType Directory -Force -Path $WorkDir | Out-Null

$SafeFasta = Join-Path $WorkDir "safe.fasta"
$MapFile   = Join-Path $WorkDir "name_map.tsv"
$TreeOut   = Join-Path (Split-Path $InputFile) "${BaseName}_restored.treefile"

# --- Sanitize FASTA headers ---
$BadChars = '[^A-Za-z0-9_.-]'
$Counter = 1
$Mapping = @{}

$fin = Get-Content $InputFile
$fout = New-Item -Force -Path $SafeFasta -ItemType File
$fmap = New-Item -Force -Path $MapFile -ItemType File

foreach ($line in $fin) {
    if ($line.StartsWith(">")) {
        $orig = $line.Substring(1).Trim()
        if ($orig -match $BadChars) {
            $safe = "TAXON_$Counter"
            $Counter++
        } else {
            $safe = $orig
        }
        $Mapping[$safe] = $orig
        Add-Content $SafeFasta ">$safe"
        Add-Content $MapFile "$safe`t$orig"
    } else {
        Add-Content $SafeFasta $line
    }
}

# --- Step 2: Run IQ-TREE ---
$IQTREE = Join-Path $PSScriptRoot "iqtree3.exe"
$IQTREE_ARGS = "-s `"$SafeFasta`" -pre `"$WorkDir\$BaseName`" -nt AUTO $($ExtraArgs -join ' ')"

Write-Host "Running IQ-TREE..."
$proc = Start-Process -FilePath $IQTREE -ArgumentList $IQTREE_ARGS -NoNewWindow -Wait -PassThru
if ($proc.ExitCode -ne 0) {
    Write-Host "IQ-TREE failed with exit code $($proc.ExitCode)" -ForegroundColor Red
    exit 1
}

# --- Restore original names ---
$Mapping = @{}
Get-Content $MapFile | ForEach-Object {
    $parts = $_ -split "`t"
    if ($parts.Count -eq 2) {
        $Mapping[$parts[0]] = $parts[1]
    }
}

$TreeText = Get-Content "$WorkDir\$BaseName.treefile" -Raw
foreach ($safe in ($Mapping.Keys | Sort-Object Length -Descending)) {
    $TreeText = $TreeText -replace [regex]::Escape($safe), [regex]::Escape($Mapping[$safe])
}

Set-Content -Path $TreeOut -Value $TreeText