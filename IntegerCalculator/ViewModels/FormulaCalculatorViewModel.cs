using CommunityToolkit.Mvvm.ComponentModel;
using IntegerCalculator.BE.EventLog.Services;
using IntegerCalculator.BE.ExpressionEvaluator;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace IntegerCalculator.ViewModels
{
	public partial class FormulaCalculatorViewModel:ObservableObject
	{
		private IEventLogService _eventLogService;
		[ObservableProperty]
		private string _selectInputFile;

		[ObservableProperty]
		private string _selectOutputFile;

		private bool isMethodGenerateFileRun = false;
		private bool isMethodSelectInputFileRun = false;
		private bool isMethodSelectOutputFileRun = false;
		private bool isMethodStartRun = false;
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

			var actualFolder = System.IO.Directory.GetCurrentDirectory();

			SelectInputFile = string.Format($"{actualFolder}\\Input_Formulas.txt");
			SelectOutputFile = string.Format($"{actualFolder}\\Output_FormulaResults.txt");
		}

		private bool canGenerateFile() => !isMethodGenerateFileRun;

		private bool canSelectInputFile() => !isMethodSelectInputFileRun;

		private bool canSelectOutputFile() => !isMethodSelectOutputFileRun;

		private bool canStart() => !isMethodStartRun && !string.IsNullOrEmpty(SelectInputFile) && !string.IsNullOrEmpty(SelectOutputFile);

		private async void onGenerateInputFile(object parameter)
		{
			isMethodGenerateFileRun = true;

			var formuLaGenerator = new FormulaGenerator(1000);
			var listFormulas = formuLaGenerator.GenerateFormulas();

			await File.WriteAllLinesAsync(SelectInputFile, listFormulas);

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
			isMethodStartRun = true;

			var existInputFile = File.Exists(SelectInputFile);
			var listWithResults = new List<string>();
			if (existInputFile)
			{
				var formulas = File.ReadAllLines(SelectInputFile);
				foreach (var item in formulas)
				{
					var expressionResult = _calculatService.EvaluateExpression(item);
					if (expressionResult != null)
						listWithResults.Add(expressionResult.Result);
				}

				await File.WriteAllLinesAsync(SelectOutputFile, listWithResults);
			}
			else
			{
				MessageBox.Show("Vstupní soubor neexistuje.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			isMethodStartRun = false;
		}
	}
}
