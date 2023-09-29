using AIVisualwfpnew.myClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace AIVisualwfpnew.Controlers
{
    [TemplatePart(Name = "PART_ROOT", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_SELECTED", Type = typeof(Border))]
    [TemplatePart(Name = "PART_START", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_END", Type = typeof(Thumb))]
    public class RangeSlider : Control
    {
        private Grid _root;

        private Border _borderSelected;

        private Thumb _startThumb;

        private Thumb _endThumb;


        /// <summary>
        /// 最小值
        /// </summary>
        public double Minmum
        {
            get { return (double)GetValue(MinmumProperty); }
            set { SetValue(MinmumProperty, value); }
        }

        public static readonly DependencyProperty MinmumProperty =
            DependencyProperty.Register("Minmum", typeof(double), typeof(RangeSlider), new PropertyMetadata(0.0d));


        /// <summary>
        /// 代表的最大值
        /// </summary>
        public double MaxMum
        {
            get { return (double)GetValue(MaxMumProperty); }
            set { SetValue(MaxMumProperty, value); }
        }

        public static readonly DependencyProperty MaxMumProperty =
            DependencyProperty.Register("MaxMum", typeof(double), typeof(RangeSlider), new PropertyMetadata(24.0d));


        /// <summary>
        /// 当前起始值
        /// </summary>
        public double StartNum
        {
            get { return (double)GetValue(StartNumProperty); }
            set { SetValue(StartNumProperty, value); }
        }

        public static readonly DependencyProperty StartNumProperty =
            DependencyProperty.Register("StartNum", typeof(double), typeof(RangeSlider), new PropertyMetadata(0.0d, new PropertyChangedCallback(StartNumChangeHandler)));

        private static void StartNumChangeHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rs = d as RangeSlider;
            if (rs == null)
                return;

            if (rs._stepWidth == 0 || rs._totalTimes == 0)
                return;

            if (!(e.NewValue is double nv))
                return;

            if (nv > rs.MaxMum || nv < rs.Minmum)
                return;

            if (nv % rs.TickFrequency != 0)
                return;

            var firstColumnDefine = rs._root.ColumnDefinitions.FirstOrDefault();
            if (firstColumnDefine == null)
                return;

            // 计算偏移量
            var offset = nv * (rs._root.ActualWidth / (rs.MaxMum - rs.Minmum));
            var realyOffset = offset - (rs._root.ColumnDefinitions[1].ActualWidth / 2); // 真实移动距离要减去thumb控件的一半，才是最终距离
            if (realyOffset < 0)
                realyOffset = 0;

            firstColumnDefine.Width = new GridLength(realyOffset);
        }


        /// <summary>
        /// 当前结束值
        /// </summary>
        public double EndNum
        {
            get { return (double)GetValue(EndNumProperty); }
            set { SetValue(EndNumProperty, value); }
        }

        public static readonly DependencyProperty EndNumProperty =
            DependencyProperty.Register("EndNum", typeof(double), typeof(RangeSlider), new PropertyMetadata(24.0d, new PropertyChangedCallback(EndNumChangeHandler)));

        private static void EndNumChangeHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rs = d as RangeSlider;
            if (rs == null)
                return;

            if (rs._stepWidth == 0 || rs._totalTimes == 0)
                return;

            if (!(e.NewValue is double nv))
                return;

            if (nv > rs.MaxMum || nv < rs.Minmum)
                return;

            if (nv % rs.TickFrequency != 0)
                return;

            var lastColumnDefine = rs._root.ColumnDefinitions.LastOrDefault();
            if (lastColumnDefine == null)
                return;

            // 计算偏移量
            var offset = (rs.MaxMum - nv) * (rs._root.ActualWidth / (rs.MaxMum - rs.Minmum));
            var realyOffset = offset - (rs._root.ColumnDefinitions[3].ActualWidth / 2); // 真实移动距离要减去thumb控件的一半，才是最终距离
            if (realyOffset < 0)
                realyOffset = 0;
            else if (realyOffset > rs._root.ActualWidth)
                realyOffset = rs._root.ActualWidth;

            System.Diagnostics.Trace.WriteLine($"EndBorder colum is:{realyOffset}");
            lastColumnDefine.Width = new GridLength(realyOffset);
        }


        /// <summary>
        /// 步进长度
        /// </summary>
        public double TickFrequency
        {
            get { return (double)GetValue(TickFrequencyProperty); }
            set { SetValue(TickFrequencyProperty, value); }
        }

        public static readonly DependencyProperty TickFrequencyProperty;


        static RangeSlider()
        {
            OverridesDefaultStyleProperty.OverrideMetadata(typeof(RangeSlider), new FrameworkPropertyMetadata(true));
            TickFrequencyProperty = DependencyProperty.Register("TickFrequency", typeof(double), typeof(RangeSlider), new FrameworkPropertyMetadata(2.0d), IsValidDoubleValue);
        }

        public RangeSlider()
        {
            Loaded += RangeSlider_Loaded;
        }

        /// <summary>
        /// 单步距离。
        /// </summary>
        private double _stepWidth = 0;

        /// <summary>
        /// 表示minMum到maxMum，步进值是：TickFrequency的情况下，一共能移动多少次
        /// </summary>
        private double _totalTimes = 0;

        private void SetStepLength()
        {
            _root.Width = _root.ActualWidth;
            _root.MaxWidth = _root.ActualWidth;
            _stepWidth = (_root.ActualWidth * TickFrequency) / (MaxMum - Minmum);
            _totalTimes = (MaxMum - Minmum) / TickFrequency;
        }

        private void RangeSlider_Loaded(object sender, RoutedEventArgs e)
        {
            SetStepLength();
            if (_startThumb != null)
            {
                _startThumb.DragDelta -= _startThumb_DragDelta;
                _startThumb.DragDelta += _startThumb_DragDelta;


                _endThumb.DragDelta -= _endThumb_DragDelta;
                _endThumb.DragDelta += _endThumb_DragDelta;
            }
        }

        private void _endThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_root == null)
                return;

            var lastColumnDefine = _root.ColumnDefinitions.LastOrDefault();
            if (lastColumnDefine == null)
                return;

            double times = 0;
            if (e.HorizontalChange > 0)
            {
                // 往右拉动
                var newVal = lastColumnDefine.Width.Value - e.HorizontalChange;
                if (newVal < 0)
                    return;

                times = Math.Floor(newVal / _stepWidth); // 移动次数
                if (times > _totalTimes)
                    times = _totalTimes;
                else if (times < 0)
                    times = 0;
            }
            else
            {
                // 往左拉动
                var newVal = lastColumnDefine.Width.Value - e.HorizontalChange;

                times = Math.Floor(newVal / _stepWidth); // 移动次数
                if (times > _totalTimes)
                    times = _totalTimes;
                else if (times < 0)
                    times = 0;
            }
            var temp = MaxMum - (times * TickFrequency);
            if (temp < StartNum)
                EndNum = StartNum;
            else
                EndNum = temp;

            System.Diagnostics.Trace.WriteLine($"EndNum is:{EndNum}");
        }

        /// <summary>
        /// 检查左右两侧变化是否会超过总大小
        /// </summary>
        /// <returns></returns>
        private bool CheckLeftAndRightOverflow(double left, double right) => left + right + (_root.ColumnDefinitions[3].ActualWidth / 2) + (_root.ColumnDefinitions[1].ActualWidth / 2) > _root.ActualWidth;


        private void _startThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_root == null)
                return;

            var firstColumnDefine = _root.ColumnDefinitions.FirstOrDefault();
            if (firstColumnDefine == null)
                return;

            double times = 0;
            if (e.HorizontalChange > 0)
            {
                // 往右拉动
                var newVal = firstColumnDefine.ActualWidth + e.HorizontalChange;

                times = Math.Floor(newVal / _stepWidth); // 移动次数
                if (times > _totalTimes)
                    times = _totalTimes;
                else if (times < 0)
                    times = 0;
            }
            else
            {
                // 往左拉动
                var newVal = firstColumnDefine.ActualWidth + e.HorizontalChange;

                times = Math.Floor(newVal / _stepWidth); // 移动次数
                if (times > _totalTimes)
                    times = _totalTimes;
                else if (times <= 0)
                    times = 0;
            }

            var temp = times * TickFrequency;
            if (temp > EndNum)
                temp = EndNum;

            StartNum = temp;
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _root = GetTemplateChild("PART_ROOT") as Grid;
            _startThumb = GetTemplateChild("PART_START") as Thumb;
            _borderSelected = GetTemplateChild("PART_SELECTED") as Border;
            _endThumb = GetTemplateChild("PART_END") as Thumb;
        }

        private static bool IsValidDoubleValue(object value)
        {
            double num = (double)value;
            if (!double.IsNaN(num))
                return !double.IsInfinity(num);

            return false;
        }
    }
}
