using IntegerCalculator.ViewModels;

namespace IntegerCalculator
{
	public class MainViewModel
	{
		public HandCalculatorViewModel HandCalculatorViewModel { get; private set; }
		public FormulaCalculatorViewModel FormulaCalculatorViewModel { get; private set; }
		public MainViewModel(HandCalculatorViewModel handCalculatorViewModel, FormulaCalculatorViewModel formulaCalculatorViewModel)
		{
			HandCalculatorViewModel = handCalculatorViewModel;
			FormulaCalculatorViewModel = formulaCalculatorViewModel;
		}
	}
}
