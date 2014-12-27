using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Windows.UI.Xaml.Navigation;

// 設定フライアウトの項目テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=273769 を参照してください

namespace TimeTableOne
{
    public sealed partial class TopBackgroundSelect : SettingsFlyout
    {

        public TopBackgroundSelect()
        {
            this.InitializeComponent();
        }

        private async void Button_Click_SelectImage(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.CommitButtonText = "背景を選択";
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".gif");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".bmp");
            picker.ViewMode = PickerViewMode.List;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            // 画像ファイルの指定
            StorageFile file = await picker.PickSingleFileAsync();
            // 選択されなかった場合
            if (file == null)
            {
                return;
            }
            Debug.WriteLine("選択した画像{0}",file.Path);
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            await file.CopyAsync(folder, file.Name, NameCollisionOption.ReplaceExisting);
            Debug.WriteLine("アプリローカルに保存{0}\\{1}", folder.Path,file.Name);
            Data.ApplicationData.Instance.Configuration.BackgroundImagePath = folder.Path +"\\" +file.Name;

        }

        private void Button_Click_SetToDefault(object sender, RoutedEventArgs e)
        {
            Data.ApplicationData.Instance.Configuration.BackgroundImagePath = "ms-appx:///Assets/Background(1).jpg";
        }
    }
  
}
