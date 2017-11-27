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
Copy-Item .\MonocleGiraffe\MonocleGiraffe\Secrets.json .\MonocleGiraffe\MonocleGiraffe.Android\Assets\Secrets.json
"{}" | Out-File -FilePath .\MonocleGiraffe\MonocleGiraffe.Android\Assets\Analytics.json