using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestingSQLiteWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DBAccess _DBAccess = new DBAccess();

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

            await _DBAccess.CheckDbAsync();
            await LoadTests();
        }

        private async System.Threading.Tasks.Task LoadTests()
        {
            try
            {
                List<Test> list = new List<Test>();
                list = await _DBAccess.LoadTestsAsync();
                list = list.OrderByDescending(x => x.Id).ToList();
                lvwDailyNotes.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageDialogHelper.Show(ex.Message, "Loading");
            }
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
                return;

            if (frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
        }

        private void appbarNew_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TestPage));
        }

        private void lvwDailyNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void lvwDailyNotes_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (lvwDailyNotes.SelectedItems.Count > 0)
            {
                Test item = new Test();
                item = (Test)lvwDailyNotes.SelectedItems[0];
                Frame.Navigate(typeof(TestPage), item);
            }
        }
    }
}
