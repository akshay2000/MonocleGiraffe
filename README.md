# Monocle Giraffe
Most awesome Imgur client for Windows and Android.  
  
<a href="https://www.microsoft.com/store/apps/9nblggh4qcvh?ocid=badge"><img height="104px" src="https://assets.windowsphone.com/f2f77ec7-9ba9-4850-9ebe-77e366d08adc/English_Get_it_Win_10_InvariantCulture_Default.png" alt="Get it on Windows 10" /></a> <a href='https://play.google.com/store/apps/details?id=in.indestructible.monoclegiraffe&utm_source=global_co&utm_medium=prtnr&utm_content=Mar2515&utm_campaign=PartBadge&pcampaignid=MKT-Other-global-all-co-prtnr-py-PartBadge-Mar2515-1'><img height="104px" alt='Get it on Google Play' src='https://play.google.com/intl/en_us/badges/images/generic/en_badge_web_generic.png'/></a>

##What is this?
This is a Windows UWP and Xamarin Android app for Imgur. You're welcome to make a fork and send some pull requests. You're also welcome to use the code and build an app (or a car). Stay in touch; I'd like to know that someone is using the code.

##How to use this?
Clone this repo. It contains code for the app. It also contains code for for C# Imgur SDK in project XamarinImgur. You will need to provide your own Client Id and secret for consuming the Imgur API. Those can be obtained from [Imgur API documentation](http://api.imgur.com/#registerapp). Following template shows how app expects secrets. Follow the instructions for plarform of your choice.

    {
        "Client_Id": "client-id",
        "Mashape_Key": "mashape-key",
        "Client_Secret": "client-secret"
    }

###Windows App
Place a file called `Secrets.json` at root of your Windows project and add the content above. You will need Visual Studio 2015 with latest copy of Windows SDK tools installed to build this project.

###Android App
Create a file called `Secrets.json` inside `Assets` directory with the content above. You will need Visual Studio 2015 with latest Xamarin Andorid installed and configured. Alternatively, you may also use Xamarin Studio, but I haven't tested it personally.

##I have a better design!
That's great. We have a separate repository for design templates and resources. Please make a pull request in [this](https://github.com/akshay2000/MonocleGiraffeDesign) repository. All design input is welcome - especially for Android.
