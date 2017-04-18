
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
	public class AppSettingsFragment : global::Android.Support.V4.App.Fragment
	{
		private readonly List<Binding> bindings = new List<Binding>();

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			return inflater.Inflate(Resource.Layout.Settings_App, container, false);
		}

		public override void OnActivityCreated(Bundle savedInstanceState)
		{
			base.OnActivityCreated(savedInstanceState);
			bindings.Add(this.SetBinding(() => Vm.IsViralEnabled, () => ShowViralSwitch.Checked, BindingMode.TwoWay));
			bindings.Add(this.SetBinding(() => Vm.IsMatureEnabled, () => ShowMatureSwitch.Checked, BindingMode.TwoWay));
			ShowViralSwitch.CheckedChange += (sender, e) =>
			{
				Vm.ChangeViralEnabled();
			};
			ShowMatureSwitch.CheckedChange += (sender, e) =>
			{
				Vm.ChangeMatureEnabled();
			};
		}

		private Switch showViralSwitch;
		public Switch ShowViralSwitch
		{
			get
			{
				showViralSwitch = showViralSwitch ?? Activity.FindViewById<Switch>(Resource.Id.ShowViralSwitch);
				return showViralSwitch;
			}
		}

		private Switch showMatureSwitch;
		public Switch ShowMatureSwitch
		{
			get
			{
				showMatureSwitch = showMatureSwitch ?? Activity.FindViewById<Switch>(Resource.Id.ShowMatureSwitch);
				return showMatureSwitch;
			}
		}

		public AppSettingsViewModel Vm
		{
			get
			{
				return App.Locator.Settings.AppSettingsViewModel;
			}
		}


		public override void OnDestroy()
		{
			base.OnDestroy();
			bindings.ForEach((b) => b.Detach());
		}
	}
}
