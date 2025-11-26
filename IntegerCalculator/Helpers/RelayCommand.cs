using System.ComponentModel;
using System.Windows.Input;

namespace IntegerCalculator.Helpers
{
	public class RelayCommand : ICommand
	{
		#region Fields

		private readonly Func<bool> _canExecute;
		private readonly Action<object> _execute;

		#endregion Fields

		#region Constructors

		public RelayCommand(Action<object> execute)
		: this(execute, null)
		{
		}

		public RelayCommand(Action<object> execute, Func<bool>? canExecute = null, INotifyPropertyChanged? propChangedSource = null)
		{
			if (execute == null)
				throw new ArgumentNullException("execute");

			_execute = execute;
			_canExecute = canExecute;
		}

		#endregion Constructors

		#region ICommand Members

		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		private event EventHandler _canExecuteChanged;

		public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

		public void Execute(object parameter) => _execute(parameter);

		protected virtual void OnCanExecuteChanged()
		{
			_canExecuteChanged?.Invoke(this, EventArgs.Empty);
		}

		private void PropChangedSource_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnCanExecuteChanged();
		}

		#endregion ICommand Members
	}
}
