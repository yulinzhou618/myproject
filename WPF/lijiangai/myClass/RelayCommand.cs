using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AIVisualwfpnew.myClass
{
    class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly Action<object> m_execute;
        private readonly Predicate<object> m_canExecute;
        public RelayCommand(Action<object> execute)
        {
            this.m_execute = execute;
        }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.m_execute = execute;
            this.m_canExecute = canExecute;
        }


        public bool CanExecute(object parameter)
        {
            if (m_canExecute == null)
                return true;

            return m_canExecute(parameter);

        }
        //public event EventHandler CanExecuteChanged
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}
        public void Execute(object parameter)
        {
            this.m_execute(parameter);
        }
    }
}
