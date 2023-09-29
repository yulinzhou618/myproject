using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace AIVisualwfpnew.myPage
{
    /// <summary>
    /// oneCameraPage.xaml 的交互逻辑
    /// </summary>
    public partial class oneCameraPage : Page
    {
        public oneCameraPage()
        {
            InitializeComponent();

            oneCameraImage.MouseLeftButtonDown += (x, y) =>
            {
                if (y.ClickCount == 2)
                    myClass.data.whileflag = "sixCameraPage";
            };
            Task.Factory.StartNew(whileRun);
        }
        void whileRun()
        {
            while (true)
            {
                Thread.Sleep(100);
                this.Dispatcher.Invoke((Action)(() =>
                {

                    camerabig.Background = myClass.data.camerabig.Background;

                }));
            }
        }
    }
}
