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
        private HeaderState _currentState;

        public EditHeaderControl()
        {
            this.InitializeComponent();
            Loaded += EditHeaderControl_Loaded;
            CurrentState=HeaderState.Default;
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
                CurrentState=HeaderState.EditLectureName;
            }
        }
         
        private void LectureTextBox_MouseEnter(object sender, PointerRoutedEventArgs e)
        {
            if (isLectureNameEditing||_isPlaceEditing) return;
            CurrentState=HeaderState.MouseOverLectureName;
            
        }

        private void LectureTextBox_MouseExit(object sender, PointerRoutedEventArgs e)
        {
            if(isLectureNameEditing||_isPlaceEditing)return;
            CurrentState=HeaderState.Default;
            
        }

        private void textBlock_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            CurrentState=HeaderState.EditLectureName;
            isLectureNameEditing = true;
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
            CurrentState=HeaderState.Default;
            ViewModel.LectureNameForEdit = lectureNameBox.Text;
        }

        private void PlaceTextBox_MouseEnter(object sender, PointerRoutedEventArgs e)
        {
            if(_isPlaceEditing||isLectureNameEditing)return;
            CurrentState=HeaderState.MouserOverPlace;
        }

        private void PlaceTextBox_MouseExit(object sender, PointerRoutedEventArgs e)
        {
            if(_isPlaceEditing||isLectureNameEditing)return;
            CurrentState=HeaderState.Default;
            
        }

        private void PlaceTextBox_MousePressed(object sender, PointerRoutedEventArgs e)
        {
            _isPlaceEditing = true;
            CurrentState=HeaderState.EditPlace;
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
            CurrentState=HeaderState.Default;
            ViewModel.PlaceForEdit = placeBox.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ColorPopup.IsOpen = !ColorPopup.IsOpen;
        }

        private HeaderState CurrentState
        {
            get { return _currentState; }
            set
            {
                if (_currentState != value)
                {
                    _currentState = value;
                    string moveTo = "";
                    switch (_currentState)
                    {
                        case HeaderState.Default:
                            isLectureNameEditing = false;
                            _isPlaceEditing = false;
                            moveTo = "BasicState";
                            break;
                        case HeaderState.EditPlace:
                            isLectureNameEditing = false;
                            _isPlaceEditing = true;
                            moveTo = "OnEditPlace";
                            break;
                        case HeaderState.EditLectureName:
                            isLectureNameEditing = true;
                            _isPlaceEditing = false;
                            moveTo = "OnEditLectureName";
                            break;
                        case HeaderState.MouserOverPlace:
                            moveTo = "OnEditPlaceChanging";
                            break;
                        case HeaderState.MouseOverLectureName:
                            moveTo = "OnEditLectureNameChanging";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    VisualStateManager.GoToState(this, moveTo, true);
                }
            }
        }

        private enum HeaderState
        {
            Default,
            EditPlace,
            EditLectureName,
            MouserOverPlace,
            MouseOverLectureName
        }

        private void LectureNameBox_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
        }
    }
}
