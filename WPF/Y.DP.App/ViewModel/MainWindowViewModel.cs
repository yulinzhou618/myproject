using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Y.DP.App.myClass;
using Y.DP.App.UserControls;
using Y.DP.App.Windows;

namespace Y.DP.App.ViewModel
{
    class MainWindowViewModel : PropertyChangeBase
    {
        MainWindow mainWindow;
        PositionControl[] positionArr = new PositionControl[13];
        //Path[] IconLightArr = new Path[13];
        BitmapImage bigImage;
        Visibility position_V = Visibility.Visible;
        Visibility position_DetailsV = Visibility.Collapsed;
        string trainID="123";
        public BitmapImage BigImage { get => bigImage; set { bigImage = value; PropertyChangedBaseEx.OnPropertyChanged(this, p => this.BigImage); } }
        public Visibility Position_DetailsV { get => position_DetailsV; set { position_DetailsV = value; PropertyChangedBaseEx.OnPropertyChanged(this, p => this.Position_DetailsV); } }
        public Visibility Position_V { get => position_V; set { position_V = value; PropertyChangedBaseEx.OnPropertyChanged(this, p => this.Position_V); } }
        public string TrainID { get => trainID; set { trainID = value;  OnPropertyChanged(TrainID); } }

        public MainWindowViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            initPosition();
            Task.Factory.StartNew(whileRun);
            BigImage = new BitmapImage();
            BigImage.BeginInit();
            BigImage.UriSource = new Uri("../Resources/Image/Other/normal.png", UriKind.RelativeOrAbsolute);
            BigImage.EndInit();
        }
        void initPosition()
        {
            for (int i = 0; i < 13; i++)
            {
                int index = i + 1;
                positionArr[i] = (PositionControl)mainWindow.FindName("position_" + index);
                positionArr[i].PreviewMouseDoubleClick += (x, y) => {
                    PositionControl positionControl = (PositionControl)x;
                    Position_V = Visibility.Collapsed;
                    Position_DetailsV = Visibility.Visible;
                    PositionDetailsControl Position_Details = (PositionDetailsControl)mainWindow.FindName("position_Details");
                    Position_Details.trainID.Content = positionControl.Name;
                };
            }
        }
        public ICommand smallCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.mainWindow.WindowState = WindowState.Minimized;
                }, () => true);
            }
        }
        bool bigFlag = true;
        public ICommand normalCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.mainWindow.WindowState = this.mainWindow.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        Image btnMouseLeave = (Image)mainWindow.FindName("btnMouseLeave");
                        if (bigFlag)
                        {
                            BigImage = new BitmapImage();
                            BigImage.BeginInit();
                            BigImage.UriSource = new Uri("../Resources/Image/Other/big.png", UriKind.RelativeOrAbsolute);
                            BigImage.EndInit();
                            bigFlag = false;
                        }
                        else
                        {
                            BigImage = new BitmapImage();
                            BigImage.BeginInit();
                            BigImage.UriSource = new Uri("../Resources/Image/Other/normal.png", UriKind.RelativeOrAbsolute);
                            BigImage.EndInit();
                            bigFlag = true;
                        }

                    });
                }, () => true);
            }
        }
        public ICommand closeCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Environment.Exit(-1);
                }, () => true);
            }
        }
        public ICommand loginCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    new LoginWindow().ShowDialog();
                }, () => true);
            }
        }
        public ICommand trainclickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MessageBox.Show("trainclickCommand");
                }, () => true);
            }
        }
        public ICommand backCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Position_V = Visibility.Visible;
                    Position_DetailsV = Visibility.Collapsed;
                }, () => true);
            }
        }
        void whileRun()
        {
            while (true)
            {
                Thread.Sleep(500);
                App.Current.Dispatcher.Invoke(() =>
                {
                   // positionArr[3].rectangle.Stroke = positionArr[3].rectangle.Stroke == Brushes.Red ? Brushes.Green : Brushes.Red;
                    positionArr[0].IconLight.Fill = positionArr[0].IconLight.Fill == Brushes.Gray ? Brushes.Red : Brushes.Gray;
                    positionArr[1].IconLight.Fill = positionArr[1].IconLight.Fill == Brushes.Gray ? Brushes.Red : Brushes.Gray;
                    positionArr[2].IconLight.Fill = positionArr[2].IconLight.Fill == Brushes.Gray ? Brushes.Red : Brushes.Gray;
                    positionArr[3].IconLight.Fill = positionArr[3].IconLight.Fill == Brushes.Gray ? Brushes.Red : Brushes.Gray;
                    positionArr[4].IconLight.Fill = positionArr[4].IconLight.Fill == Brushes.Gray ? Brushes.Red : Brushes.Gray;
                    positionArr[5].IconLight.Fill = positionArr[5].IconLight.Fill == Brushes.Gray ? Brushes.Red : Brushes.Gray;
                    positionArr[6].IconLight.Fill = positionArr[6].IconLight.Fill == Brushes.Gray ? Brushes.Red : Brushes.Gray;
                    positionArr[7].IconLight.Fill = positionArr[7].IconLight.Fill == Brushes.Gray ? Brushes.Red : Brushes.Gray;
                    positionArr[8].IconLight.Fill = positionArr[8].IconLight.Fill == Brushes.Gray ? Brushes.Red : Brushes.Gray;
                    positionArr[9].IconLight.Fill = positionArr[9].IconLight.Fill == Brushes.Gray ? Brushes.Red : Brushes.Gray;
                    positionArr[10].IconLight.Fill = positionArr[10].IconLight.Fill == Brushes.Gray ? Brushes.Red : Brushes.Gray;
                    positionArr[11].IconLight.Fill = positionArr[11].IconLight.Fill == Brushes.Gray ? Brushes.Red : Brushes.Gray;
                    positionArr[12].IconLight.Fill = positionArr[12].IconLight.Fill == Brushes.Gray ? Brushes.Red : Brushes.Gray;
                  
                    positionArr[12].train.Visibility = Visibility.Collapsed  ;
                    positionArr[12].trainNumer.Visibility = Visibility.Collapsed;
                    positionArr[12].portrait.Visibility = Visibility.Collapsed;
                    positionArr[12].portraitNumber.Visibility = Visibility.Collapsed;
                    positionArr[12].electricIcon.Visibility = Visibility.Collapsed;
                    positionArr[12].electricLabel.Visibility = Visibility.Collapsed;
                    positionArr[12].lineNumber.Visibility = Visibility.Collapsed;
                    
                });
            }
        }
    }
}

