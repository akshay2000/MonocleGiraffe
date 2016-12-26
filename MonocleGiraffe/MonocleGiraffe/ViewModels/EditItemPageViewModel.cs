using MonocleGiraffe.Models;
using XamarinImgur.APIWrappers;
using XamarinImgur.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using MonocleGiraffe.Portable.Models;

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
            CurrentSelection = new List<GalleryItem>();
        }

        IGalleryItem item = default(IGalleryItem);
        public IGalleryItem Item { get { return item; } set { Set(ref item, value); } }

        bool isAlbum = default(bool);
        public bool IsAlbum { get { return isAlbum; } set { Set(ref isAlbum, value); } }

        bool isBusy = default(bool);
        public bool IsBusy { get { return isBusy; } set { Set(ref isBusy, value); } }

        string title = default(string);
        public string Title { get { return title; } set { Set(ref title, value); } }

        string description = default(string);
        public string Description { get { return description; } set { Set(ref description, value); } }

        int albumPrivacyIndex = default(int);
        public int AlbumPrivacyIndex { get { return albumPrivacyIndex; } set { Set(ref albumPrivacyIndex, value); } }

        ObservableCollection<GalleryItem> albumImages = default(ObservableCollection<GalleryItem>);
        public ObservableCollection<GalleryItem> AlbumImages { get { return albumImages; } set { Set(ref albumImages, value); } }

        GalleryItem coverImage = default(GalleryItem);
        public GalleryItem CoverImage { get { return coverImage; } set { Set(ref coverImage, value); } }

        public async void CoverDropped(object sender, object args)
        {
            var e = (DragEventArgs)args;
            var def = e.GetDeferral();
            var imageId = await e.DataView.GetTextAsync();
            CoverImage = AlbumImages.FirstOrDefault(i => i.Id == imageId) ?? AlbumImages[0];
        }

        public void RemoveImages(object sender, object args)
        {
            var count = CurrentSelection.Count;
            for (int i = 0; i < count; i++)
            {
                AlbumImages.Remove(CurrentSelection[0]);
            }
            if (CurrentSelection.Count != 0)
                CurrentSelection.Clear();
            var cover = AlbumImages.FirstOrDefault(i => i.Id == CoverImage.Id);
            if (cover == null)
                CoverImage = AlbumImages[0];
        }

        private List<GalleryItem> CurrentSelection { get; set; }
        public void ChangeSelection(object sender, object args)
        {
            var p = (args as Windows.UI.Xaml.Controls.SelectionChangedEventArgs);
            CurrentSelection.AddRange(p.AddedItems.Select(g => (GalleryItem)g));
            foreach (var g in p.RemovedItems)
            {
                CurrentSelection.Remove((GalleryItem)g);
            }
        }

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
                var album = (AlbumItem)Item;
                string[] imageIds = AlbumImages.Select(i => i.Id).ToArray();
                imageIds = !imageIds.SequenceEqual(album.AlbumImages.Select(i => i.Id).ToArray()) ? imageIds : null;
                string title = Title != album.Title ? Title : null;
                string description = Description != album.Description ? Description : null;
                string privacy = ToAlbumPrivacy(AlbumPrivacyIndex);
                privacy = privacy != album.Privacy ? privacy : null;
                string cover = CoverImage.Id != album.Cover ? CoverImage.Id : null;
                var response = await Portable.Helpers.Initializer.Albums.UpdateAlbum(album.Id, imageIds, title, description, privacy, cover);
            }
            else
            {
                await Portable.Helpers.Initializer.Images.UpdateImage(Item.Id, Title, Description);
            }
            NavigationService.GoBack();
            IsBusy = false;
        }

        DelegateCommand cancelCommand;
        public DelegateCommand CancelCommand
           => cancelCommand ?? (cancelCommand = new DelegateCommand(() =>
           {
               NavigationService.GoBack();
           }, () => true));


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
                IsAlbum = Item.ItemType == GalleryItemType.Album;
                Title = Item.Title;
                Description = Item.Description;
                if (IsAlbum)
                {
                    var album = (AlbumItem)Item;
                    AlbumPrivacyIndex = ToIndex(album.Privacy);
                    AlbumImages = new ObservableCollection<GalleryItem>(album.AlbumImages);
                    CoverImage = album.AlbumImages.First(s => album.Cover == s.Id);
                }
                return base.OnNavigatedToAsync(parameter, mode, state);
            }
            return Task.CompletedTask;
        }

        private int ToIndex(string albumPrivacy)
        {
            switch (albumPrivacy)
            {
                case "public":
                    return 0;
                case "hidden":
                    return 1;
                case "secret":
                default:
                    return 2;
            }
        }

        private string ToAlbumPrivacy(int index)
        {
            switch (index)
            {
                case 0:
                    return "public";
                case 1:
                    return "hidden";
                case 2:
                default:
                    return "secret";
            }
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
            IsAlbum = true;
            Item = new GalleryItem(new Image { Title = "Paper Wizard", Animated = false, Link = "http://i.imgur.com/7xfC5B0.jpg", Views = 73474, Description = "Lorem Ipsum dolor sit amet" });
        }      
    }
}
