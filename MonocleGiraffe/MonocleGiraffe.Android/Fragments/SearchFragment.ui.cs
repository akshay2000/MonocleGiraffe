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
using Android.Support.V7.Widget;

namespace MonocleGiraffe.Android.Fragments
{
    public partial class SearchFragment
    {
        private TextView redditsButton;
        private TextView RedditsButton
        {
            get
            {
                redditsButton = redditsButton ?? View.FindViewById<TextView>(Resource.Id.RedditsButton);
                return redditsButton;
            }
        }

        private TextView postsButton;
        private TextView PostsButton
        {
            get
            {
                postsButton = postsButton ?? View.FindViewById<TextView>(Resource.Id.PostsButton);
                return postsButton;
            }
        }

        private TextView gifsButton;
        private TextView GifsButton
        {
            get
            {
                gifsButton = gifsButton ?? View.FindViewById<TextView>(Resource.Id.GifsButton);
                return gifsButton;
            }
        }

        private EditText queryEditText;
        private EditText QueryEditText
        {
            get
            {
                queryEditText = queryEditText ?? View.FindViewById<EditText>(Resource.Id.QueryEditText);
                return queryEditText;
            }
        }

        private RecyclerView resultsView;
        private RecyclerView ResultsView
        {
            get
            {
                resultsView = resultsView ?? View.FindViewById<RecyclerView>(Resource.Id.ResultsRecyclerView);
                return resultsView;
            }
        }

        private ProgressBar redditsProgress;
        private ProgressBar RedditsProgress
        {
            get
            {
                redditsProgress = redditsProgress ?? View.FindViewById<ProgressBar>(Resource.Id.RedditsProgress);
                return redditsProgress;
            }
        }

        private ProgressBar postsProgress;
        private ProgressBar PostsProgress
        {
            get
            {
                postsProgress = postsProgress ?? View.FindViewById<ProgressBar>(Resource.Id.PostsProgress);
                return postsProgress;
            }
        }

        private ProgressBar gifsProgress;
        private ProgressBar GifsProgress
        {
            get
            {
                gifsProgress = gifsProgress ?? View.FindViewById<ProgressBar>(Resource.Id.GifsProgress);
                return gifsProgress;
            }
        }
    }
}