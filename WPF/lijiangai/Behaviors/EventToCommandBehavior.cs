using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace AIVisualwfpnew.Behaviors
{
    /*
     * Author: Shixing
     * 
用于WPF中使用Behavior绑定事件的行为。可以不依赖第三方就能将任何控件事件绑定到ViewModel中去，且可以支持自定义参数、sender、e参数的传递
用法：
xaml中引用：
xmlns:behaiver="http://schemas.microsoft.com/expression/2010/interactivity"
 
<behaiver:Interaction.Behaviors>
    <eventbehavior:EventToCommandBehavior PassArguments="True" PassSender="True" CommandParameter="{Binding}" Event="MouseLeftButtonDown" Command="{Binding ImageMouseLeftButtonDown}"></eventbehavior:EventToCommandBehavior>
</behaiver:Interaction.Behaviors>
 
 
后续可以定义一个实现了ICommand的RelayCommand，使得与此行为配合会更好用。
*/



    /// <summary>
    /// Behavior that will connect an UI event to a viewmodel Command,
    /// allowing the event arguments to be passed as the CommandParameter.
    /// </summary>
    public class EventToCommandBehavior : Behavior<FrameworkElement>
    {
        private Delegate _handler;
        private EventInfo _oldEvent;

        // Event
        public string Event
        { get { return (string)GetValue(EventProperty); } set { SetValue(EventProperty, value); } }

        public static readonly DependencyProperty EventProperty = DependencyProperty.Register("Event", typeof(string), typeof(EventToCommandBehavior), new PropertyMetadata(null, OnEventChanged));

        // Command
        public ICommand Command
        { get { return (ICommand)GetValue(CommandProperty); } set { SetValue(CommandProperty, value); } }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommandBehavior), new PropertyMetadata(null));

        // PassArguments (default: false)
        public bool PassArguments
        { get { return (bool)GetValue(PassArgumentsProperty); } set { SetValue(PassArgumentsProperty, value); } }

        public static readonly DependencyProperty PassArgumentsProperty = DependencyProperty.Register("PassArguments", typeof(bool), typeof(EventToCommandBehavior), new PropertyMetadata(false));

        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var beh = (EventToCommandBehavior)d;

            if (beh.AssociatedObject != null) // is not yet attached at initial load
                beh.AttachHandler((string)e.NewValue);
        }

        /// <summary>
        /// 是否传递Sender
        /// </summary>
        public bool PassSender
        {
            get { return (bool)GetValue(PassSenderProperty); }
            set { SetValue(PassSenderProperty, value); }
        }

        public static readonly DependencyProperty PassSenderProperty =
            DependencyProperty.Register("PassSender", typeof(bool), typeof(EventToCommandBehavior), new PropertyMetadata(false));

        /// <summary>
        /// 自定义参数，object类型
        /// </summary>
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(EventToCommandBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            AttachHandler(this.Event); // initial set
        }

        /// <summary>
        /// Attaches the handler to the event
        /// </summary>
        private void AttachHandler(string eventName)
        {
            // detach old event
            if (_oldEvent != null)
                _oldEvent.RemoveEventHandler(this.AssociatedObject, _handler);

            // attach new event
            if (!string.IsNullOrEmpty(eventName))
            {
                EventInfo ei = this.AssociatedObject.GetType().GetEvent(eventName);
                if (ei != null)
                {
                    MethodInfo mi = this.GetType().GetMethod("ExecuteCommand", BindingFlags.Instance | BindingFlags.NonPublic);
                    _handler = Delegate.CreateDelegate(ei.EventHandlerType, this, mi);
                    ei.AddEventHandler(this.AssociatedObject, _handler);
                    _oldEvent = ei; // store to detach in case the Event property changes
                }
                else
                    throw new ArgumentException(string.Format("The event '{0}' was not found on type '{1}'", eventName, this.AssociatedObject.GetType().Name));
            }
        }

        /// <summary>
        /// Executes the Command
        /// </summary>
        private void ExecuteCommand(object sender, EventArgs e)
        {
            System.Collections.Generic.List<object> parameter = new System.Collections.Generic.List<object>();
            if (this.PassSender)
                parameter.Add(sender);
            if (this.PassArguments)
                parameter.Add(e);
            if (CommandParameter != null)
                parameter.Add(CommandParameter);

            var executeargs = parameter.ToArray();

            if (this.Command != null)
            {
                if (this.Command.CanExecute(executeargs))
                {
                    this.Command.Execute(executeargs);
                }
            }
        }
    }
}
