using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IntegerCalculator.UIComponents
{
	/// <summary>
	/// Interaction logic for Display.xaml
	/// </summary>
	public partial class Display : UserControl
	{
		private static readonly char[] AllowedChars =
	   "0123456789+-*/".ToCharArray();

		public Display()
		{
			InitializeComponent();
		}

		public string Value
		{
			get => (string)GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(nameof(Value), typeof(string), typeof(Display));

		private void DisplayText_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			// povolit jen čísla a operátory
			if (!AllowedChars.Contains(e.Text.First()))
			{
				e.Handled = true; // nepovolit vstup
			}
		}

		private void DisplayText_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			// povolit Backspace, Delete, Left, Right
			if (e.Key == Key.Back || e.Key == Key.Delete ||
				e.Key == Key.Left || e.Key == Key.Right)
			{
				return;
			}

			// blokovat vše ostatní, co není TextInput
			if (e.Key == Key.Space)
			{
				e.Handled = true;
			}
		}
	}
}
