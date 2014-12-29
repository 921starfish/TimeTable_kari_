using Windows.UI.Xaml.Controls;

// ユーザー コントロールのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace TimeTableOne.View.Pages.DayPage.Controls
{
    public sealed partial class TimeControl : UserControl
    {
        public TimeControl()
        {
            this.InitializeComponent();
        }

        private void Grid_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.DataContext = new TimeControlViewModel();
        }
    }
}
