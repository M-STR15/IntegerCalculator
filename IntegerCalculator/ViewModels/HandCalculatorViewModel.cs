using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace IntegerCalculator.ViewModels
{
	[ObservableObject]
	public partial class HandCalculatorViewModel
	{
		[ObservableProperty]
		public string _formula;

		public ICommand InsertCharacter { get; private set; }

		public HandCalculatorViewModel()
		{
			InsertCharacter = new Helpers.RelayCommand(onInsertCharacter_Execute);
		}

		private void onInsertCharacter_Execute(object parameter)
		{
			if (parameter is string character)
			{
				Formula += character;
			}
		}
	}
}
