using AIVisualwfpnew.CommunicationMsg.PushMsg;
using AIVisualwfpnew.Helpers;
using AIVisualwfpnew.myPage;
using AIVisualwfpnew.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
 
using System.Drawing.Printing;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AIVisualwfpnew
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentRendered += MainWindow_ContentRendered;
        }

        bool dragflag = true;
        bool dragflagin = true;
        bool clearinputuserflag = true;
        bool clearinputpwdflag = true;
        System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            mainInit();
            loginInit();
            printDocument.PrintPage += PrintDocument_PrintPage;
            PushManager.Current.RegisterStringMessage(CommunicationMsg.PushMsg.PushType.Invasion, new MessageStringHandler(InvasionPushMsgHandler));
        }

  
        /// <summary>
        /// 接收到服务端推送过来的消息
        /// </summary>
        /// <param name="header"></param>
        /// <param name="body"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void InvasionPushMsgHandler(PushHeaderEntity header, string body)
        {
            if (string.IsNullOrEmpty(body))
                return;

            List<ReceivedMessageEntity> datas = JsonConvert.DeserializeObject<List<ReceivedMessageEntity>>(body);
            if (datas == null || datas.Count <= 0)
                return;

            _ = this.Dispatcher.BeginInvoke(new Action(() =>
            {
                var rtb = UIHelper.GetFirstChild<RichTextBox>(myFrame);
                if (rtb == null)
                    return;

                this.AddInvasionMessageAsync(rtb, datas);
            }));
        }
        void autoPrint(System.Drawing.Image image)
        {
            videoImageNow = image;

            try
            {
                if (cbautoprint.IsChecked==true)
                {
                    printDocument.Print();
                }

            }
            catch
            {   // 停止打印
                printDocument.PrintController.OnEndPrint(printDocument, new System.Drawing.Printing.PrintEventArgs());
            }

        }
        System.Drawing.Image videoImageNow = null;

       
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (videoImageNow == null) return;
            System.Drawing.Image bmp = videoImageNow;
            int x = e.MarginBounds.X;
            int y = e.MarginBounds.Y;
            int width = (int)bmp.Width / 2;
            int height = (int)bmp.Height / 2;
            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(x, y, width, height);
            e.Graphics.DrawImage(bmp, destRect, 0, 0, bmp.Width, bmp.Height, System.Drawing.GraphicsUnit.Pixel);
        }

        private async Task AddInvasionMessageAsync(RichTextBox rtb_Msg, List<ReceivedMessageEntity> datas)
        {
            if (rtb_Msg == null || datas == null)
                return;

            foreach (var item in datas)
            {
                var pline = new Paragraph();
                var addredd = new Run($"{item.PositionName}          ");
                addredd.Foreground = Brushes.Green;
                var count = new Run("车速 " + $"{item.InvasionCount}" + "km/h   ");
                count.Foreground = Brushes.Red;
                var classstr = new Run(item.Time.ToString("yyyy-MM-dd HH:mm:ss"));
                if (item.PositionID != 6)
                {
                     count = new Run($"{item.InvasionCount}");
                     classstr = new Run(item.InvasionType.GetDescription() + item.Time.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                classstr.Foreground = Brushes.Green;
                pline.Inlines.Add(addredd);
                pline.Inlines.Add(count);
                pline.Inlines.Add(classstr);

                rtb_Msg.Document.Blocks.Add(pline);
                rtb_Msg.ScrollToEnd();


                if (item.PositionID == 6)
                {
                    Console.WriteLine(item.PositionID + "");
                    return;
                }
             

                if (cbautoprint.IsChecked == true)
                {
                    var url = new Uri(GlobalConfig.HostServer + "/static/" + item.ImageFileName);
                    System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                    var response = await httpClient.GetAsync(url);
                    if (response != null)
                    {
                        var responseStrream = await response.Content.ReadAsStreamAsync();
                        if (responseStrream == null)
                            return;
                        System.Drawing.Image image = System.Drawing.Image.FromStream(responseStrream);
                        autoPrint(image);
                    }
                }
                else
                {
                    // 每个入侵弹出一个框
                    WarningHandle warningHandle = new WarningHandle() { Data = item };
                    warningHandle.Show();
                }

                
            }
        }

        void mainInit()
        {
            myFrame.Content = new mainPage();
            cboSelectPage.Items.Add("AI管理");
            cboSelectPage.Items.Add("相机1");
            cboSelectPage.Items.Add("相机2");
            cboSelectPage.Items.Add("相机3");
            cboSelectPage.Items.Add("相机4");
            cboSelectPage.Items.Add("相机5");
            cboSelectPage.Items.Add("相机6");
            cboSelectPage.Items.Add("NVR管理");
            cboSelectPage.Items.Add("纯监控相机");

            cboSelectPage.SelectionChanged += CboSelectPage_SelectionChanged;

            cbautoprint.MouseEnter += (x, y) =>
            {
                cbautoprint.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            };
            cbautoprint.MouseLeave += (x, y) =>
            {
                cbautoprint.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ED5FF"));
            };



            minim.MouseEnter += (x, y) =>
            {
                minim.Source = new BitmapImage(new Uri("resources/image/minimenter.png", UriKind.Relative));
            };
            minim.MouseLeave += (x, y) =>
            {
                minim.Source = new BitmapImage(new Uri("resources/image/minim.png", UriKind.Relative));
            };
            minim.PreviewMouseLeftButtonDown += (x, y) =>
            {
                this.WindowState = WindowState.Minimized;
            };
            max.MouseEnter += (x, y) =>
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    max.Source = new BitmapImage(new Uri("resources/image/maxenter.png", UriKind.Relative));
                }
                else if (this.WindowState == WindowState.Normal)
                {
                    max.Source = new BitmapImage(new Uri("resources/image/normalenter.png", UriKind.Relative));
                }
            };
            max.MouseLeave += (x, y) =>
            {

                if (this.WindowState == WindowState.Maximized)
                {
                    max.Source = new BitmapImage(new Uri("resources/image/max.png", UriKind.Relative));
                }
                else if (this.WindowState == WindowState.Normal)
                {
                    max.Source = new BitmapImage(new Uri("resources/image/normal.png", UriKind.Relative));
                }
            };
            max.PreviewMouseLeftButtonDown += (x, y) =>
            {
                this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
                if (this.WindowState == WindowState.Maximized)
                {
                    max.Source = new BitmapImage(new Uri("resources/image/max.png", UriKind.Relative));
                }
                else if (this.WindowState == WindowState.Normal)
                {
                    max.Source = new BitmapImage(new Uri("resources/image/normal.png", UriKind.Relative));
                }
            };
            close.MouseEnter += (x, y) =>
            {
                close.Source = new BitmapImage(new Uri("resources/image/closeenter.png", UriKind.Relative));
            };
            close.MouseLeave += (x, y) =>
            {
                close.Source = new BitmapImage(new Uri("resources/image/close.png", UriKind.Relative));
            };
            close.PreviewMouseLeftButtonDown += (x, y) =>
            {
                this.Close();
                Environment.Exit(-1);
            };
            logout.MouseEnter += (x, y) =>
            {
                logouticon.Source = new BitmapImage(new Uri("resources/image/logoutenter.png", UriKind.Relative));
                logoutlabel.Foreground = System.Windows.Media.Brushes.Red;
            };
            logout.MouseLeave += (x, y) =>
            {
                logouticon.Source = new BitmapImage(new Uri("resources/image/loginout.png", UriKind.Relative));
                logoutlabel.Foreground = System.Windows.Media.Brushes.White;
            };
            logout.PreviewMouseLeftButtonDown += (x, y) =>
            {
                gridLogin.Visibility = Visibility.Visible;
                gridLoginIn.Visibility = Visibility.Hidden;
            };
        }
        void loginInit()
        {
            btnLogin.MouseEnter += (x, y) =>
            {
                btnLogin.Opacity = 0.8;

            };
            btnLogin.MouseLeave += (x, y) =>
            {
                btnLogin.Opacity = 1;
            };

            btnLogin.MouseLeftButtonDown += (x, y) =>
            {
                btnLogin.Opacity = 0.5;
            };
            btnLogin.MouseLeftButtonUp += async (x, y) =>
            {
                if (string.IsNullOrEmpty(userbox.Text) || string.IsNullOrEmpty(pwdbox.Password))
                {
                    MessageBox.Show("请先输入用户名和密码", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                CommunicationMsg.Login login = new CommunicationMsg.Login()
                {
                    Account = userbox.Text,
                    Password = pwdbox.Password,
                };

                var jsonStr = JsonConvert.SerializeObject(login);
                if (string.IsNullOrEmpty(jsonStr))
                    return;

               // var response = await httpClient.PostAsync($"{GlobalConfig.HostServer}/Login", new JsonContent(jsonStr));
                //if (response.IsSuccessStatusCode)
                   if (userbox.Text == "admin" || pwdbox.Password == "admin")
                {
                    btnLogin.Opacity = 1;
                    gridLogin.Visibility = Visibility.Hidden;
                    gridLoginIn.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("登录失败，请确认用户名和密码是否在正确", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

               
            };
            minimlogin.MouseEnter += (x, y) =>
            {
                minim.Source = new BitmapImage(new Uri("resources/image/minimenter.png", UriKind.Relative));
            };
            minimlogin.MouseLeave += (x, y) =>
            {
                minim.Source = new BitmapImage(new Uri("resources/image/minim.png", UriKind.Relative));
            };
            minimlogin.PreviewMouseLeftButtonDown += (x, y) =>
            {
                this.WindowState = WindowState.Minimized;
            };
            maxlogin.MouseEnter += (x, y) =>
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    maxlogin.Source = new BitmapImage(new Uri("resources/image/maxenter.png", UriKind.Relative));
                }
                else if (this.WindowState == WindowState.Normal)
                {
                    maxlogin.Source = new BitmapImage(new Uri("resources/image/normalenter.png", UriKind.Relative));
                }

            };
            maxlogin.MouseLeave += (x, y) =>
            {

                if (this.WindowState == WindowState.Maximized)
                {
                    maxlogin.Source = new BitmapImage(new Uri("resources/image/max.png", UriKind.Relative));
                }
                else if (this.WindowState == WindowState.Normal)
                {
                    maxlogin.Source = new BitmapImage(new Uri("resources/image/normal.png", UriKind.Relative));
                }
            };
            maxlogin.PreviewMouseLeftButtonDown += (x, y) =>
            {
                this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
                if (this.WindowState == WindowState.Maximized)
                {
                    maxlogin.Source = new BitmapImage(new Uri("resources/image/max.png", UriKind.Relative));
                }
                else if (this.WindowState == WindowState.Normal)
                {
                    maxlogin.Source = new BitmapImage(new Uri("resources/image/normal.png", UriKind.Relative));
                }
            };
            closelogin.MouseEnter += (x, y) =>
            {
                closelogin.Source = new BitmapImage(new Uri("resources/image/closeenter.png", UriKind.Relative));
            };
            closelogin.MouseLeave += (x, y) =>
            {
                closelogin.Source = new BitmapImage(new Uri("resources/image/close.png", UriKind.Relative));
            };
            closelogin.PreviewMouseLeftButtonDown += (x, y) =>
            {
                this.Close();
                Environment.Exit(-1);
            };

            gridLogin.PreviewMouseLeftButtonDown += (x, y) =>
            {
                if (dragflag)
                    DragMove();
            };
            topGrid.PreviewMouseLeftButtonDown += (x, y) =>
            {
                if (dragflagin)
                    DragMove();
            };

            gridtopright.MouseEnter += (x, y) =>
            {
                dragflagin = false;
            };
            gridtopright.MouseLeave += (x, y) =>
            {
                dragflagin = true;
            };
            loginbox.MouseEnter += (x, y) =>
            {
                dragflag = false;
            };
            loginbox.MouseLeave += (x, y) =>
            {
                dragflag = true;
            };
            userbox.MouseEnter += (x, y) =>
            {
                if (clearinputuserflag)
                {
                    userbox.Text = "";
                    clearinputuserflag = false;
                }
                userbox.Focus();
                userbox.Opacity = 0.5;
            };
            pwdgrid.MouseEnter += (x, y) =>
            {
                if (clearinputpwdflag)
                {
                    pwdboxtxt.Text = "";
                    pwdboxtxt.Visibility = Visibility.Hidden;
                    pwdbox.Visibility = Visibility.Visible;
                    clearinputpwdflag = false;
                }
                pwdbox.Focus();
                pwdbox.Opacity = 0.5;
            };
        }





        private void CboSelectPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ;
            int flag = cboSelectPage.SelectedIndex;
            switch (flag)
            {
 
                case 0:
                    myFrame.Content = new mainPage();
                    break;
                case 1:
                    myFrame.Content = myClass.data.cameraPage1;
                    break;
                case 2:
                    myFrame.Content = myClass.data.cameraPage2;
                    break;
                case 3:
                    myFrame.Content = myClass.data.cameraPage3;
                    break;
                case 4:
                    myFrame.Content = myClass.data.cameraPage4;
                    break;
                case 5:
                    myFrame.Content = myClass.data.cameraPage5;
                    break;
                case 6:
                    myFrame.Content = myClass.data.cameraPage6;
                    break;
                case 7:
                    myFrame.Content = myClass.data.cameraPageNVR;
                    break;
                case 8:
                    myFrame.Content = myClass.data.cameraPageView;
                    break;
                default:
                    break;
            }
        }


    }
}
