using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AIVisualwfpnew
{
    /// <summary>
    /// DialogWindowBase.xaml 的交互逻辑
    /// </summary>
    public partial class ImageViewWindow : Window
    {
        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        /// <summary>
        /// 要展示的图片
        /// </summary>
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImageViewWindow), new PropertyMetadata(null));

        public ImageViewWindow()
        {
            InitializeComponent();
        }

        public ImageViewWindow(ImageSource source) : this()
        {
            this.Image = source;
        }

        public ImageViewWindow(string imagepath) : this()
        {
            if (!File.Exists(imagepath))
                return;

            BitmapImage bitmap = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(imagepath)))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze();
            }

            this.Image = bitmap;
        }

        public ImageViewWindow(Stream stream) : this()
        {
            BitmapImage bitmap = new BitmapImage();
            using (stream)
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze();
            }
            this.Image = bitmap;
        }

        public ImageViewWindow(Bitmap image) : this()
        {
            if (image == null)
                return;

            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
                this.Image = (ImageSource)imageSourceConverter.ConvertFrom(stream);
            }
        }

        /// <summary>
        /// 打印按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog().Value)
                printDialog.PrintVisual(ImageControl, "打印图片");
        }
    }
}
