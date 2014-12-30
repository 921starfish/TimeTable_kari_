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
using TimeTableOne.View.Pages.EditPage.Controls.Popups;

// ユーザー コントロールのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234236 を参照してください
using WinRTXamlToolkit.Controls.Extensions;

namespace TimeTableOne.View.Pages.EditPage.Controls.Units
{
    public sealed partial class AssignmentListUnit : UserControl
    {
        private bool isEditState = false;

        public AssignmentListUnitViewModel ViewModel
        {
            get { return DataContext as AssignmentListUnitViewModel; }
        }
        public AssignmentListUnit()
        {
            this.InitializeComponent();
            AssignmentControl.OnPopupClose += AssignmentControl_OnPopupClose;
            VisualStateManager.GoToState(this, "BasicState", false);
           
        }

        void AssignmentControl_OnPopupClose(object sender, EventArgs e)
        {
            EditPopup.IsOpen = false;
        }

        private void TextBlock1_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "EditState", true);
            isEditState = true;
        }

        private void Grid_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (isEditState)
            {
                
                VisualStateManager.GoToState(this, "BasicState", true);
                isEditState = false;
            }
        }


        private void ShowEditPopup(object sender, RoutedEventArgs e)
        {
            EditPopup.IsOpen = !EditPopup.IsOpen;
            if (EditPopup.IsOpen)
            {
                ViewModel.EditAssignmentPopupData = new EditAssignmentPopupViewModel(ViewModel._schedule);
                var frame = (Frame)Window.Current.Content;
                var rect=EditPopup.GetBoundingRect(frame);
                EditPopup.VerticalOffset = frame.ActualHeight/2 - 300 - rect.Y;
            }
        }

        
    }
}
