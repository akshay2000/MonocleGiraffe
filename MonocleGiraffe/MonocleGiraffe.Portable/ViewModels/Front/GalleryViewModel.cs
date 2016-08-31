using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MonocleGiraffe.Portable.Helpers;
using MonocleGiraffe.Portable.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XamarinImgur.APIWrappers;
using XamarinImgur.Interfaces;
using XamarinImgur.Models;
using static XamarinImgur.APIWrappers.Enums;

namespace MonocleGiraffe.Portable.ViewModels.Front
{
    public class GalleryViewModel : BindableBase
    {
        private const string VIRAL = "Viral";
        private const string TIME = "Time";
        private const string POPULAR = "Popular";
        private const string TOP = "Top";
        private const string USER = "User";
        private const string MOST_VIRAL = "MOST VIRAL";
        private const string USER_SUB = "USER SUBMITTED";

        private readonly INavigationService navigationService;

        public GalleryViewModel(INavigationService nav, bool isInDesignMode)
        {
            navigationService = nav;
            if (isInDesignMode)
                InitDesignTime();
            else
                Init();
        }

        private async void Init()
        {
            Title = MOST_VIRAL;
            LoadGallery(POPULAR, VIRAL);
            await Task.Delay(100);
            await LoadTopics();
        }

        public void Reload()
        {
            Topic topic = Topics[TopicSelectedIndex];
            LoadTopicGallery(topic, VIRAL);
        }

        #region Section and Sorting

        private Section ToSection(string sectionString)
        {
            switch (sectionString)
            {
                case POPULAR:
                    return Enums.Section.Hot;
                case TOP:
                    return Enums.Section.Top;
                case USER:
                    return Enums.Section.User;
                default:
                    throw new NotImplementedException($"Section {sectionString} can not be handled");
            }
        }

        private string section;
        public string Section
        {
            get { return section; }
            set { Set(ref section, value); }
        }

        private Enums.Sort ToSort(string sortString)
        {
            switch (sortString)
            {
                case VIRAL:
                    return Enums.Sort.Viral;
                case TIME:
                    return Enums.Sort.Time;
                default:
                    throw new NotImplementedException($"Sort {sortString} cannot be handled");
            }
        }

        private bool isSectionVisible;
        public bool IsSectionVisible
        {
            get { return isSectionVisible; }
            set { Set(ref isSectionVisible, value); }
        }

        RelayCommand<string> sectionCommand;
        public RelayCommand<string> SectionCommand
           => sectionCommand ?? (sectionCommand = new RelayCommand<string>((string parameter) =>
           {
               LoadGallery(parameter, VIRAL);
           }));

        RelayCommand<string> sortCommand;
        public RelayCommand<string> SortCommand
           => sortCommand ?? (sortCommand = new RelayCommand<string>((string parameter) =>
           {
               Topic topic = Topics[TopicSelectedIndex];
               LoadTopicGallery(topic, parameter);
           }));

        #endregion

        private void LoadGallery(string sectionString, string sortString)
        {
            Section = sectionString;
            IsSectionVisible = sectionString != USER;
            Section section = ToSection(sectionString);
            Sort sort = ToSort(sortString);
            Images = CreateGallery(section, sort);
        }

        private async Task LoadTopics()
        {
            var topics = (await XamarinImgur.APIWrappers.Topics.GetDefaultTopics()).Content;
            topics.Insert(0, new Topic { Name = USER_SUB, Description = "Here it begins." });
            topics.Insert(0, new Topic { Name = MOST_VIRAL, Description = "Today's most popular posts." });
            Topics = new ObservableCollection<Topic>(topics);
            TopicSelectedIndex = 1;
            TopicSelectedIndex = 0;
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { Set(ref title, value); }
        }

        private IncrementalGallery images;
        public IncrementalGallery Images
        {
            get { return images; }
            set { Set(ref images, value); }
        }

        private int imageSelectedIndex;
        public int ImageSelectedIndex
        {
            get { return imageSelectedIndex; }
            set { Set(ref imageSelectedIndex, value); }
        }

        protected GalleryMetaInfo galleryMetaInfo;

        public void ImageTapped(int tappedIndex)
        {
            ImageTapped(Images[tappedIndex]);
        }
        
        public void ImageTapped(GalleryItem clickedItem)
        {
            ImageSelectedIndex = Images.IndexOf(clickedItem);
            const string navigationParamName = "GalleryInfo";
            galleryMetaInfo = new GalleryMetaInfo { Gallery = Images, SelectedIndex = ImageSelectedIndex };
            StateHelper.SessionState[navigationParamName] = galleryMetaInfo;
            navigationService.NavigateTo(PageKeyHolder.BrowserPageKey, navigationParamName);
        }

        private bool isPaneOpen;
        public bool IsPaneOpen
        {
            get { return isPaneOpen; }
            set { Set(ref isPaneOpen, value); }
        }

        public void OpenPane()
        {
            IsPaneOpen = true;
        }

        public void ClosePane()
        {
            IsPaneOpen = false;
        }

        private ObservableCollection<Topic> topics = new ObservableCollection<Topic>();
        public ObservableCollection<Topic> Topics
        {
            get { return topics; }
            set { Set(ref topics, value); }
        }

        private int topicSelectedIndex;
        public int TopicSelectedIndex
        {
            get { return topicSelectedIndex; }
            set { Set(ref topicSelectedIndex, value); }
        }   

        public void TopicTapped(Topic clickedItem)
        {
            ClosePane();
            LoadTopicGallery(clickedItem, VIRAL);
        }

        private void LoadTopicGallery(Topic topic, string sortString)
        {
            Title = topic.Name;
            if (topic.Name == MOST_VIRAL)
                LoadGallery(Section, sortString);
            else if (topic.Name == USER_SUB)
                LoadGallery(USER, sortString);
            else
            {
                IsSectionVisible = false;
                Images = CreateGallery(ToSort(sortString), topic.Id);
            }
        }

        protected virtual IncrementalGallery CreateGallery(Sort sort, int topicId)
        {
            return new IncrementalGallery(sort, topicId);
        }

        protected virtual IncrementalGallery CreateGallery(Section section, Sort sort)
        {
            return new IncrementalGallery(section, sort);
        }

        protected virtual void InitDesignTime()
        {
            Title = MOST_VIRAL;
            Section = POPULAR;
            isSectionVisible = true;
            Images = CreateGallery(ToSection(POPULAR), Sort.Viral);
            Images.Add(new GalleryItem(new Image { Title = "Paper Wizard", Animated = true, Link = "http://i.imgur.com/kJYBDHJh.gif", AccountUrl = "AvengeMeKreigerBots", Mp4 = "http://i.imgur.com/kJYBDHJ.mp4", Ups = 73474, CommentCount = 345 }));
            Images.Add(new GalleryItem(new Image { Title = "Upvote baby duck for good luck", Animated = false, Link = "http://i.imgur.com/j1jujAp.jpg", AccountUrl = "Snickletits", Mp4 = "", Ups = 879, CommentCount = 49 }));
            Images.Add(new GalleryItem(new Image { Title = "Slow Cooker Parmesan Honey Pork Roast", Animated = true, Link = "http://i.imgur.com/AhoWKkYh.gif", AccountUrl = "drocks27", Mp4 = "http://i.imgur.com/AhoWKkY.mp4", Ups = 6419, CommentCount = 561 }));
            Images.Add(new GalleryItem(new Image { Title = "my dad's work for over 20 years", Animated = false, Link = "http://imgur.com/a/izwDI", AccountUrl = "fluffybluemarshmallow", Mp4 = "", Ups = 4856, CommentCount = 194 }));
            Images.Add(new GalleryItem(new Image { Title = "Playing with blocks", Animated = true, Link = "http://i.imgur.com/QotYGysh.gif", AccountUrl = "Mikepants", Mp4 = "http://i.imgur.com/QotYGys.mp4" }));
            Images.Add(new GalleryItem(new Image { Title = "Opening the cookie jar", Animated = true, Link = "http://i.imgur.com/wutL0vLh.gif", AccountUrl = "grizzzzzly", Mp4 = "http://i.imgur.com/wutL0vL.mp4" }));
            Images.Add(new GalleryItem(new Image { Title = "Five Years Ago This Lady Rescued, Raised And Released This Wolf Pack.  They Are So Excited To See Her!", Animated = true, Link = "http://i.imgur.com/umzkvy1.gif", AccountUrl = "LindaDee", Mp4 = "http://i.imgur.com/umzkvy1.mp4" }));
            Images.Add(new GalleryItem(new Image { Title = "The new Firefox logo", Animated = true, Link = "http://i.imgur.com/pIfdoIW.gif", AccountUrl = "BOHdiCALis1de", Mp4 = "http://i.imgur.com/pIfdoIW.mp4" }));
            Images.Add(new GalleryItem(new Image { Title = "Witty Political Title that will (hopefully) not be responded to with \"Witty Political Comment\"", Animated = false, Link = "http://imgur.com/a/a2eOY", AccountUrl = "Ayziak", Mp4 = "" }));
            Images.Add(new GalleryItem(new Image { Title = "When the teacher uses your work as an example.", Animated = true, Link = "http://i.imgur.com/UT7oVCG.gif", AccountUrl = "mubi92", Mp4 = "http://i.imgur.com/UT7oVCG.mp4" }));
            Images.Add(new GalleryItem(new Image { Title = "Humanity is doomed", Animated = false, Link = "http://imgur.com/a/YfheQ", AccountUrl = "hotbreadzeke", Mp4 = "" }));
            Images.Add(new GalleryItem(new Image { Title = "He comes from a land down under...", Animated = true, Link = "http://i.imgur.com/vP8LnSw.gif", AccountUrl = "Upvotefairywholikesgifs", Mp4 = "http://i.imgur.com/vP8LnSw.mp4" }));
            Images.Add(new GalleryItem(new Image { Title = "Whaaat?", Animated = true, Link = "http://i.imgur.com/3c3OQJPh.gif", AccountUrl = "netrex", Mp4 = "http://i.imgur.com/3c3OQJP.mp4" }));
            Images.Add(new GalleryItem(new Image { Title = "Nap time is...OVER!", Animated = true, Link = "http://i.imgur.com/mnVpdzjh.gif", AccountUrl = "drbatookhan", Mp4 = "http://i.imgur.com/mnVpdzj.mp4" }));
            Images.Add(new GalleryItem(new Image { Title = "MRW I arrive at a stranger's house party, notice them bickering over what movie to watch, throw on my favorite Jim Carrey flick, and they suddenly demand to know who I am...", Animated = true, Link = "http://i.imgur.com/j57jAzI.gif", AccountUrl = "ForeveraKritik", Mp4 = "http://i.imgur.com/j57jAzI.mp4" }));

            Topics = new ObservableCollection<Topic>();
            Topics.Add(new Topic { Name = MOST_VIRAL, Description = "today's most popular posts" });
            Topics.Add(new Topic { Name = "User Submitted", Description = "brand new posts shared in real time" });
            Topics.Add(new Topic { Name = "Random", Description = "a mix from the imgur archives" });
            Topics.Add(new Topic { Name = "Staff Picks", Description = "great posts picked by imgur staff" });
            Topics.Add(new Topic { Name = "Funny", Description = "if it makes you laugh, you'll find it here" });
        }
    }

    public class IncrementalGallery : IncrementalCollection<GalleryItem>, IJsonizable
    {
        public IncrementalGallery(Sort sort, int topicId)
        {
            IsGallery = false;
            Sort = sort;
            TopicId = topicId;
        }

        public IncrementalGallery(Section section, Sort sort)
        {
            IsGallery = true;
            Section = section;
            Sort = sort;
        }

        public bool IsGallery { get; set; }

        public Section Section { get; private set; }

        public Sort Sort { get; private set; }

        public int TopicId { get; private set; }

        public string toJson()
        {
            JObject ret = new JObject();
            ret["isGallery"] = IsGallery;
            ret["sort"] = JsonConvert.SerializeObject(Sort);
            ret["section"] = JsonConvert.SerializeObject(Sort);
            ret["topicId"] = TopicId;
            return ret.ToString();
        }

        public static IncrementalGallery fromJson(string s)
        {
            JObject o = JObject.Parse(s);
            bool isGallery = (bool)o["isGallery"];
            Sort sort = JsonConvert.DeserializeObject<Sort>((string)o["sort"]);
            if (isGallery)
            {
                Section section = JsonConvert.DeserializeObject<Section>((string)o["section"]);
                return new IncrementalGallery(section, sort);
            }
            else
            {
                return new IncrementalGallery(sort, (int)o["topicId"]);
            }
        }

        protected override bool HasMoreItemsImpl()
        {
            return true;
        }

        protected async override Task<List<GalleryItem>> LoadMoreItemsImplAsync(CancellationToken c, uint page)
        {
            if (IsGallery)
                return await GetGallery(page);
            else
                return await GetTopicGallery(page);
        }

        private async Task<List<GalleryItem>> GetGallery(uint page)
        {
            List<Image> gallery;
            if (Section == Section.User)
            {
                bool showViral = Settings.GetValue<bool>("IsViralEnabled", true);
                gallery = (await Gallery.GetGallery(Section, Sort, (int)page, showViral)).Content;
            }
            else
            {
                gallery = (await Gallery.GetGallery(Section, Sort, (int)page)).Content;
            }
            return gallery?.Select(i => new GalleryItem(i)).ToList();
        }
        
        private async Task<List<GalleryItem>> GetTopicGallery(uint page)
        {
            var gallery = (await Topics.GetTopicGallery(TopicId, Sort, (int)page)).Content;
            return gallery?.Select(i => new GalleryItem(i)).ToList();
        }
        
        public ISettingsHelper Settings
        {
            get
            {
                return XamarinImgur.Helpers.Initializer.SettingsHelper;
            }
        }
    }
    
}
