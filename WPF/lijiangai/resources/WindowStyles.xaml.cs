using System.Windows;
using System.Windows.Input;

namespace AIVisualwfpnew.resources
{
    public partial class WindowStyles
    {
        private Window _window;
        public WindowStyles()
        {
        }

        private void WindowBorder_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(sender is DependencyObject dobj))
                return;

            _window = Window.GetWindow(dobj);
            if (_window == null)
                return;

            _window.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            _window.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeWindow, CanResizeWindow));
            _window.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));
            _window.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreWindow, CanResizeWindow));
            _window.CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, ShowSystemMenu));
        }

        private void ShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
        {
            if (_window == null)
                return;

            var element = e.OriginalSource as FrameworkElement;
            if (element == null)
                return;

            var point = _window.WindowState == WindowState.Maximized ? new Point(0, element.ActualHeight)
                : new Point(_window.Left + _window.BorderThickness.Left, element.ActualHeight + _window.Top + _window.BorderThickness.Top);
            point = element.TransformToAncestor(_window).Transform(point);
            SystemCommands.ShowSystemMenu(_window, point);
        }

        private void RestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            if (_window != null)
                SystemCommands.RestoreWindow(_window);
        }

        private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is Window win && win != null)
                e.CanExecute = win.ResizeMode != ResizeMode.NoResize;
        }

        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            if (_window != null)
                SystemCommands.MinimizeWindow(_window);
        }

        private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_window != null)
                e.CanExecute = _window.ResizeMode == ResizeMode.CanResize || _window.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void MaximizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Window win && win != null)
                SystemCommands.MaximizeWindow(win); ;
        }

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Window win && win != null)
                win.Close();
        }
    }
}
