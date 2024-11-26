using System.Windows.Input;

namespace DbLab.WpfApp.Base
{
    public class UiCommand : ICommand
    {
        private readonly Action<object?> _command;
        private readonly Func<bool>? _canExecute;

        public UiCommand(Action<object?> command, Func<bool>? canExecute = null)
        {
            _command = command;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public void Execute(object? parameter)
        {
            _command.Invoke(parameter);
        }

        public event EventHandler? CanExecuteChanged;
    }
}
