using AIVisualwfpnew.Helpers;
using System;
using System.Windows;

namespace AIVisualwfpnew
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            // 开启推送连接
            PushManager.Current.StartClient();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject != null && e.ExceptionObject is Exception ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
            }
            else
            {
                LogHelper.Log.Error("发生全局未处理异常");
            }
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            if (e.Exception == null)
            {
                LogHelper.Log.Error("发生全局未处理异常");
                return;
            }
            else
            {
                LogHelper.Log.Error(e.Exception.Message, e.Exception);
            }
        }
    }
}
