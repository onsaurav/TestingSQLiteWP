using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TestingSQLiteWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestPage : Page
    {
        public int ID = 0;
        public byte[] image = null;
        DBAccess _DBAccess = new DBAccess();

        public TestPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Test item = e.Parameter as Test;
            if (item != null)
                Loadprofile(item);
        }

        private void Loadprofile(Test item)
        {
            try
            {
                ID = item.Id;
                txtname.Text = item.Name;
                txtpath.Text = item.Path;
                image = item.image;
                imgPhoto.Source = null;
                if (item.image != null)
                {
                    BitmapImage _BitmapImage = new BitmapImage();
                    _BitmapImage = ImageHelper.BytesToBitmapImage(item.image);
                    imgPhoto.Source = _BitmapImage;
                }
            }
            catch (Exception ex)
            {
                MessageDialogHelper.Show(ex.Message, "ERROR");
            }
        }

        private async void appbarSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckObject())
            {
                Test _Test = LoadTestObject();
                MyResult result = await _DBAccess.SaveTestAsync(_Test);
                MessageDialogHelper.Show(result.Message, "Note");
                if (result.IsSuccess)
                    Frame.Navigate(typeof(MainPage));
            }
        }

        private bool CheckObject()
        {
            if (string.IsNullOrEmpty(txtname.Text.Trim()))
            {
                MessageDialogHelper.Show("Name is empty", "Checking");
                return false;
            }

            if (string.IsNullOrEmpty(txtpath.Text.Trim()))
            {
                MessageDialogHelper.Show("Path is empty", "Checking");
                return false;
            }

            return true;
        }

        private Test LoadTestObject()
        {
            Test _Test = new Test();
            _Test.Id = ID;
            _Test.Name = txtname.Text;
            _Test.Path = txtpath.Text;
            if (image != null)
                _Test.image = image;
            return _Test;
        }

        private void btnLoadImage_Click(object sender, RoutedEventArgs e)
        {
            SelectImageFromPhone();
        }

        public void SelectImageFromPhone()
        {
            try
            {
                CoreApplicationView view = CoreApplication.GetCurrentView();

                FileOpenPicker filePicker = new FileOpenPicker();
                filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                filePicker.ViewMode = PickerViewMode.Thumbnail;

                filePicker.FileTypeFilter.Clear();
                filePicker.FileTypeFilter.Add(".bmp");
                filePicker.FileTypeFilter.Add(".png");
                filePicker.FileTypeFilter.Add(".jpeg");
                filePicker.FileTypeFilter.Add(".jpg");
                filePicker.FileTypeFilter.Add(".gif");
                filePicker.FileTypeFilter.Add(".ico");

                filePicker.PickSingleFileAndContinue();
                view.Activated += viewActivated;
            }
            catch (Exception ex)
            {
                MessageDialogHelper.Show(ex.Message, "Error!");
            }
        }

        private async void viewActivated(CoreApplicationView sender, IActivatedEventArgs args1)
        {
            FileOpenPickerContinuationEventArgs args = args1 as FileOpenPickerContinuationEventArgs;

            if (args != null)
            {
                if (args.Files.Count == 0) return;
                StorageFile _StorageFile = args.Files[0];
                imgPhoto.Source = new BitmapImage(new Uri("file://" + _StorageFile.Path));
                image = null;
                image = await ImageHelper.GetBytesFromStorageFile(_StorageFile);
            }
        }
    }
}
