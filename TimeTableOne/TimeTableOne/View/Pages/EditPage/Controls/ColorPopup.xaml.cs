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
using TimeTableOne.Utils;

namespace TimeTableOne.View.Pages.EditPage.Controls
{
    public sealed partial class ColorPopup : UserControl
    {
        private EditPageViewModel ViewModel
        {
            get { return DataContext as EditPageViewModel; }
        }

        public ColorPopup()
        {
            this.InitializeComponent();

        }



        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = ColorList.SelectedItem as ColorPopupUnitViewModel;
            if (selected == null) return;
            ViewModel.ScheduleData.ColorData = selected.ColorBrush.Color;
            ApplicationData.SaveData();
            TableKey Key = ViewModel.TableKey;
            ViewModel.loadData(Key);


        }

        private void ColorPopup_OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            ColorPopupViewModel vm;
            ColorList.DataContext = vm = ColorPopupViewModel.GenerateViewModel(ViewModel.ScheduleData);
            try
            {
                ColorList.SelectedIndex = vm.SelectedIndex;
            }
            catch (ArgumentException ex)
            {

            }
        }


    }
}