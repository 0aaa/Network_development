using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfTcpLstnrClient0.Infrastructure
{
    class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public bool CanExecuteFlg { get; set; }
        private readonly Action<object> _action;
        public RelayCommand(Action<object> action, bool canExecuteFlg = true)
        {
            _action = action;
            CanExecuteFlg = canExecuteFlg;
        }
        public bool CanExecute(object parameter) => CanExecuteFlg;

        public void Execute(object parameter) => _action(parameter);
    }
}
