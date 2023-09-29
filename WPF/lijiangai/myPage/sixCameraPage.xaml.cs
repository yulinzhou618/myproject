using AIVisualwfpnew.myClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AIVisualwfpnew.myPage
{
    /// <summary>
    /// sixCameraPage.xaml 的交互逻辑
    /// </summary>
    public partial class sixCameraPage : Page
    {
        public sixCameraPage()
        {
            InitializeComponent();
            camera1.MouseLeftButtonDown += (x, y) =>
            {
                if (y.ClickCount == 2)
                {
                    myClass.data.whileflag = "CameraPage";
                    myClass.data.camerabigindex = 1;

                }

            };
            camera2.MouseLeftButtonDown += (x, y) =>
            {
                if (y.ClickCount == 2)
                {
                    myClass.data.whileflag = "CameraPage";
                    myClass.data.camerabigindex = 2;
                }

            };
            camera3.MouseLeftButtonDown += (x, y) =>
            {
                if (y.ClickCount == 2)
                {
                    myClass.data.whileflag = "CameraPage";
                    myClass.data.camerabigindex = 3;
                }

            };
            camera4.MouseLeftButtonDown += (x, y) =>
            {
                if (y.ClickCount == 2)
                {
                    myClass.data.whileflag = "CameraPage";
                    myClass.data.camerabigindex = 4;
                }

            };
            camera5.MouseLeftButtonDown += (x, y) =>
            {
                if (y.ClickCount == 2)
                {
                    myClass.data.whileflag = "CameraPage";
                    myClass.data.camerabigindex = 5;
                }

            };
            camera6.MouseLeftButtonDown += (x, y) =>
            {
                if (y.ClickCount == 2)
                {
                    myClass.data.whileflag = "CameraPage";
                    myClass.data.camerabigindex = 6;
                }

            };
        }
        public void draw()
        {
            data.whileFlag = true;
            Client client1 = new Client(1111, camera1, this);
            Thread eye1Thread = new Thread(client1.start);
            eye1Thread.Start();
            Client client2 = new Client(1112, camera2, this);
            Thread eye2Thread = new Thread(client2.start);
            eye2Thread.Start();
            Client client3 = new Client(1113, camera3, this);
            Thread eye3Thread = new Thread(client3.start);
            eye3Thread.Start();
            Client client4 = new Client(1114, camera4, this);
            Thread eye4Thread = new Thread(client4.start);
            eye4Thread.Start();
            Client client5 = new Client(1115, camera5, this);
            Thread eye5Thread = new Thread(client5.start);
            eye5Thread.Start();
            Client client6 = new Client(1116, camera6, this);
            Thread eye6Thread = new Thread(client6.start);
            eye6Thread.Start();
        }
        public void drawstop()
        {
            data.whileFlag = false;
        }
    }
}
