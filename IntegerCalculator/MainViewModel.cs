using IntegerCalculator.ViewModels;

namespace IntegerCalculator
{
	public class MainViewModel
	{
		public HandCalculatorViewModel HandCalculatorViewModel { get; private set; }
		public MainViewModel(HandCalculatorViewModel handCalculatorViewModel)
		{
			HandCalculatorViewModel = handCalculatorViewModel;
		}
	}
}
