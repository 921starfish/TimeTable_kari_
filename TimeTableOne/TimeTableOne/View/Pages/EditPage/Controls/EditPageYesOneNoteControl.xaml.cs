using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TimeTableOne.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TimeTableOne.Data;


// ユーザー コントロールのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace TimeTableOne.View.Pages.EditPage.Controls
{
    public sealed partial class EditPageYesOneNoteControl : UserControl
    {
       
        public EditPageYesOneNoteControl()
        {
            this.InitializeComponent();
        }

        private void EditPageYesOneNoteControl_OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            
        }

		private void NewButton_Click(object sender, RoutedEventArgs e) {
            string sectionName = DateTime.Now.ToString("yyyy年MMM月d日");
			TableUnitDataHelper.GetCurrentSchedule().RecentlySectionName = sectionName;
            OneNoteControl.OneNoteControler.Current.OpenNewSection(TableUnitDataHelper.GetCurrentSchedule().TableName, sectionName);
            Button_Loaded(null,null);
		}

		private void OpenButton_Click_1(object sender, RoutedEventArgs e) {
			string sectionName = TableUnitDataHelper.GetCurrentSchedule().RecentlySectionName;
            OneNoteControl.OneNoteControler.Current.OpenRecentlySection(TableUnitDataHelper.GetCurrentSchedule().TableName, sectionName);
		}

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext =new EditPageOneNoteControlViewModel();
        }

        private async void Button_Loaded(object sender, RoutedEventArgs e)
        {

			if (TableUnitDataHelper.GetCurrentSchedule().RecentlySectionName != "" && await OneNoteControl.OneNoteControler.Current.ExistsSection(TableUnitDataHelper.GetCurrentSchedule().TableName, TableUnitDataHelper.GetCurrentSchedule().RecentlySectionName))//セクションがあるかどうか
            {
                Button1.IsEnabled = true;
                Text1.Foreground = new SolidColorBrush(Colors.Black);
                Icon1.Visibility = Visibility.Visible;
               
            }
            else
            {
                Button1.IsEnabled = false;
                Text1.Foreground = new SolidColorBrush(Colors.Gray);
                Icon1.Visibility = Visibility.Collapsed;
            }
        }

    }
}
