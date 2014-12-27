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
using TimeTableOne.Data;

// ユーザー コントロールのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234236 を参照してください
using TimeTableOne.Utils;

namespace TimeTableOne.View.Pages.EditPage.Controls
{
    public sealed partial class EditHeaderControl : UserControl
    {
        private bool isLectureNameEditing = false;
        private bool _isPlaceEditing;

        public EditHeaderControl()
        {
            this.InitializeComponent();
            Loaded += EditHeaderControl_Loaded;
            VisualStateManager.GoToState(this, "BasicState", true);
        }

        public EditHeaderControlViewModel ViewModel
        {
            get { return DataContext as EditHeaderControlViewModel; }
        }

        void EditHeaderControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new EditHeaderControlViewModel(TableUnitDataHelper.GetCurrentKey());
            if (String.IsNullOrWhiteSpace(ViewModel.LectureName))
            {
                VisualStateManager.GoToState(this, "OnEditLectureName", false);
            }
        }
         
        private void LectureTextBox_MouseEnter(object sender, PointerRoutedEventArgs e)
        {
            if (isLectureNameEditing||_isPlaceEditing) return;
            VisualStateManager.GoToState(this, "OnEditLectureNameChanging", true);
        }

        private void LectureTextBox_MouseExit(object sender, PointerRoutedEventArgs e)
        {
            if(isLectureNameEditing||_isPlaceEditing)return;
            VisualStateManager.GoToState(this, "BasicState", true);
        }

        private void textBlock_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "OnEditLectureName", true);
            isLectureNameEditing = true;
            lectureNameBox.Focus(FocusState.Pointer);
        }

        private void LectureTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox b = sender as TextBox;
            if (b != null)
            {
                b.SelectAll();
            }
        }

        private void LectureTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            isLectureNameEditing = false;
            VisualStateManager.GoToState(this, "BasicState", true);
            ViewModel.LectureNameForEdit = lectureNameBox.Text;
        }

        private void PlaceTextBox_MouseEnter(object sender, PointerRoutedEventArgs e)
        {
            if(_isPlaceEditing||isLectureNameEditing)return;
            VisualStateManager.GoToState(this, "OnEditPlaceChanging", true);
        }

        private void PlaceTextBox_MouseExit(object sender, PointerRoutedEventArgs e)
        {
            if(_isPlaceEditing||isLectureNameEditing)return;
            VisualStateManager.GoToState(this, "BasicState", true);
        }

        private void PlaceTextBox_MousePressed(object sender, PointerRoutedEventArgs e)
        {
            _isPlaceEditing = true;
            VisualStateManager.GoToState(this, "OnEditPlace", true);
            placeBox.Focus(FocusState.Keyboard);
        }

        private void PlaceTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox b = sender as TextBox;
            if(b!=null)
            {
                b.SelectAll();
            }
        }

        private void PlaceTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _isPlaceEditing = false;
            VisualStateManager.GoToState(this, "BasicState", true);
            ViewModel.PlaceForEdit = placeBox.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ColorPopup.IsOpen = !ColorPopup.IsOpen;
        }
    }
}
