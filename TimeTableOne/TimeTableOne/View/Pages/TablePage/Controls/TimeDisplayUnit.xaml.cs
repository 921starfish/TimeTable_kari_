using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TimeTableOne.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// ユーザー コントロールのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace TimeTableOne.View.Pages.TablePage.Controls
{
    public sealed partial class TimeDisplayUnit : UserControl
    {
        private TimeDisplayUnitViewModel ViewModel
        {
            get { return DataContext as TimeDisplayUnitViewModel; }
        }
        private bool isFocused = false;
       
        public TimeDisplayUnit()
        {
            this.InitializeComponent();
            VisualStateManager.GoToState(this, "NotMouseOver", false);
            TablePageUpdateEvents.EditTimeDisplay += TimeDisplayUnitOnPointerPressed;
            TablePageUpdateEvents.CommitTimeDisplay += CommitTimeDisplay;
            TablePageUpdateEvents.RequestTimeCommit += PushTimeSpan;
        }

        public void PushTimeSpan()
        {
            PushTimeSpanTask();
        }

        public async Task PushTimeSpanTask()
        {
            var x =await GetTimeSpanTask();
            TablePageUpdateEvents.ScheduleTimeSpansList.Add(GetTimeSpanTask().Result); 
                  
        }

        private async Task<ScheduleTimeSpan> GetTimeSpanTask()
        {
            return await ViewModel.GetTargetModelSpan();
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            if (isFocused) return;
            VisualStateManager.GoToState(this, "MouseOverToEdit", true);
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            if (isFocused) return;
            VisualStateManager.GoToState(this, "NotMouseOver", true);
        }

        public void CommitTimeDisplay()
        {
            CommitTimeDisplayTask();
        }

        public async Task CommitTimeDisplayTask()
        {
            isFocused = !isFocused;
            await ViewModel.CommitChange();
            VisualStateManager.GoToState(this, "NotMouseOver", false);
        }

        public void TimeDisplayUnitOnPointerPressed()
        {
            VisualStateManager.GoToState(this, "Editing", true);
            textBox.Focus(FocusState.Keyboard);
            isFocused = !isFocused;
        }


        private void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box != null)
            {
                box.SelectAll();
            }
        }

        private void TextBox_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
        }

        //{
        //    if (e.Key == VirtualKey.Enter)
        //    {
        //        if (isFocused)
        //        {
        //            EndFocus();
        //        }
        //    }
        //}
    }
}
