using System;
using System.Windows.Input;

namespace FF_WPF.Commands
{
    public class Command : ICommand
    {
        private readonly Action<object> _execute;

        public Command(Action<object> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public bool CanExecute(object parameter)
        {
            return true;
            //throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
