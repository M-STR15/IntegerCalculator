using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace IntegerCalculator.ViewModels
{
	[ObservableObject]
	public partial class HandCalculatorViewModel
	{
		private static readonly char[] AllowedChars = "0123456789+-*/".ToCharArray();

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
				var isAllowed = AllowedChars.Contains(character.First());
				if (isAllowed)
				{
					Formula += character;
				}
			}
		}
	}
}
