using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace AIVisualwfpnew.Adorners
{
    public class PlaceholderAdorner : Adorner
    {

        /// <summary>
        /// 占位符字符串
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(PlaceholderAdorner), new PropertyMetadata(null));

        private TextBlock _textBlock;
        public PlaceholderAdorner(UIElement adornedElement) : base(adornedElement)
        {
            _textBlock = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Background = new SolidColorBrush(Colors.Transparent),
                Foreground = new SolidColorBrush(Colors.Gray),
                Margin = new Thickness(5, 0, 5, 0),
            };

            Binding txtbinding = new Binding(PlaceholderAdorner.TextProperty.Name);
            txtbinding.Source = this;
            _textBlock.SetBinding(TextBlock.TextProperty, txtbinding);

            AddLogicalChild(_textBlock);
            AddVisualChild(_textBlock);
        }

        protected override Visual GetVisualChild(int index)
        {
            return _textBlock;
        }

        protected override int VisualChildrenCount => 1;

        /// <summary>
        /// 应用子元素大小。
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            _textBlock.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return base.ArrangeOverride(finalSize);
        }

        /// <summary>
        /// 测量子元素大小
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            _textBlock.Measure(constraint);
            return base.MeasureOverride(constraint);
        }
    }
}
