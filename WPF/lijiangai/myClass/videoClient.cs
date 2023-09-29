using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AIVisualwfpnew.myClass
{
    class videoClient
    {
        bool runflag = true;
        
      public void start()
        {
            
        }
        public void stop()
        {
            runflag = false;
        }
        public void draw(string jpgPath,Canvas canvas)
        {
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(jpgPath);
            myBitmapImage.EndInit();
            canvas.Background = new ImageBrush(myBitmapImage);
        }

    }
}
