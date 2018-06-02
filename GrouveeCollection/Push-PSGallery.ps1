$targetPath = "$PSScriptRoot\bin\Release\GrouveeCollection"
$manifestPath = "$targetPath\GrouveeCollection.psd1"

$nuGetApiKey = Read-Host "Please enter your nuget api key"
if([string]::IsNullOrWhiteSpace($nuGetApiKey))
{
	Write-Error "No NuGet Api Key was provided."
	exit
}
	
Publish-Module -NuGetApiKey $nuGetApiKey -Path $targetPath
Read-Host
