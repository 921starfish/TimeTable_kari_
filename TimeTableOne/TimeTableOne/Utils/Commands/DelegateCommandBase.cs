
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableOne.Utils.Commands
{

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

    class BasicCommand : DelegateCommandBase
    {
        private readonly Func<bool> _canExecute;

        public BasicCommand(Action act,Func<bool> canExecute) : base(act)
        {
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public new void NotifyCanExecuteChanged()
        {
            base.NotifyCanExecuteChanged();
        }
    }
}
