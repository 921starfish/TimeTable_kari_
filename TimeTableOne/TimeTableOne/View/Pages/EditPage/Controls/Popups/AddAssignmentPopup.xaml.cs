using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// ユーザー コントロールのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace TimeTableOne.View.Pages.EditPage.Controls.Popups
{
    public sealed partial class AddAssignmentPopup : UserControl
    {
        public AddAssignmentPopup()
        {
            this.InitializeComponent();
        }

        public AddAssignmentPopupViewModel ViewModel
        {
            get { return DataContext as AddAssignmentPopupViewModel; }
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            Point transformPoint = b.TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0));


            PopupMenu menu = new PopupMenu();
            menu.Commands.Add(new UICommand("テスト1"));
            menu.Commands.Add(new UICommand("テスト2"));
            await menu.ShowAsync(transformPoint);

        }
    }
}
