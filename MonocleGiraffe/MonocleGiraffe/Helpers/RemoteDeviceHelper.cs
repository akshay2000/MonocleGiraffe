using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.RemoteSystems;
using Windows.UI.Core;
using Windows.Foundation.Metadata;
using Windows.System;
using MonocleGiraffe.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using MonocleGiraffe.Portable.Models;
using Windows.Foundation.Collections;

namespace MonocleGiraffe.Helpers
{
    public class RemoteDeviceHelper
    {
        private RemoteSystemWatcher remoteSystemWatcher;
        private Dictionary<string, RemoteSystem> deviceMap;

        public ObservableCollection<RemoteSystem> Devices { get; set; }

        public RemoteDeviceHelper()
        {
            Devices = new ObservableCollection<RemoteSystem>();
            deviceMap = new Dictionary<string, RemoteSystem>();
            BuildDeviceList();
        }


        private List<IRemoteSystemFilter> BuildDeviceFilter()
        {
            // construct an empty list
            var localListOfFilters = new List<IRemoteSystemFilter>();

            // construct a discovery type filter that only allows "proximal" connections:
            RemoteSystemDiscoveryTypeFilter discoveryFilter = new RemoteSystemDiscoveryTypeFilter(RemoteSystemDiscoveryType.Proximal);

            // construct an availibility status filter that only allows devices marked as available.
            RemoteSystemStatusTypeFilter statusFilter = new RemoteSystemStatusTypeFilter(RemoteSystemStatusType.Available);


            // add the 3 filters to the listL
            //localListOfFilters.Add(discoveryFilter);
            localListOfFilters.Add(statusFilter);

            // return the list
            return localListOfFilters;
        }

        private async void BuildDeviceList()
        {
            RemoteSystemAccessStatus accessStatus = await RemoteSystem.RequestAccessAsync();
            if (accessStatus == RemoteSystemAccessStatus.Allowed)
            {
                remoteSystemWatcher = RemoteSystem.CreateWatcher(BuildDeviceFilter());

                // Subscribing to the event raised when a new remote system is found by the watcher.
                remoteSystemWatcher.RemoteSystemAdded += RemoteSystemWatcher_RemoteSystemAdded;

                // Subscribing to the event raised when a previously found remote system is no longer available.
                remoteSystemWatcher.RemoteSystemRemoved += RemoteSystemWatcher_RemoteSystemRemoved;

                remoteSystemWatcher.Start();
            }
        }

        private async void RemoteSystemWatcher_RemoteSystemRemoved(RemoteSystemWatcher sender, RemoteSystemRemovedEventArgs args)
        {
            if (deviceMap.ContainsKey(args.RemoteSystemId))
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => Devices.Remove(deviceMap[args.RemoteSystemId]));
                deviceMap.Remove(args.RemoteSystemId);
            }
        }

        private async void RemoteSystemWatcher_RemoteSystemAdded(RemoteSystemWatcher sender, RemoteSystemAddedEventArgs args)
        {
            if (!deviceMap.ContainsKey(args.RemoteSystem.Id))
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => Devices.Add(args.RemoteSystem));
                deviceMap[args.RemoteSystem.Id] = args.RemoteSystem;
            }
        }

        private static bool? isRemoteAvailable;
        public static bool IsRemoteAvailable
        {
            get
            {
                isRemoteAvailable = isRemoteAvailable ?? ApiInformation.IsTypePresent("Windows.System.RemoteSystems.RemoteSystem");
                return isRemoteAvailable ?? false;
            }
        }

        public async Task LaunchOnSystem(RemoteSystem system)
        {
            BrowserPageViewModel browserVM = SimpleIoc.Default.GetInstance<BrowserPageViewModel>();
            string collectionJson = "{}";
            if (browserVM.Images is IJsonizable)
                collectionJson = (browserVM.Images as IJsonizable).toJson();
            ValueSet values = new ValueSet();
            values["images"] = collectionJson;
            values["index"] = browserVM.FlipViewIndex;
            RemoteLauncherOptions option = new RemoteLauncherOptions();
            option.FallbackUri = new Uri("https://www.microsoft.com/store/p/monocle-giraffe/9nblggh4qcvh");
            RemoteLaunchUriStatus launchUriStatus = await RemoteLauncher.LaunchUriAsync(new RemoteSystemConnectionRequest(system), 
                new Uri($"imgur:?images={collectionJson}&index={browserVM.FlipViewIndex}&type={browserVM.Images.GetType().Name}"));
        }
    }
}
