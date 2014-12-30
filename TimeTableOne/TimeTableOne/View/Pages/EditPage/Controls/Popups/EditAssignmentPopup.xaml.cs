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
    public sealed partial class EditAssignmentPopup : UserControl
    {
        public EditAssignmentPopup()
        {
            this.InitializeComponent();
            Loaded += AddAssignmentPopup_Loaded;
            DataContextChanged += EditAssignmentPopup_DataContextChanged;
        }

        void EditAssignmentPopup_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            
        }

        private void AddAssignmentPopup_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public EditAssignmentPopupViewModel ViewModel
        {
            get { return DataContext as EditAssignmentPopupViewModel; }
        }


        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            Point transformPoint = b.TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0));


            PopupMenu menu = new PopupMenu();
            menu.Commands.Add(new UICommand("明日", (d) => assignDayAfter(1)));
            menu.Commands.Add(new UICommand("次週", (d) => assignDayAfter(7)));
            menu.Commands.Add(new UICommand("再来週", (d) => assignDayAfter(14)));
            menu.Commands.Add(new UICommand("3週間後", (d) => assignDayAfter(21)));
            menu.Commands.Add(new UICommand("4週間後", (d) => assignDayAfter(28)));
            await menu.ShowAsync(transformPoint);

        }

        private void assignDayAfter(int date)
        {
            ViewModel.DueDate = DateTime.Now.AddDays(date);
        }
    }
}
