﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AIVisualwfpnew.myPage
{
    /// <summary>
    /// cameraPage1.xaml 的交互逻辑
    /// </summary>
    public partial class cameraPage1 : Page
    {
        public cameraPage1()
        {
            InitializeComponent();
            cameraBrowser1.ObjectForScripting = true;
            
            //cameraBrowser1.Navigate(new Uri("http://127.0.0.1/"));
        }
    }
}
