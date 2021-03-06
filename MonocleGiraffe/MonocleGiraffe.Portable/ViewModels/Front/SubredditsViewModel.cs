﻿using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MonocleGiraffe.Portable.Helpers;
using MonocleGiraffe.Portable.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MonocleGiraffe.Portable.Helpers.Initializer;

namespace MonocleGiraffe.Portable.ViewModels.Front
{
    public class SubredditsViewModel : BindableBase
    {
        private readonly INavigationService navigationService;
        public SubredditsViewModel(INavigationService nav, bool isDesignMode)
        {
            navigationService = nav;
            if (isDesignMode)
                InitDesignTime();
            else
                Init();
        }

        private void Init()
        {
            LoadSubreddits();
        }

        private void InitDesignTime()
        {
            LoadSubredditsDesignTime();
        }

        private ObservableCollection<SubredditItem> subreddits;
        public ObservableCollection<SubredditItem> Subreddits
        {
            get { return subreddits; }
            set { Set(ref subreddits, value); }
        }

        private const string subredditsFileName = "subreddits.json";
        public async void LoadSubreddits()
        {
            string jsonString = await RoamingDataHelper.GetText(subredditsFileName);
            var subredditsList = JArray.Parse(jsonString).ToObject<List<SubredditItem>>();
            if (subredditsList.Count == 0)
            {
                subredditsList = new List<SubredditItem>() {
                    new SubredditItem { Title = "Funny", Url = "funny" },
                    new SubredditItem { Title = "Pictures", Url = "pics" },
                    new SubredditItem { Title = "WTF?!", Url = "funny" },
                    new SubredditItem { Title = "The cutest things on the internet", Url = "aww" },
                    new SubredditItem { Title = "Cats", Url = "cats" },
                    new SubredditItem { Title = "EarthPorn", Url = "earthporn" },
                    new SubredditItem { Title = "Dank Memes", Url = "adviceanimals" }
                };            
            }
            Subreddits = new ObservableCollection<SubredditItem>(subredditsList);
        }

        RelayCommand<SubredditItem> toggleFavorite;
        public RelayCommand<SubredditItem> ToggleFavorite
           => toggleFavorite ?? (toggleFavorite = new RelayCommand<SubredditItem>(async (SubredditItem parameter) =>
           {
               if (parameter.IsFavorited)
                   await RemoveSubreddit(parameter);
               else
                   await AddSubreddit(parameter);
           }));

        public async Task AddSubreddit(SubredditItem subreddit)
        {
            subreddit.IsFavorited = true;
            Subreddits.Insert(0, subreddit);
            await SaveSubreddits();
        }

        public async Task RemoveSubreddit(SubredditItem subreddit)
        {
            subreddit.IsFavorited = false;
            Subreddits.Remove(Subreddits.Where(s => s.Url == subreddit.Url).First());
            await SaveSubreddits();            
        }

        private async Task SaveSubreddits()
        {
            string text = JsonConvert.SerializeObject(Subreddits);
            await RoamingDataHelper.StoreText(text, subredditsFileName);
        }

        RelayCommand<SubredditItem> goToSub;
        public RelayCommand<SubredditItem> GoToSub
           => goToSub ?? (goToSub = new RelayCommand<SubredditItem>((SubredditItem parameter) =>
           {
               NavigateToSub(parameter);
           }));

        protected void NavigateToSub(SubredditItem sub)
        {
            string navigationParamName = "Subreddit";
            StateHelper.SessionState[navigationParamName] = sub;
            navigationService.NavigateTo(PageKeyHolder.SubGalleryPageKey, navigationParamName);            
        }
        
        protected virtual void LoadSubredditsDesignTime()
        {
            Subreddits = new ObservableCollection<SubredditItem>();
            Subreddits.Add(new SubredditItem { Title = "Funny", Url = "funny" });
            Subreddits.Add(new SubredditItem { Title = "Pictures", Url = "pics" });
            Subreddits.Add(new SubredditItem { Title = "WTF?!", Url = "funny" });
            Subreddits.Add(new SubredditItem { Title = "The cutest things on the internet", Url = "aww" });
            Subreddits.Add(new SubredditItem { Title = "Cats", Url = "cats" });
            Subreddits.Add(new SubredditItem { Title = "EarthPorn", Url = "earthporn" });
            Subreddits.Add(new SubredditItem { Title = "Dank Memes", Url = "adviceanimals" });
        }
    }
}
