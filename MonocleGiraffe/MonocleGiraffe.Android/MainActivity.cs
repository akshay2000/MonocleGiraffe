using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MonocleGiraffe.Android.LibraryImpl;
using Android.Content.Res;
using System.IO;
using XamarinImgur.Helpers;

namespace MonocleGiraffe.Android
{
    [Activity(Label = "Monocle Giraffe", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        SettingsHelper helper;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            helper = new SettingsHelper(this);

            // Get our button from the layout resource,
            // and attach an event to it
            Button logInButton = FindViewById<Button>(Resource.Id.LogInButton);

            logInButton.Click += LogInButton_Click;

            Init();               
        }

        private void Init()
        {
            string secrets = LoadSecretsFile();
            Initializer.Init(new AuthBroker(this), new Vault(), new SettingsHelper(this), secrets, () => new HttpClient(), false);
        }

        private void LogInButton_Click(object sender, EventArgs e)
        {
            const string authUrl = "https://api.imgur.com/oauth2/authorize";
            const string callback = "http://localhost:8080/MonocleGiraffeAndroid";
            var result = Initializer.AuthBroker.AuthenticateAsync(new Uri(authUrl), new Uri(callback));
        }

        private string LoadSecretsFile()
        {
            string content;
            AssetManager assets = this.Assets;
            using (StreamReader sr = new StreamReader(assets.Open("Secrets.json")))
            {
                content = sr.ReadToEnd();
            }
            return content;
        }

        //private void SaveButton_Click(object sender, EventArgs e)
        //{
        //    var editText = FindViewById<EditText>(Resource.Id.EditText);
        //    helper.SetLocalValue("MyText", editText.Text);
        //}

        //private void GetButton_Click(object sender, EventArgs e)
        //{
        //    var text = helper.GetLocalValue<string>("MyText", "YOYO!");
        //    var textView = FindViewById<TextView>(Resource.Id.TextView);
        //    textView.Text = text;
        //}
    }
}

