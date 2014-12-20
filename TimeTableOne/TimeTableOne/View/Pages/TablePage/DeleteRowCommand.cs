using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTableOne.Data;
using TimeTableOne.Utils.Commands;

namespace TimeTableOne.View.Pages.TablePage
{
    public class DeleteRowCommand:DelegateCommandBase
    {
        public DeleteRowCommand(Action act) : base(act)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return ApplicationData.Instance.Configuration.TableCount > 1;
        }

        public void NotifyCanExecuteChanged()
        {
            base.NotifyCanExecuteChanged();
        }
    }
}
