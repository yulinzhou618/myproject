using System;
using System.Windows;

namespace AIVisualwfpnew.myPage
{
    /// <summary>
    /// searchWindow.xaml 的交互逻辑
    /// </summary>
    public partial class searchWindow : Window
    {
        public searchWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ContentRendered += SearchWindow_ContentRendered;
        }
        bool dragflag = true;
        private void SearchWindow_ContentRendered(object sender, EventArgs e)
        {
            searchwindowclose.MouseEnter += (x, y) =>
            {
                searchwindowclose.Opacity = 0.8;
                dragflag = false;
            };
            searchwindowclose.MouseLeave += (x, y) =>
            {
                searchwindowclose.Opacity = 1;
                dragflag = true;
            };

            searchwindowclose.MouseLeftButtonDown += (x, y) =>
            {
                searchwindowclose.Opacity = 0.5;
            };
            searchwindowclose.MouseLeftButtonUp += (x, y) =>
            {
                searchwindowclose.Opacity = 1;
                this.Close();
            };

            btntitle.PreviewMouseLeftButtonDown += (x, y) =>
            {
                if (dragflag)
                    DragMove();
            };
        }
    }
}
