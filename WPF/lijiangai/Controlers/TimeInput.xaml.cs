using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AIVisualwfpnew.Controlers
{
    [TemplatePart(Name = "PART_HOUR", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_MINIT", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_SECOND", Type = typeof(TextBox))]
    public class TimeInput : Control
    {
        private TextBox _hourTextBox;
        private TextBox _minitTextBox;
        private TextBox _secondTextBox;
        static TimeInput()
        {
            OverridesDefaultStyleProperty.OverrideMetadata(typeof(TimeInput), new FrameworkPropertyMetadata(true));
        }

        public TimeInput()
        {
            this.Loaded += TimeInput_Loaded;
        }

        private void TimeInput_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _hourTextBox = GetTemplateChild("PART_HOUR") as TextBox;
            _minitTextBox = GetTemplateChild("PART_MINIT") as TextBox;
            _secondTextBox = GetTemplateChild("PART_SECOND") as TextBox;
        }
    }
}
