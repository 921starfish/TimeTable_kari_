﻿using System;
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

namespace TimeTableOne.View.Pages.EditPage.Controls.Units
{
    public sealed partial class AssignmentListUnit : UserControl
    {
        private bool isEditState = false;
        public AssignmentListUnit()
        {
            this.InitializeComponent();
            VisualStateManager.GoToState(this, "BasicState", false);
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
    }
}
