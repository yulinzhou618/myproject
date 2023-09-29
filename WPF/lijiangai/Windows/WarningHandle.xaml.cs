using AIVisualwfpnew.CommunicationMsg.PushMsg;
using AIVisualwfpnew.Entitys;
using AIVisualwfpnew.Helpers;
using AIVisualwfpnew.myClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AIVisualwfpnew.Windows
{
    /// <summary>
    /// WarningHandle.xaml 的交互逻辑
    /// </summary>
    public partial class WarningHandle : Window, INotifyPropertyChanged
    {
        public WarningHandle()
        {
            PrintCommand = new RelayCommand(PrintCommandHandler);
            InitializeComponent();
            this.Loaded += WarningHandle_Loaded;
        }

        private async void WarningHandle_Loaded(object sender, RoutedEventArgs e)
        {
            var url = new Uri(GlobalConfig.HostServer + "/static/" + Data.ImageFileName);
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return;

            var responseStrream = await response.Content.ReadAsStreamAsync();
            if (responseStrream == null)
                return;

            BitmapImage bitmap = new BitmapImage();
            using (responseStrream)
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = responseStrream;
                bitmap.EndInit();
                bitmap.Freeze();
            }

            img.Source = bitmap;
        }

        private void PrintCommandHandler(object obj)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog().Value)
                printDialog.PrintVisual(img, "打印图片");
        }

        private ReceivedMessageEntity _data;

        public ReceivedMessageEntity Data
        {
            get { return _data; }
            set { _data = value; NotifyPropertyChanged(); }
        }

        public ICommand PrintCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
