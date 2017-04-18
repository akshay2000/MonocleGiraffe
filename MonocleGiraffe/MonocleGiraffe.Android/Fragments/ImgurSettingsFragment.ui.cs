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

namespace MonocleGiraffe.Android.Fragments
{
    public partial class ImgurSettingsFragment
    {
        private View authLayout;
        public View AuthLayout
        {
            get
            {
                authLayout = authLayout ?? Activity.FindViewById<LinearLayout>(Resource.Id.AuthLayout);
                return authLayout;
            }
        }

        private EditText bioView;
        public EditText BioView
        {
            get
            {
                bioView = bioView ?? Activity.FindViewById<EditText>(Resource.Id.BioEditText);
                return bioView;
            }
        }

        private Switch imagePublicSwitch;
        public Switch ImagePublicSwitch
        {
            get
            {
                imagePublicSwitch = imagePublicSwitch ?? Activity.FindViewById<Switch>(Resource.Id.ImagePublicSwitch);
                return imagePublicSwitch;
            }
        }

        private Switch enableMessagingSwitch;
        public Switch EnableMessagingSwitch
        {
            get
            {
                enableMessagingSwitch = enableMessagingSwitch ?? Activity.FindViewById<Switch>(Resource.Id.EnableMessagingSwitch);
                return enableMessagingSwitch;
            }
        }

        private Spinner albumPrivacySpinner;
        public Spinner AlbumPrivacySpinner
        {
            get
            {
                albumPrivacySpinner = albumPrivacySpinner ?? Activity.FindViewById<Spinner>(Resource.Id.AlbumPrivacySpinner);
                return albumPrivacySpinner;
            }
        }

        private Switch matureContentSwitch;
        public Switch MatureContentSwitch
        {
            get
            {
                matureContentSwitch = matureContentSwitch ?? Activity.FindViewById<Switch>(Resource.Id.MatureContentSwitch);
                return matureContentSwitch;
            }
        }

        private Button saveSettingsButton;
        public Button SaveSettingsButton
        {
            get
            {
                saveSettingsButton = saveSettingsButton ?? Activity.FindViewById<Button>(Resource.Id.SaveSettingsButton);
                return saveSettingsButton;
            }
        }

        private Button signOutButton;
        public Button SignOutButton
        {
            get
            {
                signOutButton = signOutButton ?? Activity.FindViewById<Button>(Resource.Id.SignOutButton);
                return signOutButton;
            }
        }

        private Button signInButton;
        public Button SignInButton
        {
            get
            {
                signInButton = signInButton ?? Activity.FindViewById<Button>(Resource.Id.SignInButton);
                return signInButton;
            }
        }
    }
}