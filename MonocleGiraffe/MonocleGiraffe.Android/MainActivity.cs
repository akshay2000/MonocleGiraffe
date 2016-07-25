using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MonocleGiraffe.Android.LibraryImpl;

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
            Button saveButton = FindViewById<Button>(Resource.Id.SaveButton);
            Button getButton = FindViewById<Button>(Resource.Id.GetButton);

            saveButton.Click += SaveButton_Click;
            getButton.Click += GetButton_Click;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var editText = FindViewById<EditText>(Resource.Id.EditText);
            helper.SetLocalValue("MyText", editText.Text);
        }

        private void GetButton_Click(object sender, EventArgs e)
        {
            var text = helper.GetLocalValue<string>("MyText", "YOYO!");
            var textView = FindViewById<TextView>(Resource.Id.TextView);
            textView.Text = text;
        }
    }
}

