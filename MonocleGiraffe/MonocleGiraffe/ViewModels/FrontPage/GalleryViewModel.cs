using MonocleGiraffe.Helpers;
using MonocleGiraffe.Models;
using MonocleGiraffe.Pages;
using SharpImgur.APIWrappers;
using SharpImgur.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Windows.ApplicationModel;
using System.Threading;
using static SharpImgur.APIWrappers.Enums;

namespace MonocleGiraffe.ViewModels.FrontPage
{
    public class GalleryViewModel : BindableBase
    {
        private const string VIRAL = "Viral";
        private const string TIME = "Time";
        private const string POPULAR = "Popular";
        private const string TOP = "Top";
        private const string MOST_VIRAL = "MOST VIRAL";

        public GalleryViewModel()
        {
            if (DesignMode.DesignModeEnabled)
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

        private Enums.Section ToSection(string sectionString)
        {
            switch (sectionString)
            {
                case POPULAR:
                    return Enums.Section.Hot;
                case TOP:
                    return Enums.Section.Top;
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

        DelegateCommand<string> sectionCommand;
        public DelegateCommand<string> SectionCommand
           => sectionCommand ?? (sectionCommand = new DelegateCommand<string>((string parameter) =>
           {
               LoadGallery(parameter, VIRAL);
           }));

        DelegateCommand<string> sortCommand;
        public DelegateCommand<string> SortCommand
           => sortCommand ?? (sortCommand = new DelegateCommand<string>((string parameter) =>
           {
               Topic topic = Topics[TopicSelectedIndex];
               LoadTopicGallery(topic, parameter);
           }));

        #endregion

        private void LoadGallery(string sectionString, string sortString)
        {
            Section = sectionString;
            IsSectionVisible = true;
            Section section = ToSection(sectionString);
            Sort sort = ToSort(sortString);
            Images = new IncrementalGallery(MOST_VIRAL, section, sort);
            //Images.LoadMoreItemsAsync(10);
        }

        private async Task LoadTopics()
        {
            var topics = await SharpImgur.APIWrappers.Topics.GetDefaultTopics();
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

        public void ImageTapped(object sender, object parameter)
        {
            var args = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            var clickedItem = args.ClickedItem as GalleryItem;
            ImageSelectedIndex = Images.IndexOf(clickedItem);
            const string navigationParamName = "GalleryInfo";
            BootStrapper.Current.SessionState[navigationParamName] = new GalleryMetaInfo { Gallery = Images, SelectedIndex = ImageSelectedIndex };
            BootStrapper.Current.NavigationService.Navigate(typeof(BrowserPage), navigationParamName);
            return;
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

        public void TopicTapped(object sender, object parameter)
        {
            var args = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            var clickedItem = args.ClickedItem as Topic;
            ClosePane();
            LoadTopicGallery(clickedItem, VIRAL);
        }

        private void LoadTopicGallery(Topic topic, string sortString)
        {
            Title = topic.Name;
            if (topic.Name == MOST_VIRAL)
                LoadGallery(Section, sortString);
            else
            {
                IsSectionVisible = false;
                Images = new IncrementalGallery(topic.Name, ToSort(sortString), topic.Id);
            }
        }

        private void InitDesignTime()
        {
            Title = MOST_VIRAL;
            Section = POPULAR;
            isSectionVisible = true;
            Images = new IncrementalGallery(Title, ToSection(POPULAR), Sort.Viral);
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

    public class IncrementalGallery : IncrementalCollection<GalleryItem>
    {
        private const string VIRAL = "Viral";
        private const string TIME = "Time";
        private const string POPULAR = "Popular";
        private const string TOP = "Top";
        private const string MOST_VIRAL = "MOST VIRAL";

        public IncrementalGallery(string title, Sort sort, int topicId)
        {
            Title = title;
            Sort = sort;
            TopicId = topicId;
        }

        public IncrementalGallery(string title, Section section, Sort sort)
        {
            Title = title;
            Section = section;
            Sort = sort;
        }

        public string Title { get; private set; }

        public Section Section { get; private set; }

        public Sort Sort { get; private set; }

        public int TopicId { get; private set; }

        protected override bool HasMoreItemsImpl()
        {
            return true;
        }

        protected async override Task<List<GalleryItem>> LoadMoreItemsImplAsync(CancellationToken c, uint page)
        {
            if (Title == MOST_VIRAL)
                return await GetGallery(page);
            else
                return await GetTopicGallery(page);
        }

        private async Task<List<GalleryItem>> GetGallery(uint page)
        {
            var gallery = await Gallery.GetGallery(Section, Sort, (int)page);
            return gallery?.Select(i => new GalleryItem(i)).ToList();
        }

        private async Task<List<GalleryItem>> GetTopicGallery(uint page)
        {
            var gallery = await Topics.GetTopicGallery(TopicId, Sort, (int)page);
            return gallery?.Select(i => new GalleryItem(i)).ToList();
        }
    }
    
}
