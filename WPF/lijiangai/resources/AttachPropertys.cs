using AIVisualwfpnew.Adorners;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace AIVisualwfpnew.resources
{
    public class AttachPropertys : DependencyObject
    {
        public static string GetPlaceholder(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceholderProperty);
        }

        public static void SetPlaceholder(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceholderProperty, value);
        }

        /// <summary>
        /// 占位符
        /// </summary>
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(AttachPropertys), new PropertyMetadata(null, new PropertyChangedCallback(PlaceholderChangedHandler)));

        private static void PlaceholderChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox tb)
            {
                tb.TextChanged -= Tb_TextChanged;
                tb.TextChanged += Tb_TextChanged;
                Tb_TextChanged(tb, null);
            }
        }

        private static void Tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb)
                HandPlaceHolder(tb);
        }

        private static void HandPlaceHolder(TextBox tb)
        {
            if (tb.Text == null || string.IsNullOrEmpty(tb.Text))
            {
                // 显示占位符
                AddPlaceHolderAdorner(tb);
            }
            else
            {
                // 去掉占位符
                RemoveAllPlaceHolderAdorner(tb);
            }
        }

        public static void AddPlaceHolderAdorner(TextBox tb)
        {
            var layer = AdornerLayer.GetAdornerLayer(tb);
            if (layer == null)
                return;

            if (!HasPlaceHolderAdorner(layer, tb))
            {
                var ph = new PlaceholderAdorner(tb);
                Binding bd = new Binding();
                bd.Source = tb;
                bd.Path = new PropertyPath(AttachPropertys.PlaceholderProperty);
                bd.Mode = BindingMode.TwoWay;
                ph.SetBinding(PlaceholderAdorner.TextProperty, bd);
                layer.Add(ph);
            }
        }

        /// <summary>
        /// 移除文本框上所有占位符层。
        /// </summary>
        /// <param name="tb"></param>
        private static void RemoveAllPlaceHolderAdorner(TextBox tb)
        {
            var layer = AdornerLayer.GetAdornerLayer(tb);
            if (layer == null)
                return;

            var adorners = layer.GetAdorners(tb);
            if (adorners == null)
                return;

            var placholderAdoners = adorners.Where(c => c is PlaceholderAdorner);
            if (placholderAdoners == null)
                return;

            foreach (var ad in placholderAdoners)
                layer.Remove(ad);
        }

        private static bool HasPlaceHolderAdorner(AdornerLayer layer, UIElement ele)
        {
            if (layer == null)
                return false;

            var adorners = layer.GetAdorners(ele);
            if (adorners == null)
                return false;

            var plachoderadorners = adorners.Where(c => c is PlaceholderAdorner);
            if (plachoderadorners == null)
                return false;

            return plachoderadorners.Any();
        }


        public static ImageSource GetImage(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(ImageProperty);
        }

        public static void SetImage(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(ImageProperty, value);
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.RegisterAttached("Image", typeof(ImageSource), typeof(AttachPropertys), new PropertyMetadata(null));



        public static ImageSource GetHoverImage(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(HoverImageProperty);
        }

        public static void SetHoverImage(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(HoverImageProperty, value);
        }

        /// <summary>
        /// 鼠标滑动上去时的图片
        /// </summary>
        public static readonly DependencyProperty HoverImageProperty =
            DependencyProperty.RegisterAttached("HoverImage", typeof(ImageSource), typeof(AttachPropertys), new PropertyMetadata(null));



        public static bool GetHasMiniBtn(DependencyObject obj)
        {
            return (bool)obj.GetValue(HasMiniBtnProperty);
        }

        public static void SetHasMiniBtn(DependencyObject obj, bool value)
        {
            obj.SetValue(HasMiniBtnProperty, value);
        }
        /// <summary>
        /// 指示窗体是否有最小化按钮
        /// </summary>
        public static readonly DependencyProperty HasMiniBtnProperty =
            DependencyProperty.RegisterAttached("HasMiniBtn", typeof(bool), typeof(AttachPropertys), new PropertyMetadata(true));


        public static bool GetHasResetBtn(DependencyObject obj)
        {
            return (bool)obj.GetValue(HasResetBtnProperty);
        }

        public static void SetHasResetBtn(DependencyObject obj, bool value)
        {
            obj.SetValue(HasResetBtnProperty, value);
        }

        /// <summary>
        /// 指示窗体是否有重置大小按钮
        /// </summary>
        public static readonly DependencyProperty HasResetBtnProperty =
            DependencyProperty.RegisterAttached("HasResetBtn", typeof(bool), typeof(AttachPropertys), new PropertyMetadata(true));


        public static bool GetIsChecked(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsCheckedProperty);
        }

        public static void SetIsChecked(DependencyObject obj, bool value)
        {
            obj.SetValue(IsCheckedProperty, value);
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.RegisterAttached("IsChecked", typeof(bool), typeof(AttachPropertys), new PropertyMetadata(false));
    }
}
