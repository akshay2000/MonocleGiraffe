using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.ApplicationModel.Email;
using Windows.UI;
using Windows.UI.Xaml.Navigation;

namespace MonocleGiraffe.ViewModels
{

    public class FeedbackPageViewModel : ViewModelBase
    {
        public FeedbackPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // design-time experience
            }
            else
            {
                // runtime experience
            }
        }


        string feedbackText = default(string);
        public string FeedbackText
        {
            get { return feedbackText; }
            set
            {
                IsSendEnabled = !string.IsNullOrWhiteSpace(value);
                Set(ref feedbackText, value);
            }
        }

        DelegateCommand send;
        public DelegateCommand Send
           => send ?? (send = new DelegateCommand(async () =>
           {
               await SendFeedback();
           }));


        bool isSendEnabled = default(bool);
        public bool IsSendEnabled { get { return isSendEnabled; } set { Set(ref isSendEnabled, value); } }

        private async Task SendFeedback()
        {
            var emailMessage = new EmailMessage();
            emailMessage.Body = FeedbackText;
            emailMessage.Subject = "Monocle Giraffe for Windows";
            emailMessage.To.Add(new EmailRecipient("akshay2000+mg@hotmail.com"));
            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }    

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (state.Any())
            {
                // restore state
                state.Clear();
            }
            else
            {
                // use parameter
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                // save state
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }
    }
}
