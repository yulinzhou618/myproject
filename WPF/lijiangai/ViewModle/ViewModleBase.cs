using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AIVisualwfpnew.ViewModle
{
    public class ViewModleBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotityPropertyChange([CallerMemberName] string propName = "")
        {
            if (string.IsNullOrEmpty(propName) || this.PropertyChanged == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
