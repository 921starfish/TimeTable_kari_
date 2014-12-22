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

namespace TimeTableOne.View.Pages.EditPage.Controls
{
    public sealed partial class EditHeaderControl : UserControl
    {
        private bool isLectureNameEditing = false;
        private bool isLectureNameBoxFocused = false;
        private bool _isPlaceEditing;
        private bool isPlaceTextBoxFocused;

        public EditHeaderControl()
        {
            this.InitializeComponent();
            VisualStateManager.GoToState(this, "BasicState", true);
        }

        private void EditHeaderControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new EditHeaderControlViewModelInDesign();
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
            textBox.Focus(FocusState.Keyboard);
            textBox.SelectAll();
            isLectureNameBoxFocused = true;
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
            isLectureNameBoxFocused = false;
            isLectureNameEditing = false;
            VisualStateManager.GoToState(this, "BasicState", true);
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
            textBox1.Focus(FocusState.Keyboard);
            isPlaceTextBoxFocused = true;
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
            isPlaceTextBoxFocused = false;
            VisualStateManager.GoToState(this, "BasicState", true);
        }
    }
}
