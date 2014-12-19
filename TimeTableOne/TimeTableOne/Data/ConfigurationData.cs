using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableOne.Data
{
    public class ConfigurationData
    {
        public TableType TableTypeSetting=TableType.AllDay;

        public int TableCount = 7;
    }

    public enum TableType
    {
        AllDay,WeekDay
    }
}
