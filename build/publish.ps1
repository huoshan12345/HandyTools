function Invoke-Call {
  param (
    [scriptblock]$ScriptBlock,
    [string]$ErrorAction = $ErrorActionPreference
  )
  & @ScriptBlock
  if (($LastExitCode -ne 0) -and $ErrorAction -eq "Stop") {
    Write-Host "press any key to exit"
    Read-Host
    exit $LastExitCode
  }
}

$ErrorActionPreference = "Stop"

$dir = Split-Path -Parent $MyInvocation.MyCommand.Definition
$root = Join-Path $dir "..\"
$src = Join-Path $root "src"
$output = Join-Path $root "publish"
If ((test-path $output)) {
  Remove-Item -Recurse -Force $output
}
New-Item -ItemType Directory -Force -Path $output | Out-Null
Invoke-Call -ScriptBlock { 
  dotnet publish --nologo -v q $root -o $output
}