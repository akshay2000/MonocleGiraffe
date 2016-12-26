using GalaSoft.MvvmLight.Ioc;
using MonocleGiraffe.Portable.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinImgur.APIWrappers;
using XamarinImgur.Helpers;

namespace MonocleGiraffe.Portable.Helpers
{
    public static class Initializer
    {
        static Initializer()
        {
            networkHelper = SimpleIoc.Default.GetInstance<NetworkHelper>();
        }

        public static void Init(IRoamingDataHelper roamingDataHelper, ISharingHelper sharingHelper, IClipboardHelper clipboardHelper)
        {
            RoamingDataHelper = roamingDataHelper;
            SharingHelper = sharingHelper;
            ClipboardHelper = clipboardHelper;
        }

        public static IRoamingDataHelper RoamingDataHelper { get; private set; }
        public static ISharingHelper SharingHelper { get; private set; }
        public static IClipboardHelper ClipboardHelper { get; private set; }

        //Too lazy to refactor. So provide gateway to IOC as static properties

        private static readonly NetworkHelper networkHelper;

        private static Accounts accounts;
        public static Accounts Accounts { get { return accounts = accounts ?? new Accounts(networkHelper); } }

        private static Albums albums;
        public static Albums Albums { get { return albums = albums ?? new Albums(networkHelper); } }

        private static Comments comments;
        public static Comments Comments { get { return comments = comments ?? new Comments(networkHelper); } }

        private static Gallery gallery;
        public static Gallery Gallery { get { return gallery = gallery ?? new Gallery(networkHelper); } }

        private static Images images;
        public static Images Images { get { return images = images ?? new Images(networkHelper); } }

        private static Reddits reddits;
        public static Reddits Reddits { get { return reddits = reddits ?? new Reddits(networkHelper); } }

        private static Topics topics;
        public static Topics Topics { get { return topics = topics ?? new Topics(networkHelper); } }

        public static AuthenticationHelper AuthenticationHelper { get { return SimpleIoc.Default.GetInstance<AuthenticationHelper>(); } }

        public static SecretsHelper SecretsHelper { get { return SimpleIoc.Default.GetInstance<SecretsHelper>(); } }

        public static NetworkHelper NetworkHelper { get { return networkHelper; } }
    }
}
