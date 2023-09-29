using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace AIVisualwfpnew.myPage
{
    class mainPageViewModel : INotifyPropertyChanged
    {
    
            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName = null)
            {
                if (PropertyChanged != null)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            private string meaInfo;
            public string MeaInfo
            {
                get
                {
                    return meaInfo;
                }

                set
                {
                    meaInfo = value;
                    OnPropertyChanged("MeaInfo");
                }
            }

            private StrokeCollection inkStrokes;
            public StrokeCollection InkStrokes
            {
                get { return inkStrokes; }
                set
                {
                    inkStrokes = value;
                    OnPropertyChanged("InkStrokes");
                }
            }

    }
}
