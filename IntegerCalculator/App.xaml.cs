using IntegerCalculator.BE.EventLog.Services;
using Ninject;
using System.Windows;
using System.Windows.Input;
using IntegerCalculator.BE.ExpressionEvaluator.Infrastructure;
using IntegerCalculator.BE.EventLog.Infrastructure;
using IntegerCalculator.Infrastructure;
using IntegerCalculator.BE.Shared.Infrastructure;

namespace IntegerCalculator
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public IKernel Kernel { get; private set; }
		private IEventLogService _log;

		[STAThread]
		protected override void OnStartup(StartupEventArgs e)
		{
			_log = new EventLogService();
			// Zachycení neošetřených výjimek na úrovni aplikačního vlákna
			AppDomain.CurrentDomain.UnhandledException += currentDomain_UnhandledException;
			// Zachycení neošetřených výjimek na úrovni dispatcheru (UI vlákno)
			DispatcherUnhandledException += app_DispatcherUnhandledException;

			configureContainer();

			Current.MainWindow = Kernel.Get<MainWindow>();
			Current.MainWindow.DataContext = Kernel.Get<MainViewModel>();
			Current.MainWindow.Show();
		}

		private void configureContainer()
		{
			Kernel = new StandardKernel();

			Kernel.AddIntegerCalculatorBESharedInfrastructure();
			Kernel.AddIntegerCalculatorBEExpressionEvaluatorInfrastructure();
			Kernel.AddIntegerCalculatorBEEventLog();
			Kernel.AddIntegerCalculatorInfrastructure();
		}

		private void app_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			//_log.Error(Guid.Parse("c9c951ff-f176-4bcc-be5e-a87a9920c3e1"), $"Neočekávaná chyba (UI vlákno): {e.Exception.Message}");

			// Nastavení e.Handled na true zabrání aplikaci spadnout
			e.Handled = false;
		}

		private void currentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			//if (e.ExceptionObject is Exception ex)
			//	_log.Error(Guid.Parse("4fb577ad-9eba-4afa-9a89-368268989360"), $"Neočekávaná chyba (jiné vlákno): {ex.Message}");
			//else
			//	_log.Error(Guid.Parse("fbc0e288-2f92-45e7-a8fc-2c5136c0dec0"), $"Došlo k neošetřené výjimce, která není typu Exception.");

		}


		private bool _isDragging;
		private Point _startPoint;

		private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				_isDragging = true;
				_startPoint = e.GetPosition((IInputElement)sender);
				(sender as UIElement)?.CaptureMouse();
			}
		}


		private void Grid_MouseMove(object sender, MouseEventArgs e)
		{
			if (_isDragging)
			{
				var window = Application.Current.MainWindow;
				if (window != null)
				{
					var currentPosition = e.GetPosition(window);
					window.Left += currentPosition.X - _startPoint.X;
					window.Top += currentPosition.Y - _startPoint.Y;
				}
			}
		}

		private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
		{
			_isDragging = false;
			(sender as UIElement)?.ReleaseMouseCapture();
		}
	}
}
