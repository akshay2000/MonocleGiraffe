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
using MonocleGiraffe.Portable.ViewModels.Front;
using GalaSoft.MvvmLight.Helpers;

namespace MonocleGiraffe.Android.Fragments
{
    public class AccountFragment : global::Android.Support.V4.App.Fragment
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
            return inflater.Inflate(Resource.Layout.Front_Account, container, false);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            bindings.Add(this.SetBinding(() => Vm.UserName, () => UserNameTextView.Text));
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            bindings.ForEach((b) => b.Detach());
        }

        public AccountViewModel Vm { get { return App.Locator.Front.AccountVM; } }

        private TextView userNameTextView;
        public TextView UserNameTextView
        {
            get
            {
                userNameTextView = userNameTextView ?? View.FindViewById<TextView>(Resource.Id.UserNameTextView);
                return userNameTextView;
            }
        }        
    }
}