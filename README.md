# Grouvee Collection Powershell Module

A powershell module that uses the [Grouvee Collection Parser library](https://github.com/mmuffins/) to parse .csv files generated when exporting a collection from grouvee.com. Also available on [PowerShell Gallery](https://www.powershellgallery.com/packages/GrouveeCollection).

## Installation
### Automated
Follow the instructions on [PowerShell Gallery](https://www.powershellgallery.com/packages/GrouveeCollection). Or run the following command
```powershell
Install-Module -Name GrouveeCollection 
```

### Manual
Compile the project and run the following command in the build target directory

```powershell
Import-Module -Name .\GrouveeCollection.psd1
Get-Command -Module GrouveeCollection
```


