# Monocle Giraffe
Most awesom Imgur client for Windows and Android.

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
Place a file called `Secrets.json` at root of your Windows project and add the content above.

###Android App
Create a file called `Secrets.json` inside `Assets` directory with the content above.

##I have a better design!
That's great. We have separate repository for design templates and resources. Please make a pull request in [this](https://github.com/akshay2000/MonocleGiraffeDesign) repository. All design input is welcome - especially for Android.
