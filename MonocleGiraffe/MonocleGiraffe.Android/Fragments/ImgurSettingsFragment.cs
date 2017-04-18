using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using MonocleGiraffe.Portable.ViewModels.Settings;

namespace MonocleGiraffe.Android.Fragments
{
    public partial class ImgurSettingsFragment : global::Android.Support.V4.App.Fragment
    {
        List<Binding> bindings = new List<Binding>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
             return inflater.Inflate(Resource.Layout.Settings_Imgur, container, false);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            bindings.Add(this.SetBinding(() => Vm.State, () => SignInButton.Visibility).ConvertSourceToTarget(s => s == "NotAuthenticated" ? ViewStates.Visible : ViewStates.Gone));
            bindings.Add(this.SetBinding(() => Vm.State, () => AuthLayout.Visibility).ConvertSourceToTarget(s => s == "Authenticated" ? ViewStates.Visible : ViewStates.Gone));
            bindings.Add(this.SetBinding(() => Vm.Bio, () => BioView.Text, BindingMode.TwoWay));
            bindings.Add(this.SetBinding(() => Vm.PublicImages, () => ImagePublicSwitch.Checked, BindingMode.TwoWay));
            bindings.Add(this.SetBinding(() => Vm.MessagingEnabled, () => EnableMessagingSwitch.Checked, BindingMode.TwoWay));

            ArrayAdapter adapter = ArrayAdapter.CreateFromResource(Activity, Resource.Array.AlbumPrivacyItems, global::Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(global::Android.Resource.Layout.SimpleSpinnerDropDownItem);
            AlbumPrivacySpinner.Adapter = adapter;

            bindings.Add(this.SetBinding(() => Vm.AlbumPrivacyIndex).WhenSourceChanges(() => AlbumPrivacySpinner.SetSelection(Vm.AlbumPrivacyIndex)));
            AlbumPrivacySpinner.ItemSelected += AlbumPrivacySpinner_ItemSelected;
            bindings.Add(this.SetBinding(() => Vm.ShowMature, () => MatureContentSwitch.Checked, BindingMode.TwoWay));
            SignInButton.SetCommand("Click", Vm.SignInCommand);
            SaveSettingsButton.SetCommand("Click", Vm.SaveCommand);
            SignOutButton.SetCommand("Click", Vm.SignOutCommand);
        }

        private void AlbumPrivacySpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Vm.AlbumPrivacyIndex = e.Position;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            bindings.ForEach(b => b.Detach());
            AlbumPrivacySpinner.ItemSelected -= AlbumPrivacySpinner_ItemSelected;
        }

        public ImgurSettingsViewModel Vm
        {
            get
            {
                return App.Locator.Settings.ImgurSettingsViewModel;
            }
        }
    }
}