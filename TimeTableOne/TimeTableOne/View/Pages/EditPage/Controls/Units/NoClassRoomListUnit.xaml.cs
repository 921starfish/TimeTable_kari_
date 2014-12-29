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
using TimeTableOne.Data;

namespace TimeTableOne.View.Pages.EditPage.Controls.Units
{
    public sealed partial class NoClassRoomListUnit : UserControl
    {
        public NoClassRoomListUnit()
        {
            this.InitializeComponent();
        }

        public NoClassRoomListUnitViewModel ViewModel
        {
            get { return DataContext as NoClassRoomListUnitViewModel; }
        }

        void ClassRoomChangeUnit_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ClassRoomChangeUnit_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {

        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var rootVM = TableUnitDataHelper.GetCurrentEditPageViewModel();
            if (ViewModel.IsNoClass)
            {
                MessageDialog dialog =
    new MessageDialog(
        string.Format("{0}の「{1}」を休講を解除してよろしいですか?", ViewModel.DisplayDate, rootVM.TableName), "休講設定");
                dialog.Commands.Add(new UICommand("はい", (a) => { ViewModel.ChangeNoClassState(false); }));
                dialog.Commands.Add(new UICommand("いいえ", (a) => { }));
                await dialog.ShowAsync();
            }
            else
            {
                MessageDialog dialog =
                    new MessageDialog(
                        string.Format("{0}の「{1}」を休講にしてよろしいですか?", ViewModel.DisplayDate, rootVM.TableName), "休講設定");
                dialog.Commands.Add(new UICommand("はい", (a) => { ViewModel.ChangeNoClassState(true); }));
                dialog.Commands.Add(new UICommand("いいえ", (a) => { }));
                await dialog.ShowAsync();
            }
        }
    }
}
