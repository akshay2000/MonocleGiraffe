"{
  `"Client_Id`": `"$env:CLIENT_ID`",
  `"Mashape_Key`": `"$env:MASHAPE_KEY`",
  `"Client_Secret`": `"$env:CLIENT_SECRET`",
  `"Ad_Config`" :
  {
	`"App_Key`": `"$env:ADDUPLEX_APP_KEY`",
	`"Banner_Id`": `"$env:ADDUPLEX_BANNER_ID`"
  }
}" | Out-File -FilePath .\MonocleGiraffe\MonocleGiraffe\Secrets.json

Write-Output "Operating from source directory $env:APPCENTER_SOURCE_DIRECTORY"

$inPath = '.\MonocleGiraffe\MonocleGiraffe.sln'
# $lines = Get-Content -Path .\input.txt
# Clear-Content $inPath
# $uid = "undefined"
# for ($i = 0; $i -lt $lines.Count; $i++) {
#     $line = $lines[$i]
#     if ($uid -eq "undefined" -and $line -match 'MonocleGiraffe.Android') {
#         $split = $line.Split(',')
#         $uid = $split[2].Trim().Trim('"')
#         $i++
#         continue
#     } elseif ($line -match $uid) {
#         continue
#     } else {
#         Add-Content $inPath $line
#     }
# }

Write-Output "Contents of SLN\n\n"

Get-Content $inPath