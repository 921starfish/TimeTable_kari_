using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace TimeTableOne.View.Pages.DayPage
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class DayPageView : Page
    {
        public DayPageView()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new DayPageViewModel(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = new DayPageViewModel(this);
        }
    }
}
