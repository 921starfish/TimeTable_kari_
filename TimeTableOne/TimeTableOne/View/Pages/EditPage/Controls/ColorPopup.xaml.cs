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
            TableUnitDataHelper.GetCurrentSchedule().ColorData = selected.ColorBrush.Color;
            ApplicationData.SaveData();
            TableKey Key = TableUnitDataHelper.GetCurrentKey();
            // ViewModel.loadData(Key);
            EditPageUpdateEvents.OnColorUpdate();
         

        }

        private void ColorPopup_OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (DataContext is ColorPopupViewModel) return;
            ColorPopupViewModel vm;
            DataContext = vm = ColorPopupViewModel.GenerateViewModel(TableUnitDataHelper.GetCurrentSchedule());
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