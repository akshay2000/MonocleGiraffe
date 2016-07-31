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
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Helpers;
using System.Collections.Generic;
using MonocleGiraffe.Portable.ViewModel;

namespace MonocleGiraffe.Android
{
    [Activity(Label = "Monocle Giraffe", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : ActivityBase
    {
        int count = 1;
        private readonly List<Binding> bindings = new List<Binding>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            bindings.Add(this.SetBinding(() => Vm.SampleText, () => SampleTextView.Text));

            LogInButton.SetCommand("Click", Vm.NavigateCommand);
        }

        private MainViewModel Vm
        {
            get
            {
                return App.Locator.Main;
            }
        }

        private Button logInButton;
        public Button LogInButton
        {
            get
            {
                logInButton = logInButton ?? FindViewById<Button>(Resource.Id.LogInButton);
                return logInButton;
            }
        }

        private TextView sampleTextView;
        public TextView SampleTextView
        {
            get
            {
                sampleTextView = sampleTextView ?? FindViewById<TextView>(Resource.Id.TextView);
                return sampleTextView;
            }
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

