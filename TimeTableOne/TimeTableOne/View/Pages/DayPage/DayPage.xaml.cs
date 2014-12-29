using System;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Pickers;
using TimeTableOne.Background;
using TimeTableOne.Utils;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace TimeTableOne.View.Pages.DayPage
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class DayPage : Page
    {
        public DayPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new DayPageViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof (TablePage.TablePage));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PageUtil.MovePage(MainStaticPages.TablePage);
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            BottomCommandBar.IsOpen = !BottomCommandBar.IsOpen;
        }
        private async void AppBarButton_Click_SelectImage(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.CommitButtonText = "背景を選択";
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".gif");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".bmp");
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            // 画像ファイルの指定
            StorageFile file = await picker.PickSingleFileAsync();
            // 選択されなかった場合
            if (file == null)
            {
                return;
            }
            Debug.WriteLine("選択した画像{0}", file.Path);
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            await file.CopyAsync(folder, "Background"+file.FileType, NameCollisionOption.ReplaceExisting);
            Debug.WriteLine("アプリローカルに保存{0}\\{1}", folder.Path, "\\Background" + file.FileType);
            Data.ApplicationData.Instance.Configuration.BackgroundImagePath = folder.Path + "\\Background" + file.FileType;
            this.DataContext = new DayPageViewModel();
        }

        private async void NotificationSetting(object sender, RoutedEventArgs e)
        {
           await BackgroundTaskManager.AskRegister();
        }
    }
}
