using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using MonocleGiraffe.Portable.ViewModels;
using Android.Content.Res;
using System.IO;
using XamarinImgur.Helpers;
using MonocleGiraffe.Android.LibraryImpl;
using GalaSoft.MvvmLight.Views;

namespace MonocleGiraffe.Android.Activities
{
    [Activity(Label = "SplashScreen", MainLauncher = true, Icon = "@drawable/icon", NoHistory = true)]
    public class SplashScreen : ActivityBase
    {
        private readonly List<Binding> bindings = new List<Binding>();

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Splash);
            Init();
            SetBindings();
            await Vm.ShakeHandsAndNavigate();
        }

        private void SetBindings()
        {
            bindings.Add(Vm.SetBinding(() => Vm.State, this,
                () => ProgressRing.Visibility, BindingMode.OneWay)
                .ConvertSourceToTarget((string state) =>
                state == "Busy" ? ViewStates.Visible : ViewStates.Invisible));
            bindings.Add(Vm.SetBinding(() => Vm.Message, this,
                () => MessageText.Text, BindingMode.OneWay));
            bindings.Add(Vm.SetBinding(() => Vm.State, this,
                () => SignInButton.Visibility, BindingMode.OneWay)
                .ConvertSourceToTarget((string state) =>
                state == "AuthError" ? ViewStates.Visible : ViewStates.Invisible));
            bindings.Add(Vm.SetBinding(() => Vm.State, this,
                () => RetryButton.Visibility, BindingMode.OneWay)
                .ConvertSourceToTarget((string state) =>
                state == "AnonError" ? ViewStates.Visible : ViewStates.Invisible));
            RetryButton.Click += async delegate { await Vm.ShakeHandsAndNavigate(); };
            SignInButton.Click += async delegate { await Vm.SignInAndNavigate(); };
        }

        private void Init()
        {
            string secrets = LoadSecretsFile();
            Initializer.Init(new AuthBroker(this), new Vault(), new SettingsHelper(this), secrets, () => new HttpClient(), false);
            Portable.Helpers.Initializer.Init(new RoamingDataHelper(), new SharingHelper(), new ClipboardHelper());
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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            bindings.ForEach((b) => b.Detach());
        }

        public SplashViewModel Vm { get { return App.Locator.Splash; } }

        private ProgressBar progressRing;
        public ProgressBar ProgressRing
        {
            get
            {
                progressRing = progressRing ?? FindViewById<ProgressBar>(Resource.Id.ProgressRing);
                return progressRing;
            }
        }

        private TextView messageText;
        public TextView MessageText
        {
            get
            {
                messageText = messageText ?? FindViewById<TextView>(Resource.Id.MessageTextView);
                return messageText;
            }
        }

        private Button signInButton;
        public Button SignInButton
        {
            get
            {
                signInButton = signInButton ?? FindViewById<Button>(Resource.Id.SignInButton);
                return signInButton;
            }
        }

        private Button retryButton;
        public Button RetryButton
        {
            get
            {
                retryButton = retryButton ?? FindViewById<Button>(Resource.Id.RetryButton);
                return retryButton;
            }
        }
    }
}