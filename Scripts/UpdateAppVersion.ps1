Param(
    [String]$projectFolder,
    [String]$buildId
)

  
if (-Not $projectfolder) {
    Write-Host "no project folder set"
    exit 1
}

if (-Not $buildId) {
    Write-Host "no build Id set"
    exit 1
}

  
$manifestfile = Get-Item -Path "$projectFolder\Package.appxmanifest"
  
$manifestXml = New-Object -TypeName System.Xml.XmlDocument
$manifestXml.Load($manifestfile.Fullname)

$currentVersion = [Version]$manifestXml.Package.Identity.Version
$updatedVersion = [Version]($currentVersion.Major.ToString() + '.' + $currentVersion.Minor + '.' + $currentVersion.Build + '.' + $buildId)

$manifestXml.Package.Identity.Version = [String]$updatedVersion
$manifestXml.save($manifestfile.FullName)