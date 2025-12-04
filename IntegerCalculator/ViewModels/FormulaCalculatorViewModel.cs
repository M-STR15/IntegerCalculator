using CommunityToolkit.Mvvm.ComponentModel;
using IntegerCalculator.BE.EventLog.Services;
using IntegerCalculator.BE.ExpressionEvaluator;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace IntegerCalculator.ViewModels
{
	public partial class FormulaCalculatorViewModel : ObservableObject
	{
		private IEventLogService _eventLogService;
		[ObservableProperty]
		private string _selectInputFile;

		[ObservableProperty]
		private string _selectOutputFile;

		[ObservableProperty]
		private int _progressExpression;

		private bool isMethodGenerateFileRun = false;
		private bool isMethodSelectInputFileRun = false;
		private bool isMethodSelectOutputFileRun = false;
		[ObservableProperty]
		private bool _isMethodStartRun;
		public ICommand GenerateInputFileCommand { get; private set; }
		public ICommand SelectInputFileCommand { get; private set; }
		public ICommand SelectOutputFileCommand { get; private set; }
		public ICommand StartCommand { get; private set; }
		private CalculatService _calculatService;
		public FormulaCalculatorViewModel(IEventLogService eventLogService, CalculatService calculatService)
		{
			_eventLogService = eventLogService;
			_calculatService = calculatService;

			SelectInputFileCommand = new Helpers.RelayCommand(onSelectFile, canSelectInputFile);
			GenerateInputFileCommand = new Helpers.RelayCommand(onGenerateInputFile, canGenerateFile);
			SelectOutputFileCommand = new Helpers.RelayCommand(onSelectOutputFile, canSelectOutputFile);
			StartCommand = new Helpers.RelayCommand(onStart, canStart);

			var actualFolder = Directory.GetCurrentDirectory();

			SelectInputFile = string.Format($"{actualFolder}\\_Input_Formulas.txt");
			SelectOutputFile = string.Format($"{actualFolder}\\_Output_FormulaResults.txt");
		}

		private bool canGenerateFile() => !isMethodGenerateFileRun;

		private bool canSelectInputFile() => !isMethodSelectInputFileRun;

		private bool canSelectOutputFile() => !isMethodSelectOutputFileRun;

		private bool canStart() => !IsMethodStartRun && !string.IsNullOrEmpty(SelectInputFile) && !string.IsNullOrEmpty(SelectOutputFile);

		private async void onGenerateInputFile(object parameter)
		{
			isMethodGenerateFileRun = true;

			var formuLaGenerator = new FormulaGenerator();
			var numberOfFormula = 100000000;
			if (!File.Exists(SelectInputFile))
				File.Create(SelectInputFile);

			using (var writer = new StreamWriter(SelectInputFile))
			{
				for (int i = 0; i < numberOfFormula; i++)
				{
					var formula = await formuLaGenerator.GenerateFormulaAsync();

					if (formula != null)
						await writer.WriteLineAsync(formula);
				}
			}
			 
			isMethodGenerateFileRun = false;
		}

		private async void onSelectFile(object parameter)
		{
			try
			{
				isMethodSelectInputFileRun = true;

				var dialog = new OpenFileDialog();
				dialog.Filter = "Textové soubory (*.txt)|*.txt|Všechny soubory (*.*)|*.*";
				dialog.Title = "Vyber TXT soubor";

				if (dialog.ShowDialog() == true)
				{
					string cesta = dialog.FileName;
					SelectInputFile = cesta;
				}
			}
			catch (Exception ex)
			{
				await _eventLogService.LogErrorAsync(Guid.Parse("d3a1874c-b312-4cc7-b518-3dc9cf88b031"), ex, null);
			}
			finally
			{
				isMethodSelectInputFileRun = false;
			}
		}
		private void onSelectOutputFile(object parameter)
		{
			isMethodSelectOutputFileRun = true;

			var dlg = new OpenFolderDialog
			{
				Title = "Selected folder"
			};

			if (dlg.ShowDialog() == true)
			{
				SelectOutputFile = dlg.FolderName;
			}
			isMethodSelectOutputFileRun = false;
		}

		private async void onStart(object parameter)
		{
			IsMethodStartRun = true;
			ProgressExpression = 0;

			var existInputFile = File.Exists(SelectInputFile);
			var listWithResults = new List<string>();
			if (existInputFile)
			{
				using (var reader = new StreamReader(SelectInputFile))
				using (var writer = new StreamWriter(SelectOutputFile))
				{
					string? line;
					int stepProgress = 0;

					int countProgress = File.ReadLines(SelectInputFile).Count();

					while ((line = reader.ReadLine()) != null)
					{
						var expressionResult = _calculatService.EvaluateExpression(line);

						if (expressionResult != null)
							await writer.WriteLineAsync(expressionResult.Result);

						stepProgress++;
						ProgressExpression = (stepProgress * 100) / countProgress;
					}
				}
			}
			else
			{
				MessageBox.Show("Vstupní soubor neexistuje.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			IsMethodStartRun = false;
		}
	}
}
