using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMCWbClnt_dz_downloadmaster_async_await0.Infrastructure
{
    class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly Action<object> _action;
        public RelayCommand(Action<object> action) => _action = action;
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => _action(parameter);
    }
}
