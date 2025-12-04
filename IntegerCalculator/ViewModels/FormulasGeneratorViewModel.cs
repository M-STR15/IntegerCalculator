using CommunityToolkit.Mvvm.ComponentModel;
using IntegerCalculator.BE.EventLog.Services;
using IntegerCalculator.BE.ExpressionEvaluator;
using System.IO;
using System.Windows.Input;

namespace IntegerCalculator.ViewModels
{
	[ObservableObject]
	public partial class FormulasGeneratorViewModel
	{
		[ObservableProperty]
		private double _numberOfFormula = 100000000;

		[ObservableProperty]
		private double _progressGeneration;

		[ObservableProperty]
		private string _selectInputFile;

		private bool isMethodGenerateFileRun = false;
		public ICommand GenerateInputFileCommand { get; }
		private IEventLogService _eventLogService;
		public FormulasGeneratorViewModel(string selectInputFile, IEventLogService eventLogService)
		{
			_eventLogService = eventLogService;
			GenerateInputFileCommand = new Helpers.RelayCommand(onGenerateInputFile, canGenerateFile);
			SelectInputFile = selectInputFile;
		}

		private bool canGenerateFile() => !isMethodGenerateFileRun;
		private async void onGenerateInputFile(object parameter)
		{
			isMethodGenerateFileRun = true;

			try
			{
				var formuLaGenerator = new FormulaGenerator();

				if (!File.Exists(SelectInputFile))
					File.Create(SelectInputFile);

				using (var writer = new StreamWriter(SelectInputFile, append: false))
				{
					double stepProgress = 0;
					for (int i = 0; i < NumberOfFormula; i++)
					{
						var formula = await formuLaGenerator.GenerateFormulaAsync();

						if (!string.IsNullOrEmpty(formula))
							await writer.WriteLineAsync(formula);

						stepProgress++;
						var percent = (stepProgress * 100) / NumberOfFormula;
						ProgressGeneration = Math.Round(percent, 4);
					}
				}
			}
			catch (Exception ex)
			{
				_eventLogService.LogError(Guid.Parse("3aa90f29-fd8a-4e20-9291-eaf8a01cc8b9"), ex, null);
			}
			finally
			{
				isMethodGenerateFileRun = false;
			}
		}
	}
}
