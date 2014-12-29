using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace TimeTableOne.View.Pages.EditPage.Controls.Units
{
    public sealed partial class ClassRoomChangeUnit : UserControl
    {
        public ClassRoomChangeUnit()
        {
            this.InitializeComponent();
        }

        void ClassRoomChangeUnit_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ClassRoomChangeUnit_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void textBlock_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "EditState", true);
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "BasicState", true);

            Debug.WriteLine(textBox.Text);// 授業名
        }
    }
}
