using TimeTableOne.Utils;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace TimeTableOne.View.Pages.DayPage
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class DayPage : Page
    {
        public DayPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new DayPageViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof (TablePage.TablePage));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PageUtil.MovePage(MainStaticPages.TablePage);
        }
    }
}
