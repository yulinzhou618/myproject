using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Y.DP.App.Windows
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            ContentRendered += LoginWindow_ContentRendered;
        }

        private void LoginWindow_ContentRendered(object sender, EventArgs e)
        {
            Cancle.Click += (x, y) => { this.Close(); };
            Login.Click += (x, y) => { MessageBox.Show(passBox.Password); };
        }
    }
}
