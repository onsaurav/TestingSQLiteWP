using System;
using System.Collections.Generic;
using System.Text;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace TestingSQLiteWP
{
    public class MyResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public static class MessageDialogHelper
    {
        public static async void Show(string content, string title)
        {
            MessageDialog messageDialog = new MessageDialog(content, title);
            await messageDialog.ShowAsync();
        }
    }

    public class UtilityHelper
    {
        public async void ExitMe()
        {
            MessageDialog msg = new MessageDialog("Are you sure, you want to exit this apps?", "Exit!");

            msg.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(CommandHandlers)));
            msg.Commands.Add(new UICommand("No", new UICommandInvokedHandler(CommandHandlers)));

            await msg.ShowAsync();
        }

        public void CommandHandlers(IUICommand commandLabel)
        {
            var Actions = commandLabel.Label;
            switch (Actions)
            {
                case "Yes":
                    Application.Current.Exit();
                    break;
                case "No":
                    break;
            }
        }

        public async void AppsRating()
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + Windows.ApplicationModel.Package.Current.Id.Name));
        }
    }
}
