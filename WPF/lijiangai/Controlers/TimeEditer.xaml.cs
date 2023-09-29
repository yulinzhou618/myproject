using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AIVisualwfpnew.Controlers
{
    [TemplatePart(Name = "PART_YEAR", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_MONTH", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_DAY", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_HOUR", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_MINUTE", Type = typeof(TextBox))]
    public class TimeEditer : Control
    {
        private TextBox _yearTextBox;
        private TextBox _monthTextBox;
        private TextBox _dayTextBox;
        private TextBox _hourTextBox;
        private TextBox _minuteTextBox;

        /// <summary>
        /// 最终时间。
        /// </summary>
        public DateTime DateTime
        {
            get { return (DateTime)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); }
        }

        public static readonly DependencyProperty DateTimeProperty =
            DependencyProperty.Register("DateTime", typeof(DateTime), typeof(TimeEditer), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DateTimeChangeHandler)));

        private static void DateTimeChangeHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as TimeEditer;
            if (self == null)
                return;

            if (e.NewValue == null || !(e.NewValue is DateTime nv))
                return;

            if (self._yearTextBox != null)
                self._yearTextBox.Text = nv.Year.ToString();

            if (self._monthTextBox != null)
                self._monthTextBox.Text = nv.Month.ToString();

            if (self._dayTextBox != null)
                self._dayTextBox.Text = nv.Day.ToString();

            if (self._hourTextBox != null)
                self._hourTextBox.Text = nv.Hour.ToString();

            if (self._minuteTextBox != null)
                self._minuteTextBox.Text = nv.Minute.ToString();
        }

        static TimeEditer()
        {
            OverridesDefaultStyleProperty.OverrideMetadata(typeof(TimeEditer), new FrameworkPropertyMetadata(true));
        }

        public override void OnApplyTemplate()
        {
            if (this.DateTime == default(DateTime))
                this.DateTime = DateTime.Now;
            base.OnApplyTemplate();
            _yearTextBox = GetTemplateChild("PART_YEAR") as TextBox;
            _monthTextBox = GetTemplateChild("PART_MONTH") as TextBox;
            _dayTextBox = GetTemplateChild("PART_DAY") as TextBox;
            _hourTextBox = GetTemplateChild("PART_HOUR") as TextBox;
            _minuteTextBox = GetTemplateChild("PART_MINUTE") as TextBox;

            if (_yearTextBox != null)
            {
                _yearTextBox.Text = this.DateTime.Year.ToString();
                _yearTextBox.LostFocus -= _yearTextBox_LostFocus;
                _yearTextBox.LostFocus += _yearTextBox_LostFocus;
            }


            if (_monthTextBox != null)
            {
                _monthTextBox.Text = this.DateTime.Month.ToString();
                _monthTextBox.LostFocus -= _yearTextBox_LostFocus;
                _monthTextBox.LostFocus += _yearTextBox_LostFocus;
            }

            if (_dayTextBox != null)
            {
                _dayTextBox.Text = this.DateTime.Day.ToString();
                _dayTextBox.LostFocus -= _yearTextBox_LostFocus;
                _dayTextBox.LostFocus += _yearTextBox_LostFocus;
            }

            if (_hourTextBox != null)
            {
                _hourTextBox.Text = this.DateTime.Hour.ToString();
                _hourTextBox.LostFocus -= _yearTextBox_LostFocus;
                _hourTextBox.LostFocus += _yearTextBox_LostFocus;
            }

            if (_minuteTextBox != null)
            {
                _minuteTextBox.Text = this.DateTime.Minute.ToString();
                _minuteTextBox.LostFocus -= _yearTextBox_LostFocus;
                _minuteTextBox.LostFocus += _yearTextBox_LostFocus;
            }
        }

        private void _yearTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_yearTextBox == null || _monthTextBox == null || _dayTextBox == null || _hourTextBox == null || _minuteTextBox == null)
                return;

            if (!System.DateTime.TryParse($"{_yearTextBox.Text}-{_monthTextBox.Text}-{_dayTextBox.Text} {_hourTextBox.Text}:{_minuteTextBox.Text}:{this.DateTime.Second}", out System.DateTime newtime))
            {
                Reset();
                return;
            }

            if (newtime == this.DateTime)
                return;

            this.DateTime = newtime;
        }

        private void Reset()
        {
            _yearTextBox.Text = this.DateTime.Year.ToString();
            _monthTextBox.Text = this.DateTime.Month.ToString();
            _dayTextBox.Text = this.DateTime.Day.ToString();
            _hourTextBox.Text = this.DateTime.Hour.ToString();
            _minuteTextBox.Text = this.DateTime.Minute.ToString();
        }
    }
}
