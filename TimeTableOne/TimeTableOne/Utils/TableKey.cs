using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableOne.Utils
{
    public class TableKey
    {
        public TableKey(int tableNumber, int numberOfday)
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
                if ((int) _dayOfWeek == 0) return 7;
                else return (int) _dayOfWeek;
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
                    case 0:
                        _dayOfWeek = DayOfWeek.Sunday;
                        break;
                    case 7:
                        _dayOfWeek = DayOfWeek.Sunday;
                        break;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            TableKey key = obj as TableKey;
            return key.dayOfWeek == dayOfWeek && key.TableNumber == TableNumber;
        }
    }
}
