using CommunityToolkit.Mvvm.ComponentModel;
using IntegerCalculator.BE.ExpressionEvaluator;
using System.Windows.Input;

namespace IntegerCalculator.ViewModels
{
	[ObservableObject]
	public partial class HandCalculatorViewModel
	{
		private static readonly char[] AllowedChars = "0123456789+-*/".ToCharArray();

		[ObservableProperty]
		public string _formula;

		public ICommand InsertCharacterCommand { get; private set; }
		public ICommand EqualsCommand { get; private set; }
		private CalculatService _calculatService;

		public HandCalculatorViewModel(CalculatService calculatService)
		{
			_calculatService = calculatService;

			InsertCharacterCommand = new Helpers.RelayCommand(onInsertCharacter_Execute);
			EqualsCommand = new Helpers.RelayCommand(onEqualsCommand_Execute);
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

		private void onEqualsCommand_Execute(object parameter)
		{
			_calculatService.EvaluateExpression(" 3 * 3 ");
			_calculatService.EvaluateExpression(" 3 * 3 + 4");

			_calculatService.EvaluateExpression(" 33 * 3 ");
		}
	}
}
