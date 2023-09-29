using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace AIVisualwfpnew.myClass
{
    class Client
    {
        int port = 0;
        public Canvas canvas;
        Page page;
        public Client(int port, Canvas canvas, Page page)
        {
            this.port = port;
            this.canvas = canvas;
            this.page = page;
        }
        public void start()
        {
            IPAddress ip = IPAddress.Parse("10.12.44.22");
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(new IPEndPoint(ip, this.port)); //配置服务器IP与端口
            while (data.whileFlag)
            {
                try
                {
                    byte[] result = new byte[1024 * 1024 * 10];
                    int length = clientSocket.Receive(result);
                    Console.WriteLine("lenght:" + length);
                    if (length < 1) continue; //如果接受的数据小于等于0则继续接收
                    this.page.Dispatcher.Invoke((Action)(() =>
                    {
                        MemoryStream ms1 = new MemoryStream(result);
                        BitmapImage myBitmapImage = new BitmapImage();
                        myBitmapImage.BeginInit();
                        myBitmapImage.StreamSource = ms1;
                        myBitmapImage.EndInit();
                        ImageBrush image = new ImageBrush(myBitmapImage);
                        this.canvas.Background = image;
                        switch (this.port)
                        {
                            case 1111:
                                if (myClass.data.camerabigindex == 1)
                                    myClass.data.camerabig.Background = image;
                                break;
                            case 1112:
                                if (myClass.data.camerabigindex == 2)
                                    myClass.data.camerabig.Background = image;
                                break;
                            case 1113:
                                if (myClass.data.camerabigindex == 3)
                                    myClass.data.camerabig.Background = image;
                                break;
                            case 1114:
                                if (myClass.data.camerabigindex == 4)
                                    myClass.data.camerabig.Background = image;
                                break;
                            case 1115:
                                if (myClass.data.camerabigindex == 5)
                                    myClass.data.camerabig.Background = image;
                                break;
                            case 1116:
                                if (myClass.data.camerabigindex == 6)
                                    myClass.data.camerabig.Background = image;
                                break;
                            default:
                                break;
                        }

                    }));
                }
                catch (Exception ex) { }
            }
            clientSocket.Close();
        }
    }
}
