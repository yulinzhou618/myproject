using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Y.DP.App.ViewModel
{
    [Serializable]
    class PropertyChangeBase : INotifyPropertyChanged
    {
        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }

    static class PropertyChangedBaseEx
    {
        public static void OnPropertyChanged<T, TProperty>(this T PropertyChangeBase, Expression<Func<T, TProperty>> propertyname) where T : PropertyChangeBase
        {
            var PropertyName = propertyname.Body as MemberExpression;
            if (null != PropertyName)
            {
                PropertyChangeBase.OnPropertyChanged(PropertyName.Member.Name);
            }
        }
    }
}
