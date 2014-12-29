using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimeTable0._0β.Commands
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged = delegate { };

        protected void NotifyCanExecuteChanged()
        {
            this.CanExecuteChanged(this, new EventArgs());
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
}
