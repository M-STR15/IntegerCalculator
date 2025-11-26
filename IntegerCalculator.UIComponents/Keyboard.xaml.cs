using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IntegerCalculator.UIComponents
{
	/// <summary>
	/// Interaction logic for Keyboard.xaml
	/// </summary>
	public partial class Keyboard : UserControl
	{
		public static readonly DependencyProperty ClickCharacterCommandProperty =
		DependencyProperty.Register(
			nameof(ClickCharacterCommand),
			typeof(ICommand),
			typeof(Keyboard),
			new PropertyMetadata(null));

		public ICommand ClickCharacterCommand
		{
			get => (ICommand)GetValue(ClickCharacterCommandProperty);
			set => SetValue(ClickCharacterCommandProperty, value);
		}

		public static readonly DependencyProperty ClickCharacterCommandParameterProperty =
			  DependencyProperty.Register(
				  nameof(ClickCharacterCommandParameter),
				  typeof(object),
				  typeof(Keyboard),
				  new PropertyMetadata(null));

		public object ClickCharacterCommandParameter
		{
			get => GetValue(ClickCharacterCommandParameterProperty);
			set => SetValue(ClickCharacterCommandParameterProperty, value);
		}

		public Keyboard()
		{
			InitializeComponent();
		}

		// Metoda, kterou zavoláte při kliknutí na tlačítko
		private void OnCharacterButtonClick(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			var character = button?.Content?.ToString();

			if (ClickCharacterCommand?.CanExecute(character) == true)
			{
				ClickCharacterCommand.Execute(character);
			}
		}
	}
}
