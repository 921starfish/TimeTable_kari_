using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace TimeTableOne.Utils
{
    public static class ColorUtil
    {
        private static DayOfWeek[] headers =
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
            DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
        };

        private static Color[] colors =
        {
            Colors.Black, Colors.Black, Colors.Black, Colors.Black, Colors.Black,
            Colors.CornflowerBlue, Colors.Red
        };

        public static float Liminance(this Color col)
        {
            return (float) (0.298912*col.R/255d + 0.586611*col.G/255d + 0.114478*col.B/255d);
        }

        public static Brush GetWeekColor(this DayOfWeek week,bool isWhiteTheme=true)
        {
            var col = colors[Array.IndexOf(headers, week)];
            if (!isWhiteTheme && col == Colors.Black)
            {
                col = Colors.White;
            }
            return new SolidColorBrush(col);
        }
    }
}
