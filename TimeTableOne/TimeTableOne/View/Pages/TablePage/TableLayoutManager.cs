using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    return 170;
                    break;
                case TableType.WeekDay:
                    return 250;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        
    }
}
