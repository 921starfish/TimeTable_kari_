using System;

namespace TimeTableOne.Utils.Commands
{
    public class BasicCommand : DelegateCommandBase
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