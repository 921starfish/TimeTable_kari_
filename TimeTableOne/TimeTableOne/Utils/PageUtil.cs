using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TimeTableOne.View.Pages.DayPage;
using TimeTableOne.View.Pages.TablePage;

namespace TimeTableOne.Utils
{
    public static class PageUtil
    {
        public static void MovePage(MainStaticPages staticPage)
        {
            Type pageType = null;
            switch (staticPage)
            {
                case MainStaticPages.DayPage:
                    pageType = typeof (DayPage);
                    break;
                case MainStaticPages.TablePage:
                    pageType = typeof (TablePage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("staticPage");
            }
            ((Frame)Window.Current.Content).Navigate(pageType);
        }
    }

    public enum MainStaticPages
    {
        DayPage,
        TablePage
    }
}
