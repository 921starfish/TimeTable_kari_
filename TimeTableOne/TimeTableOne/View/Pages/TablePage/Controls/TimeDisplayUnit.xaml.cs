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

namespace TimeTableOne.View.Pages.TablePage.Controls
{
    public sealed partial class TimeDisplayUnit : UserControl
    {

        private bool isFocused = false;
        public TimeDisplayUnit()
        {
            this.InitializeComponent();
            VisualStateManager.GoToState(this, "NotMouseOver", false);
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
            if(isFocused)return;
            VisualStateManager.GoToState(this, "NotMouseOver", true);
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (isFocused)
            {
                VisualStateManager.GoToState(this, "MouseOverToEdit", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Editing", true);
                textBox.SelectAll();
            }
            isFocused = !isFocused;
        }
    }
}
