using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTable0._0β
{
    public class TableKey
    {
        public TableKey(int tableNumber,int numberOfday)
        {
            this.TableNumber = tableNumber;
            this.NumberOfDay = numberOfday;
        }
        public TableKey(int tableNumber, DayOfWeek dayOfWeek)
        {
            this.TableNumber = tableNumber;
            this.dayOfWeek = dayOfWeek;
        }
        private DayOfWeek _dayOfWeek;
        public int TableNumber { get; set; }
        public DayOfWeek dayOfWeek
        {
            get { return _dayOfWeek; }
            set { _dayOfWeek = value; }
        }
        public int NumberOfDay
        {
            get
            {
                switch (_dayOfWeek)
                {
                    case DayOfWeek.Friday:
                        return 5;
                    case DayOfWeek.Monday:
                        return 1;
                    case DayOfWeek.Saturday:
                        return 6;
                    case DayOfWeek.Sunday:
                        return 7;
                    case DayOfWeek.Thursday:
                        return 4;
                    case DayOfWeek.Tuesday:
                        return 2;
                    case DayOfWeek.Wednesday:
                        return 3;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (value)
                {
                    case 1:
                        _dayOfWeek = DayOfWeek.Monday;
                        break;
                    case 2:
                        _dayOfWeek = DayOfWeek.Tuesday;
                        break;
                    case 3:
                        _dayOfWeek = DayOfWeek.Wednesday;
                        break;
                    case 4:
                        _dayOfWeek = DayOfWeek.Thursday;
                        break;
                    case 5:
                        _dayOfWeek = DayOfWeek.Friday;
                        break;
                    case 6:
                        _dayOfWeek = DayOfWeek.Saturday;
                        break;
                    case 7:
                        _dayOfWeek = DayOfWeek.Sunday;
                        break;
                }
            }
        }
    }
}
