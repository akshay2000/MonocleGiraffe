using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpImgur.APIWrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MonocleGiraffe.ViewModels
{
    public class MainViewModel : NotifyBase
    {
        private const string subredditsFileName = "subreddits.json";

        public MainViewModel()
        {
            //LoadTopics();
        }

        private string galleryTitle;
        public string GalleryTitle
        {
            get
            {
                return galleryTitle;
            }
            set
            {
                if (galleryTitle != value)
                {
                    galleryTitle = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (selectedIndex != value)
                {
                    selectedIndex = value;
                    //NotifyPropertyChanged();
                    //CalculateZoomFactor();
                }
            }
        }        

        //private float zoomFactor;
        //public float ZoomFactor
        //{
        //    get
        //    {
        //        if (zoomFactor == 0)
        //            return 0.5F;
        //        return zoomFactor;
        //    }
        //    set
        //    {
        //        if (zoomFactor != value)
        //        {
        //            zoomFactor = value;
        //            NotifyPropertyChanged();
        //        }
        //    }
        //}

        //private void CalculateZoomFactor()
        //{
        //    GalleryItem item = ImageItems[SelectedIndex];
        //    if (ViewPortHeight == 0 || ViewPortWidth == 0 || item.Width == 0 || item.Height == 0)
        //        return;

        //    float newZoomFactor = (float)Math.Min(ViewPortWidth / item.Width, ViewPortHeight / item.Height);
        //    ZoomFactor = Math.Min(1.0F, newZoomFactor);
        //}

        //private double viewPortWidth;
        //public double ViewPortWidth
        //{
        //    get { return viewPortWidth; }
        //    set
        //    {
        //        if (viewPortWidth != value)
        //        {
        //            viewPortWidth = value;
        //            CalculateZoomFactor();
        //            NotifyPropertyChanged();
        //        }
        //    }
        //}

        //private double viewPortHeight;
        //public double ViewPortHeight
        //{
        //    get { return viewPortHeight; }
        //    set
        //    {
        //        if (viewPortHeight != value)
        //        {
        //            viewPortHeight = value;
        //            CalculateZoomFactor();
        //            NotifyPropertyChanged();
        //        }
        //    }
        //}

        private ObservableCollection<GalleryItem> imageItems;
        public ObservableCollection<GalleryItem> ImageItems
        {
            get
            {
                return imageItems;
            }
            set
            {
                if (imageItems != value)
                {
                    imageItems = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private ObservableCollection<SharpImgur.Models.Topic> topics;// = new ObservableCollection<SharpImgur.Models.Topic>();
        public ObservableCollection<SharpImgur.Models.Topic> Topics
        {
            get
            {                
                return topics;
            }
            set
            {
                if (topics != value)
                {
                    topics = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private ObservableCollection<SubredditItem> subreddits;
        public ObservableCollection<SubredditItem> Subreddits
        {
            get { return subreddits; }
            set
            {
                if (subreddits != value)
                {
                    subreddits = value;
                    NotifyPropertyChanged();
                }
            }
        }
        
        public async void LoadTopics()
        {
            var topicsList = (await SharpImgur.APIWrappers.Topics.GetDefaultTopics()).Content;
            Topics = new ObservableCollection<SharpImgur.Models.Topic>(topicsList);
        }

        public async void LoadSubreddits()
        {
            string jsonString = await RoamingDataHelper.GetText(subredditsFileName);
            var subredditsList = JArray.Parse(jsonString).ToObject<List<SubredditItem>>();
            if (subredditsList.Count == 0)
            {
                subredditsList = new List<SubredditItem>() {
                    //new Subreddit("earthporn", "EarthPorn"),
                    //new Subreddit("funny", "Funny"),
                    //new Subreddit("pics", "Pics"),
                    //new Subreddit("gifs", "GIFs"),
                    //new Subreddit("aww", "AWW")
                };
            }
            Subreddits = new ObservableCollection<SubredditItem>(subredditsList);
        }

        public void AddSubreddit(SubredditItem subreddit)
        {
            Subreddits.Insert(0, subreddit);
            SaveSubreddits();
        }

        public void RemoveSubreddit(SubredditItem subreddit)
        {
            Subreddits.Remove(subreddit);
            SaveSubreddits();
            if(subreddit.Title == GalleryTitle)
            {
                LoadGallery();
            }
        }

        internal async void SaveSubreddits()
        {
            string text = JsonConvert.SerializeObject(Subreddits);
            await RoamingDataHelper.StoreText(text, subredditsFileName);
        }

        public async void LoadGallery()
        {
            //ImageItems = new ObservableCollection<GalleryItem>();
            //GalleryTitle = "Gallery";
            //var gallery = await Gallery.GetGallery( Enums.Section.Top);
            //foreach (var image in gallery)
            //{
            //    var gItem = new GalleryItem(image);
            //    Debug.Write("Title = \"" + gItem.Title + "\", ");
            //    //Debug.Write(gItem.Description + ",");
            //    Debug.Write("IsAnimated = \"" + gItem.IsAnimated + "\", ");
            //    Debug.Write("Link = \"" + gItem.Link + "\", ");
            //    Debug.Write("UploaderName = \"" + gItem.UploaderName + "\", ");
            //    Debug.WriteLine("Mp4 = \"" + gItem.Mp4 + "\"");
            //    ImageItems.Add(gItem);
            //}
        }

        public async void LoadSubreddit(SubredditItem subreddit)
        {
            //ImageItems = new ObservableCollection<GalleryItem>();
            //GalleryTitle = subreddit.FriendlyName;
            //var subredditGallery = await Gallery.GetSubreddditGallery(subreddit.ActualName);
            //foreach (var image in subredditGallery)
            //{
            //    ImageItems.Add(new GalleryItem(image));
            //}
        }

        public async void LoadTopic(SharpImgur.Models.Topic topic)
        {
            //ImageItems = new ObservableCollection<GalleryItem>();
            //GalleryTitle = topic.Name;
            //var topicGallery = await SharpImgur.APIWrappers.Topics.GetTopicGallery(topic.Id);
            //foreach (var image in topicGallery)
            //{
            //    ImageItems.Add(new GalleryItem(image));
            //}
        }
    }
}
