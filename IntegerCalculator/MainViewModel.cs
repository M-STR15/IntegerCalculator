using CommunityToolkit.Mvvm.ComponentModel;
using IntegerCalculator.BE.Shared.Helpers;
using IntegerCalculator.ViewModels;

namespace IntegerCalculator
{
	public partial class MainViewModel : ObservableObject
	{
		[ObservableProperty]
		private string _versionSW;
		public HandCalculatorViewModel HandCalculatorViewModel { get; private set; }
		public FormulaCalculatorViewModel FormulaCalculatorViewModel { get; private set; }
		public MainViewModel(HandCalculatorViewModel handCalculatorViewModel, FormulaCalculatorViewModel formulaCalculatorViewModel, VersionSWHelper versionSWHelper)
		{
			HandCalculatorViewModel = handCalculatorViewModel;
			FormulaCalculatorViewModel = formulaCalculatorViewModel;

			_versionSW = VersionSWHelper.GetVersionSW();
		}
	}
}
