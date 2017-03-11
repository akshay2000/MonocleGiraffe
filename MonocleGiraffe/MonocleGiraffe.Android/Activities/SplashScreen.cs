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
using GalaSoft.MvvmLight.Ioc;
using XamarinImgur.Interfaces;
using FFImageLoading;
using Xamarin.Android.Net;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;

namespace MonocleGiraffe.Android.Activities
{
    [Activity(Label = "Monocle Giraffe", MainLauncher = true, NoHistory = true)]
    public class SplashScreen : ActivityBase
    {
        private readonly List<Binding> bindings = new List<Binding>();

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            SetContentView(Resource.Layout.Splash);
            Init();
            SetBindings();
            await Vm.ShakeHandsAndNavigate();
            AnalyticsHelper.SendView("Splash");
        }

        private void SetBindings()
        {
            bindings.Add(this.SetBinding(() => Vm.State,
                () => ProgressRing.Visibility, BindingMode.OneWay)
                .ConvertSourceToTarget((string state) =>
                state == "Busy" ? ViewStates.Visible : ViewStates.Invisible));
            bindings.Add(this.SetBinding(() => Vm.Message,
                () => MessageText.Text, BindingMode.OneWay));
            bindings.Add(this.SetBinding(() => Vm.State,
                () => SignInButton.Visibility, BindingMode.OneWay)
                .ConvertSourceToTarget((string state) =>
                state == "AuthError" ? ViewStates.Visible : ViewStates.Invisible));
            bindings.Add(this.SetBinding(() => Vm.State,
                () => RetryButton.Visibility, BindingMode.OneWay)
                .ConvertSourceToTarget((string state) =>
                state == "AnonError" ? ViewStates.Visible : ViewStates.Invisible));
            RetryButton.Click += async delegate { await Vm.ShakeHandsAndNavigate(); };
            SignInButton.Click += async delegate { await Vm.SignInAndNavigate(); };
        }

		private void ConfigureIoc()
		{
            if (SimpleIoc.Default.IsRegistered<IHttpClient>())
                return;
			SimpleIoc.Default.Register<IHttpClient, HttpClient>();
			SimpleIoc.Default.Register<ISecretsProvider>(() => new SecretsProvider(Assets));
			SimpleIoc.Default.Register<IVault>(() => new Vault(this));
			SimpleIoc.Default.Register<IAuthBroker>(() => new AuthBroker(this, SimpleIoc.Default.GetInstance<ISecretsProvider>()));
			SimpleIoc.Default.Register<ISettingsHelper>(() => new SettingsHelper(this));
			SimpleIoc.Default.Register<AuthenticationHelper>();
			SimpleIoc.Default.Register<SecretsHelper>();

            SimpleIoc.Default.Register<Context>(() => ApplicationContext);
			var authHelper = SimpleIoc.Default.GetInstance<AuthenticationHelper>();
			var secretsHelper = SimpleIoc.Default.GetInstance<SecretsHelper>();
			SimpleIoc.Default.Register<NetworkHelper>(() => new NetworkHelper(authHelper, () => new HttpClient(), secretsHelper));
		}

        private void Init()
        {
            HandlePermissions();
            ConfigureIoc();
            ImageService.Instance.Initialize(new FFImageLoading.Config.Configuration() { HttpClient = new System.Net.Http.HttpClient(new AndroidClientHandler()) });
            Portable.Helpers.Initializer.Init(new RoamingDataHelper(), new SharingHelper(ApplicationContext), new ClipboardHelper(ApplicationContext));
        }

        private void HandlePermissions()
        {
            var permission = ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage);
            if (permission != global::Android.Content.PM.Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage }, 2);
            }
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