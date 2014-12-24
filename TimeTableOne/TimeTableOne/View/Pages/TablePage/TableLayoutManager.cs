using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TimeTableOne.Data;

namespace TimeTableOne.View.Pages.TablePage
{
    public class TableLayoutManager
    {
        public static int getElementWidth(TableType type)
        {
            switch (type)
            {
                case TableType.AllDay:
                    return (int)(((Frame)(Window.Current.Content)).ActualWidth / 7 - ((Frame)(Window.Current.Content)).ActualWidth / 50);
                    break;

                case TableType.WeekDay:
                    return (int)(((Frame)(Window.Current.Content)).ActualWidth / 5 - ((Frame)(Window.Current.Content)).ActualWidth / 30);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        public static int getElementCount(TableType type)
        {
            switch (type)
            {
                case TableType.AllDay:
                    return 7;
                    break;
                case TableType.WeekDay:
                    return 5;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }


    }
}
