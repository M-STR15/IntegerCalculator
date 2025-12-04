using CommunityToolkit.Mvvm.ComponentModel;
using IntegerCalculator.BE.ExpressionEvaluator;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace IntegerCalculator.ViewModels
{
	public partial class HandCalculatorViewModel:ObservableObject
	{
		private static readonly char[] AllowedChars = "0123456789+-*/".ToCharArray();

		[ObservableProperty]
		public string _valueDisplay;

		public ICommand InsertCharacterCommand { get; private set; }
		public ICommand EqualsCommand { get; private set; }
		private ICalculatService _calculatService;
		public ObservableCollection<string> CalculationSteps { get; private set; } = new();

		public HandCalculatorViewModel(ICalculatService calculatService)
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
					ValueDisplay += character;
				}
			}
		}

		private void onEqualsCommand_Execute(object parameter)
		{
			var expressionResult = _calculatService.EvaluateExpression(ValueDisplay, true);
			CalculationSteps.Clear();
			if (expressionResult != null)
			{
				foreach (var item in expressionResult.CalculationSteps)
				{
					CalculationSteps.Add(item);
				}

				ValueDisplay = expressionResult.Result;
			}
		}
	}
}
