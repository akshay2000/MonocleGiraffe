using MonocleGiraffe.Models;
using SharpImgur.APIWrappers;
using SharpImgur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.UI;
using Windows.UI.Xaml.Navigation;

namespace MonocleGiraffe.ViewModels
{
    public class EditItemPageViewModel : ViewModelBase
    {
        public EditItemPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                InitDesignTime();
            }
            else
            {
                Init();
            }
        }

        private void Init()
        {
            
        }

        IGalleryItem item = default(IGalleryItem);
        public IGalleryItem Item { get { return item; } set { Set(ref item, value); } }
        
        bool isAlbum = default(bool);
        public bool IsAlbum { get { return isAlbum; } set { Set(ref isAlbum, value); } }
        
        bool isBusy = default(bool);
        public bool IsBusy { get { return isBusy; } set { Set(ref isBusy, value); } }

        DelegateCommand saveCommand;
        public DelegateCommand SaveCommand
           => saveCommand ?? (saveCommand = new DelegateCommand(async () =>
           {
               await Save();
           }, () => true));

        private async Task Save()
        {
            IsBusy = true;
            if (IsAlbum)
            {

            }
            else
            {
                await Images.UpdateImage(Item.Id, Item.Title, Item.Description);
                NavigationService.GoBack();
            }
            IsBusy = false;
        }

        DelegateCommand cancelCommand;
        public DelegateCommand CancelCommand
           => cancelCommand ?? (cancelCommand = new DelegateCommand(() =>
           {
               Item.Title = OldTitle;
               Item.Description = OldDescription;
               NavigationService.GoBack();
           }, () => true));

        private string OldTitle { get; set; }
        private string OldDescription { get; set; }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (state.Any())
            {
                // restore state
                state.Clear();
            }
            else
            {
                Item = (IGalleryItem)BootStrapper.Current.SessionState[(string)parameter];
                OldTitle = Item.Title;
                OldDescription = Item.Description;
                IsAlbum = Item.ItemType == GalleryItemType.Album;
                return base.OnNavigatedToAsync(parameter, mode, state);
            }
            return Task.CompletedTask;
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                // save state
            }
            return Task.CompletedTask;
        }

        public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            return Task.CompletedTask;
        }

        private void InitDesignTime()
        {
            IsAlbum = false;
            Item = new GalleryItem(new Image { Title = "Paper Wizard", Animated = false, Link = "http://i.imgur.com/7xfC5B0.jpg", Views = 73474, Description = "Lorem Ipsum dolor sit amet" });
        }
    }
}
