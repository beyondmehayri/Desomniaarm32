param([string]$Version = "")

# Build application components
. "./buildApplication.ps1"

# Build Installer
. "./buildSetup.ps1" -Version $Version

Remove-Item -Path $components -Recurse -Force

exit 0

#Start-Sleep -Seconds 1;