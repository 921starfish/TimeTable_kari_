using Windows.Foundation.Metadata;
using Windows.UI;
using TimeTableOne.View.Pages.TablePage.Controls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// ユーザー コントロールのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace TimeTableOne.View.Pages.DayPage.Controls
{
    public sealed partial class DayPageTable : UserControl
    {
        public DayPageTable()
        {
            this.InitializeComponent();
        
        }


      
        private void Button1_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (Block1.Text == "")
            {
            }
            else
            {
                Button1.Background = new SolidColorBrush(Color.FromArgb(255, 128, 57, 123));
                Button1.BorderBrush = new SolidColorBrush(Color.FromArgb(0,0,0,0));
                Button1.Content = "";
            }
        }


    }
}
