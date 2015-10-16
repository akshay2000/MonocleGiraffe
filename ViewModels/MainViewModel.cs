﻿using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
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

        private ObservableCollection<SharpImgur.Models.Topic> topics = new ObservableCollection<SharpImgur.Models.Topic>();
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

        private async void LoadTopics()
        {
            var topicsList = await Topic.GetDefaultTopics();
            Topics = new ObservableCollection<SharpImgur.Models.Topic>(topicsList);
        }
    }
}
