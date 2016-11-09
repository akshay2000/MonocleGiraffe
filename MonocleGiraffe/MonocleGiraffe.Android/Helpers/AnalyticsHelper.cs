using System;
using Android.Gms.Analytics;
using Android.Content;
using Newtonsoft.Json.Linq;
using System.IO;

namespace MonocleGiraffe.Android
{
	public static class AnalyticsHelper
	{
        static AnalyticsHelper()
        {
            var gaInstace = GoogleAnalytics.GetInstance(AppContext);
            //gaInstace.SetLocalDispatchPeriod(10);
            GATracker = gaInstace.NewTracker(GATrackingId);
            GATracker.EnableAdvertisingIdCollection(true);
            GATracker.EnableExceptionReporting(true);            
        }

        private static Context AppContext => global::Android.App.Application.Context;
        public static Tracker GATracker { get; private set; }        

        private static string gaTrackingId;
        private static string GATrackingId
        {
            get
            {
                gaTrackingId = gaTrackingId ?? (string)LoadAnalyticsJson()["google_analytics_property_id"];
                return gaTrackingId;
            }
        }

        private static JObject LoadAnalyticsJson()
        {
            string content = "{}";
            var assets = AppContext.Assets;
            using (StreamReader sr = new StreamReader(assets.Open("Analytics.json")))
            {
                content = sr.ReadToEnd();
            }
            return JObject.Parse(content);
        }

        public static void SendView(string viewName)
        {
            GATracker.SetScreenName(viewName);
            GATracker.Send(new HitBuilders.ScreenViewBuilder().Build());
        }

        public static void SendEvent(string category, string action)
        {
            GATracker.Send(new HitBuilders.EventBuilder(category, action).SetLabel("AppEvent").Build());
        }

        public static void SendException(string description, bool isFatal)
        {
            GATracker.Send(new HitBuilders.ExceptionBuilder().SetDescription(description).SetFatal(isFatal).Build());
        }

    }
}

