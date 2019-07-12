using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Memory.ViewModel
{
    class MyCommand : ICommand
    {
        public Predicate<object> CanExecuteFunc
        {
            get;
            set;
        }

        public Action<object> ExecuteFunc
        {
            get;
            set;
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            ExecuteFunc(parameter);
        }
    }
}
