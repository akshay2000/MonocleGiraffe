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
using Android.Support.Design.Widget;

namespace MonocleGiraffe.Android.Fragments
{
    public class BrowserSheetFragment : BottomSheetDialogFragment
    {
        public enum MenuItem
        {
            Save,
            Share,
            CopyLink,
            None
        }

        private const string Save = "Save";
        private const string Share = "Share";
        private const string CopyLink = "Copy Link";

        //TODO: Add Save option here after downloads are implemented
        private readonly string[] menuItems = new string[] { Share, CopyLink };

        public override void SetupDialog(Dialog dialog, int style)
        {
            base.SetupDialog(dialog, style);
            View contentView = View.Inflate(Context, Resource.Layout.Sheet_Browser, null);
            var menuList = contentView.FindViewById<ListView>(Resource.Id.MenuListView);
            var a = new ArrayAdapter<string>(Context, global::Android.Resource.Layout.SimpleListItem1, menuItems);
            menuList.Adapter = a;
            menuList.ItemClick += MenuList_ItemClick;
            dialog.SetContentView(contentView);
        }

        public event EventHandler<MenuItem> MenuTapped;

        private void OnMenuTapped(object sender, MenuItem e)
        {
            MenuTapped?.Invoke(sender, e);
        }

        private void MenuList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string selectedEntry = menuItems[e.Position];
            MenuItem item = MenuItem.None;
            switch (selectedEntry)
            {
                case Save:
                    item = MenuItem.Save;
                    break;
                case Share:
                    item = MenuItem.Share;
                    break;
                case CopyLink:
                    item = MenuItem.CopyLink;
                    break;              
                default:
                    break;
            }
            OnMenuTapped(sender, item);
        }
    }
}