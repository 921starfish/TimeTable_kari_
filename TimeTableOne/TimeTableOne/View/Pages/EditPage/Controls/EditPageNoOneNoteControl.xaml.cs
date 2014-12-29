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
using TimeTableOne.Data;

namespace TimeTableOne.View.Pages.EditPage.Controls
{
    public sealed partial class EditPageOneNoteControl : UserControl
    {
        public EditPageOneNoteControl()
        {
            this.InitializeComponent();
        }

		private async void NewButton_Click(object sender, RoutedEventArgs e) {
			await OneNoteControl.OneNoteControler.Current.CreateNotebook(TableUnitDataHelper.GetCurrentSchedule().TableName);
		}
    }
}
