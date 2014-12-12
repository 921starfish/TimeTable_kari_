
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableOne.Utils.Commands
{
    public delegate void Action();

    public abstract class DelegateCommandBase : CommandBase
    {
        private Action onExecute;
        public DelegateCommandBase(Action act)
        {
            this.onExecute = act;
        }

        public override void Execute(object o)
        {
            onExecute();
        }
    }
}
