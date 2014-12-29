using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableOne.Data.Args
{
    public class AssignmentChangedEventArgs:EventArgs
    {
        private Guid changedScheduleId;

        public AssignmentChangedEventArgs(Guid changedScheduleId)
        {
            this.changedScheduleId = changedScheduleId;
        }

        public Guid ChangedScheduleId
        {
            get { return changedScheduleId; }
        }
    }
}
