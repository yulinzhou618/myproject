using AIVisualwfpnew.CommunicationMsg;
using AIVisualwfpnew.Entitys;
using AIVisualwfpnew.Helpers;
using AIVisualwfpnew.myClass;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

namespace AIVisualwfpnew.Windows
{
    /// <summary>
    /// PaintingAreaWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PaintingAreaWindow : Window
    {
        /// <summary>
        /// 私有类，用于零时记录每个位置的数据库ID，数据库存储的配置信息，当前界面绘制的配置信息，图片原始像素大小。用于后面配置信息和界面信息的转换使用
        /// </summary>
        private class PositionZoneConfigInfo
        {
            public int PositionID { get; set; }

            /// <summary>
            /// 图片原始宽度
            /// </summary>
            public int ImageWidth { get; set; }

            /// <summary>
            /// 图片原始高度
            /// </summary>
            public int ImageHeight { get; set; }

            /// <summary>
            /// 宽度比例 （图片原始高度 / 图片UI高度)
            /// </summary>
            public double WidthScale { get; set; }

            /// <summary>
            /// 高度比例  （图片原始高度 / 图片UI高度）
            /// </summary>
            public double HeightScale { get; set; }

            public PointCollection UIPoints { get; set; }

            public PointCollection ServerConfigPoints { get; set; }
        }

        /// <summary>
        /// 要绘制的图片数据源，用于界面绑定。
        /// </summary>
        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(PaintingAreaWindow), new PropertyMetadata(null));

        public static int GetBtnOrderIndex(DependencyObject obj)
        {
            return (int)obj.GetValue(BtnOrderIndexProperty);
        }

        public static void SetBtnOrderIndex(DependencyObject obj, int value)
        {
            obj.SetValue(BtnOrderIndexProperty, value);
        }

        /// <summary>
        /// 位置按钮顺序
        /// </summary>
        public static readonly DependencyProperty BtnOrderIndexProperty =
            DependencyProperty.RegisterAttached("BtnOrderIndex", typeof(int), typeof(PaintingAreaWindow), new PropertyMetadata(-1));

        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime DateNow
        {
            get { return (DateTime)GetValue(DateNowProperty); }
            set { SetValue(DateNowProperty, value); }
        }

        public static readonly DependencyProperty DateNowProperty =
            DependencyProperty.Register("DateNow", typeof(DateTime), typeof(PaintingAreaWindow), new PropertyMetadata(DateTime.Now));

        /// <summary>
        /// 选中区域的名称，用于界面显示
        /// </summary>
        public string SelectedAreaName
        {
            get { return (string)GetValue(SelectedAreaNameProperty); }
            set { SetValue(SelectedAreaNameProperty, value); }
        }

        public static readonly DependencyProperty SelectedAreaNameProperty =
            DependencyProperty.Register("SelectedAreaName", typeof(string), typeof(PaintingAreaWindow), new PropertyMetadata(null));

        public static string GetBtnAreaName(DependencyObject obj)
        {
            return (string)obj.GetValue(BtnAreaNameProperty);
        }

        public static void SetBtnAreaName(DependencyObject obj, string value)
        {
            obj.SetValue(BtnAreaNameProperty, value);
        }

        /// <summary>
        /// 附加属性，用于标识按钮对应的区域的名称。
        /// </summary>
        public static readonly DependencyProperty BtnAreaNameProperty =
            DependencyProperty.RegisterAttached("BtnAreaName", typeof(string), typeof(PaintingAreaWindow), new PropertyMetadata(null));

        /// <summary>
        /// 当前选中的位置按钮序号
        /// </summary>
        public int CurrentOrderIndex
        {
            get { return (int)GetValue(CurrentOrderIndexProperty); }
            set { SetValue(CurrentOrderIndexProperty, value); }
        }

        public static readonly DependencyProperty CurrentOrderIndexProperty =
            DependencyProperty.Register("CurrentOrderIndex", typeof(int), typeof(PaintingAreaWindow), new PropertyMetadata(-1, new PropertyChangedCallback(CurrentOrderIndexChangedHandler)));

        private async static void CurrentOrderIndexChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is PaintingAreaWindow pw))
                return;

            // 判断当前位置是否已有入侵区域数据，有的话加载到界面上去。
            if (!(e.NewValue is int nv))
                return;

            var temp = pw.positionConfigs.FirstOrDefault(c => c.PositionID == nv);
            if (temp == null)
                return;

            try
            {
                var btns = UIHelper.GetChild<Button>(pw);
                if (btns != null)
                {
                    foreach (var btn in btns)
                    {
                        var btnAreaName = PaintingAreaWindow.GetBtnAreaName(btn);
                        var btnAreaIndex = PaintingAreaWindow.GetBtnOrderIndex(btn);
                        if (string.IsNullOrEmpty(btnAreaName) || btnAreaIndex < 0)
                            continue;

                        if (btnAreaIndex == nv)
                        {
                            pw.SelectedAreaName = btnAreaName;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error($"区域入侵绘制界面，设置区域名称时异常：{ex.Message}",ex);
            }

            // TODO: 加载指定位置的图片
            try
            {
                GetImageStream imageStream = new GetImageStream() { PositionID = nv + 1 };
                var url = new Uri(GlobalConfig.HostServer + "/GetPositionImage");
                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                var response = await httpClient.PostAsync(url, new JsonContent(JsonConvert.SerializeObject(imageStream)));
                if (!response.IsSuccessStatusCode)
                {
                    pw.polyline.Points.Clear();
                    pw.Image = null;
                    return;
                }

                var imgbytes = await response.Content.ReadAsStreamAsync();
                using (imgbytes)
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = imgbytes;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    // 获取图片原始分辨率，用于后续计算绘制点最终坐标位置。
                    temp.ImageWidth = bitmap.PixelWidth;
                    temp.ImageHeight = bitmap.PixelHeight;

                    pw.Image = bitmap;
                }
            }
            catch (Exception ex)
            {
                pw.polyline.Points.Clear();
                pw.Image = null;
                LogHelper.Log.Error(ex.Message, ex);
                return;
            }

            if (pw.polyline == null)
            {
                pw.polyline = NewPolyLine();
                pw.canv.Children.Clear();
                pw.canv.Children.Add(pw.polyline);
            }
            else
                pw.polyline.Points.Clear();

            if (temp.UIPoints == null)
                return;

            while (double.IsNaN(pw.img.ActualWidth)||double.IsInfinity(pw.img.ActualWidth)||pw.img.ActualWidth==0)
                await Task.Delay(100);

            if (temp.UIPoints.Count <= 0 && temp.ServerConfigPoints != null && temp.ServerConfigPoints.Count > 0)
            {
                // 将服务端配置的区域节点数据转换为以当前界面宽高的UI点位信息。
                temp.WidthScale = temp.ImageWidth / pw.img.ActualWidth;
                temp.HeightScale = temp.ImageHeight / pw.img.ActualHeight;
                System.Diagnostics.Trace.WriteLine($"原始大小和展示大小的宽度比例为：{temp.WidthScale}，高度比为：{temp.HeightScale}");
                foreach (var item in temp.ServerConfigPoints)
                    temp.UIPoints.Add(new Point() { X = item.X / temp.WidthScale, Y = item.Y / temp.HeightScale });
            }

            // 如果只有四个点位，需要将最后将第一个点位添加到最后一个点位上去，形成闭合
            if (temp.UIPoints.Count == 4)
            {
                var firstPoint = temp.UIPoints.FirstOrDefault();
                temp.UIPoints.Add(new Point { X = firstPoint.X,Y = firstPoint.Y});
            }

            foreach (var point in temp.UIPoints)
                pw.polyline.Points.Add(point);
        }

        public ICommand PositionBtnClickCommand { get; set; }

        public ICommand LeftBtnClickCommand { get; set; }

        public ICommand RightBtnClickCommand { get; set; }

        public ICommand StartPaintCommand { get; set; }

        /// <summary>
        /// 重绘
        /// </summary>
        public ICommand RepaintCommand { get; set; }

        /// <summary>
        /// 保存
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// 绘制按钮文字
        /// </summary>
        public string PaintBtnContent
        {
            get { return (string)GetValue(PaintBtnContentProperty); }
            set { SetValue(PaintBtnContentProperty, value); }
        }

        public static readonly DependencyProperty PaintBtnContentProperty =
            DependencyProperty.Register("PaintBtnContent", typeof(string), typeof(PaintingAreaWindow), new PropertyMetadata("绘 制"));

        private int _paintingClickCount = 0;

        /// <summary>
        /// 是否正在绘制
        /// </summary>
        public bool IsPainting
        {
            get { return (bool)GetValue(IsPaintingProperty); }
            set { SetValue(IsPaintingProperty, value); }
        }

        public static readonly DependencyProperty IsPaintingProperty =
            DependencyProperty.Register("IsPainting", typeof(bool), typeof(PaintingAreaWindow), new PropertyMetadata(false, new PropertyChangedCallback(IspaitingChangedHandler)));

        private static void IspaitingChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is PaintingAreaWindow pw))
                return;

            if (e.NewValue is bool pt)
            {
                if (pt)
                {
                    pw.PaintBtnContent = "停止绘制";
                    pw.polyline = NewPolyLine();
                    pw.canv.Children.Clear();
                    pw.canv.Children.Add(pw.polyline);
                }
                else
                {
                    pw.PaintBtnContent = "绘 制";
                    pw._paintingClickCount = 0;
                }
            }
        }

        private static Polyline NewPolyLine()
        {
            return new Polyline() { Stroke = new SolidColorBrush(Color.FromRgb(46, 213, 255)), StrokeThickness = 2, StrokeDashArray = new DoubleCollection() { 2, 2 } };
        }

        private Polyline polyline = null;

        /// <summary>
        /// 记录每个位置的摄像机的入侵区域绘制信息（）
        /// </summary>
        //private Dictionary<int, PointCollection> positionPoints = new Dictionary<int, PointCollection>();

        private List<PositionZoneConfigInfo> positionConfigs = new List<PositionZoneConfigInfo>();

        public PaintingAreaWindow()
        {
            InitializeComponent();
            PositionBtnClickCommand = new RelayCommand(PositionBtnHandler);
            LeftBtnClickCommand = new RelayCommand(LeftBtnClickHandler);
            RightBtnClickCommand = new RelayCommand(RightBtnHandler);
            StartPaintCommand = new RelayCommand(StartPaintHandler);
            RepaintCommand = new RelayCommand(RepaintHandler);
            SaveCommand = new RelayCommand(SaveHandler);
            this.Loaded += PaintingAreaWindow_Loaded;
        }

        private async void SaveHandler(object obj)
        {
            List<ConfigPositionInvasionZoneInfo> param = new List<ConfigPositionInvasionZoneInfo>();
            foreach (var positionsKeyVal in positionConfigs)
            {
                List<ServerPoint> points = new List<ServerPoint>();
                if (positionsKeyVal.UIPoints.Count <= 0)
                {
                    if (positionsKeyVal.ServerConfigPoints.Count > 0)
                    {
                        foreach (var item in positionsKeyVal.ServerConfigPoints)
                            points.Add(new ServerPoint() { X = item.X,Y = item.Y});
                    }
                }
                else
                {
                    // 将UI界面比例换算成按照图片原始大小的真实比例。
                    foreach (var item in positionsKeyVal.UIPoints)
                    {
                        var temp = new ServerPoint() { X = positionsKeyVal.WidthScale * item.X, Y = positionsKeyVal.WidthScale * item.Y };
                        points.Add(temp);
                    }
                }

                var savepoints = points.Count > 4 ? points.Take(4).ToList() : points; // 最多存储四个点位
                param.Add(new ConfigPositionInvasionZoneInfo() { ID = positionsKeyVal.PositionID + 1, Points = savepoints });
            }

            var url = new Uri(GlobalConfig.HostServer + "/SetMonitorArea");
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            var response = await httpClient.PostAsync(url, new JsonContent(JsonConvert.SerializeObject(param)));
            if (response.IsSuccessStatusCode)
            {
                this.Close();
                MessageBox.Show("保存成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("保存失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void RepaintHandler(object obj)
        {
            canv.Children.Clear();
            var item = positionConfigs.FirstOrDefault(c => c.PositionID == CurrentOrderIndex);
            if (item == null)
                return;

            item.UIPoints.Clear();
        }

        private void StartPaintHandler(object obj) => IsPainting = !IsPainting;

        private void RightBtnHandler(object obj)
        {
            if (CurrentOrderIndex + 1 > 5)
                CurrentOrderIndex = 0;
            else
                CurrentOrderIndex += 1;
        }

        private void LeftBtnClickHandler(object obj)
        {
            if (CurrentOrderIndex - 1 < 0)
                CurrentOrderIndex = 5;
            else
                CurrentOrderIndex -= 1;
        }

        private void PositionBtnHandler(object obj)
        {
            if (obj == null)
                return;

            if (!(obj is int index))
                return;

            CurrentOrderIndex = index;
        }

        private async void PaintingAreaWindow_Loaded(object sender, RoutedEventArgs e)
        {
            positionConfigs.Clear();
            // 加载已有配置。
            var url = new Uri(GlobalConfig.HostServer + "/GetPositionConfig");
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            var response = await httpClient.PostAsync(url, new JsonContent("{}"));
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                if (data == null)
                    return;

                var configinfo = JsonConvert.DeserializeObject<List<PositionInvasionConfigInfo>>(data);
                if (configinfo == null || configinfo.Count <= 0)
                    return;

                foreach (var item in configinfo)
                {
                    if (item.Points == null || item.Points.Count <= 0)
                        continue;

                    PointCollection points = new PointCollection();
                    foreach (var point in item.Points)
                        points.Add(new Point(point.X, point.Y));

                    positionConfigs.Add(new PositionZoneConfigInfo()
                    {
                        PositionID = item.ID - 1,
                        UIPoints = new PointCollection(),
                        ServerConfigPoints = points
                    });
                }
            }

            CurrentOrderIndex = 0; // 设置初始位置为
        }

        private void canv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsPainting)
                return;

            Point point = Mouse.GetPosition(canv);
            polyline.Points.Add(point);
            _paintingClickCount += 1;

            // 已绘制完4个点时，自动补充完成区域封闭，且退出编辑模式
            if (_paintingClickCount == 4)
            {
                IsPainting = false;
                // 清除多余点位
                while (polyline.Points.Count > 4)
                    polyline.Points.RemoveAt(polyline.Points.Count - 1); // 移除最后一个点位置

                var firstpoint = polyline.Points.FirstOrDefault();
                if (firstpoint != null)
                    polyline.Points.Add(new Point(firstpoint.X, firstpoint.Y));

                var temp = positionConfigs.FirstOrDefault(c => c.PositionID == CurrentOrderIndex);
             
                temp.UIPoints.Clear();
                foreach (var item in polyline.Points)
                    temp.UIPoints.Add(new Point(item.X, item.Y));
            }
        }

        private void canv_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsPainting)
                return;

            while (polyline.Points.Count > _paintingClickCount)
                polyline.Points.RemoveAt(polyline.Points.Count - 1); // 移除最后一个点位置

            Point point = Mouse.GetPosition(canv);
            polyline.Points.Add(point);
        }
    }

    public class PaintingBtnCheckedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2)
                return false;
            var btnIndex = values[0];
            var currentIndex = values[1];
            if (btnIndex == null || currentIndex == null)
                return false;

            if (btnIndex is int b && currentIndex is int c)
                return b == c;

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
