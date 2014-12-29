using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
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
            string sectionName = "m月N日";
			OneNoteControl.OneNoteControler.Current.OpenNewSection(((EditPageViewModel)DataContext).TableName,sectionName);
		}

		private void OpenButton_Click_1(object sender, RoutedEventArgs e) {
			string sectionName = "m月N日";
			OneNoteControl.OneNoteControler.Current.OpenRecentlySection(((EditPageViewModel)DataContext).TableName, sectionName);
		}
    }
}
