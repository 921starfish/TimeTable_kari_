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
using TimeTableOne.Data;

// ユーザー コントロールのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace TimeTableOne.View.Pages.EditPage.Controls
{
    public sealed partial class ClassRoomControl : UserControl
    {
        public ClassRoomControl()
        {
            this.InitializeComponent();

            Loaded += ClassRoomControl_Loaded;
        }

        void ClassRoomControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new ClassRoomControlViewModel();
        }

        private void ToggleNoClassPopup(object sender, RoutedEventArgs e)
        {
            NoClassControl.IsOpen = !NoClassControl.IsOpen;
        }

        private async void ToggleChangeRoomPopup(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TableUnitDataHelper.GetCurrentSchedule().Place))
            {
                MessageDialog dlg = new MessageDialog("場所が入力されていないため、教室変更を行うことができません");
                dlg.Commands.Add(new UICommand("はい"));
                dlg.DefaultCommandIndex = 0;
                var cmd = await dlg.ShowAsync();
                if (cmd == dlg.Commands[0])
                {
                    return;
                }
            }
            else
            {
                ChangeRoomControl.IsOpen = !ChangeRoomControl.IsOpen;
            }
            
        }
    }
}
