# This file generates Secrets.json at the given path
Param(
    [string]$outPath,
    [string]$clientId,
    [string]$mashApeKey,
    [string]$clientSecret
)
"{
  `"Client_Id`": `"$clientId`",
  `"Mashape_Key`": `"$mashApeKey`",
  `"Client_Secret`": `"$clientSecret`"
}" | Out-File -FilePath $outPath