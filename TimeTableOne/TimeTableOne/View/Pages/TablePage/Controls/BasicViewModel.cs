using System.ComponentModel;
using System.Runtime.CompilerServices;
using TimeTableOne.Annotations;

namespace TimeTableOne.View.Pages.TablePage.Controls
{
    public abstract class BasicViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}