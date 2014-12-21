using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeTableOne.Common;

namespace TimeTableOne.View.Pages.TablePage.Controls
{
    public class AppendRowButtonViewModel:BasicViewModel
    {
        public AppendRowButtonViewModel()
        {
            
        }

        public ICommand Append
        {
            get { return BasicTableCommands.AddRowCommand; }
        }
    }
}
