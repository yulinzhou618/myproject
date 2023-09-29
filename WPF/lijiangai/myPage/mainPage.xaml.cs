using AIVisualwfpnew.Helpers;
using AIVisualwfpnew.myClass;
using AIVisualwfpnew.resources;
using AIVisualwfpnew.Windows;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AIVisualwfpnew.myPage
{
    /// <summary>
    /// mainPage.xaml 的交互逻辑
    /// </summary>
    public partial class mainPage : Page
    {
        bool videoable = false;
        videoClient videoClient = new videoClient();
        /// <summary>
        /// 轮对棚喊麦按钮处理事件
        /// </summary>
        public ICommand LunDuiPengVoiceCommand { get; set; }

        /// <summary>
        /// 出入段喊麦按钮点击处理事件
        /// </summary>
        public ICommand ChuRuDuanVoiceCommand { get; set; }

        public mainPage()
        {
            LunDuiPengVoiceCommand = new RelayCommand(LunDuiPengVoiceHandler);
            ChuRuDuanVoiceCommand = new RelayCommand(ChuRuDuanVoiceHandler);
            InitializeComponent();
            cbalarm.MouseEnter += (x, y) =>
            {
                cbalarm.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            };
            cbalarm.MouseLeave += (x, y) =>
            {
                cbalarm.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2ED5FF"));
            };
            searchgrid.MouseEnter += (x, y) =>
            {
                searchgrid.Opacity = 0.8;
            };
            searchgrid.MouseLeave += (x, y) =>
            {
                searchgrid.Opacity = 1;
            };
            searchgrid.MouseDown += (x, y) =>
            {
                searchgrid.Opacity = 0.5;
                searchWindow sw = new searchWindow();
                sw.Owner = Window.GetWindow(this);
                sw.ShowDialog();


            };
            searchgrid.MouseUp += (x, y) =>
            {
                searchgrid.Opacity = 1;
            };

            settimegrid.MouseEnter += (x, y) =>
            {
                settimegrid.Opacity = 0.8;
            };
            settimegrid.MouseLeave += (x, y) =>
            {
                settimegrid.Opacity = 1;
            };
            settimegrid.MouseDown += (x, y) =>
            {
                settimegrid.Opacity = 0.5;
                NewTimeSettingWindow newTimeSetting = new NewTimeSettingWindow();
                newTimeSetting.Owner = Window.GetWindow(this);
                newTimeSetting.ShowDialog();
            };
            settimegrid.MouseUp += (x, y) =>
            {
                settimegrid.Opacity = 1;
            };

            btnVideogrid.PreviewMouseLeftButtonDown += (x, y) =>
            {

                videoable = !videoable;

                if (videoable)
                {
                    btnVideoshow.Source = new BitmapImage(new Uri("../resources/image/videonone.png", UriKind.Relative));
                    myClass.data.sixCameraPage.draw();
                }
                else
                {
                    videoClient.stop();
                    btnVideoshow.Source = new BitmapImage(new Uri("../resources/image/video.png", UriKind.Relative));
                    myClass.data.sixCameraPage.drawstop();

                }
            };


            cameraContent.Content = myClass.data.sixCameraPage;
            Task.Factory.StartNew(whileRun);
        }

        /// <summary>
        /// 指示出入段音柱是否正在语音喊话
        /// </summary>
        private SpeakerResponseData _churuduanstartinfo;

        private async void ChuRuDuanVoiceHandler(object obj)
        {
            if (!(obj is Button btn))
                return;

            var _ischuruduanspeaking = AttachPropertys.GetIsChecked(btn);
            if (_ischuruduanspeaking)
            {
                if (_churuduanstartinfo == null)
                    return;

                var temp = await SpeakerHelper.StopAudioBroadcast(_churuduanstartinfo.Number, _churuduanstartinfo.GUID);
                if (temp == null)
                {
                    MessageBox.Show("轮对检测棚音柱停止喊话失败", "失败", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (temp.State == 0)
                {
                    MessageBox.Show("轮对检测棚音柱停止喊话失败", "失败", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _churuduanstartinfo = null;
                AttachPropertys.SetIsChecked(btn, false);
            }
            else
            {
                var param = new List<string>() { GlobalConfig.ChuRuDuanSpeakerID };
                _churuduanstartinfo = await SpeakerHelper.StartAudioBroadcast(param);
                if (_churuduanstartinfo == null)
                {
                    MessageBox.Show("轮对检测棚音柱喊话失败", "失败", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (_churuduanstartinfo.State == 0)
                {
                    MessageBox.Show("轮对检测棚音柱喊话失败", "失败", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                AttachPropertys.SetIsChecked(btn, true);
            }
        }

        /// <summary>
        /// 指示轮对棚音柱是否正在语音喊话
        /// </summary>
        private SpeakerResponseData _lunduipengstartinfo;
        private async void LunDuiPengVoiceHandler(object obj)
        {
            if (!(obj is Button btn))
                return;

            var _islunduipengspeaking = AttachPropertys.GetIsChecked(btn);
            if (_islunduipengspeaking)
            {
                if (_lunduipengstartinfo == null)
                    return;

                var temp = await SpeakerHelper.StopAudioBroadcast(_lunduipengstartinfo.Number, _lunduipengstartinfo.GUID);
                if (temp == null)
                {
                    MessageBox.Show("轮对检测棚音柱停止喊话失败", "失败", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (temp.State == 0)
                {
                    MessageBox.Show("轮对检测棚音柱停止喊话失败", "失败", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _lunduipengstartinfo = null;
                AttachPropertys.SetIsChecked(btn, false);
            }
            else
            {
                var param = new List<string>() { GlobalConfig.LunDuiPengSpeakerID };
                _lunduipengstartinfo = await SpeakerHelper.StartAudioBroadcast(param);
                if (_lunduipengstartinfo == null)
                {
                    MessageBox.Show("轮对检测棚音柱喊话失败", "失败", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (_lunduipengstartinfo.State == 0)
                {
                    MessageBox.Show("轮对检测棚音柱喊话失败", "失败", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                AttachPropertys.SetIsChecked(btn, true);
            }
        }

        void whileRun()
        {
            while (true)
            {
                Thread.Sleep(100);
                this.Dispatcher.Invoke((Action)(() =>
                {
                    switch (myClass.data.whileflag)
                    {
                        case "sixCameraPage":
                            cameraContent.Content = myClass.data.sixCameraPage;
                            break;
                        case "CameraPage":
                            cameraContent.Content = myClass.data.oneCameraPage;
                            break;
                        default:
                            break;
                    }
                    myClass.data.whileflag = null;
                }));
            }
        }
        void addRtb_Msg(string address, string countstr, string claserstr)
        {
            var pline = new Paragraph();
            var addredd = new Run(address);
            addredd.Foreground = Brushes.Green;
            var count = new Run(countstr);
            count.Foreground = Brushes.Red;
            var classstr = new Run(claserstr);
            classstr.Foreground = Brushes.Green;

            pline.Inlines.Add(addredd);
            pline.Inlines.Add(count);
            pline.Inlines.Add(classstr);

            rtb_Msg.Document.Blocks.Add(pline);
            rtb_Msg.ScrollToEnd();
        }

        private void BtnPainting_Click(object sender, RoutedEventArgs e)
        {
            PaintingAreaWindow paintingAreaWindow = new PaintingAreaWindow();
            paintingAreaWindow.Owner = Application.Current.MainWindow;
            paintingAreaWindow.ShowDialog();
        }
    }
}
