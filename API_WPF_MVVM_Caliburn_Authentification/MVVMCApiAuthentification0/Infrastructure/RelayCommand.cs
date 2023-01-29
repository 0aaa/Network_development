using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMCApiAuthentification0.Infrastructure
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecuteFlag { get; set; }
        private readonly Action<object> _action;
        public RelayCommand(Action<object> action)
        {
            _action = action;
            CanExecuteFlag = true;
        }
        public bool CanExecute(object parameter) => CanExecuteFlag;

        public void Execute(object parameter) => _action(parameter);
    }
}
