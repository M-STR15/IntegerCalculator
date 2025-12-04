namespace IntegerCalculator.BE.ExpressionEvaluator
{
	public class FormulaGenerator
	{
		public int NumberOfFormula { get; private set; }
		private IList<char> _operators;

		public FormulaGenerator(int numberOfFormula)
		{
			NumberOfFormula = numberOfFormula;
			_operators = new List<char> { '/', '*', '+', '-' };
		}

		public IList<string> GenerateFormulas()
		{
			var formulas = new List<string>();
			string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

			for (int i = 0; i < NumberOfFormula; i++)
			{
				var countOperators = _operators.Count;
				var numberOfOperators = new Random().Next(1, countOperators); // Náhodný počet operátorů mezi 1 a 4
				var formula = "";
				formula = new Random().Next(-1000, 1000).ToString();
				for (int o = 0; o < numberOfOperators; o++)
				{
					var selectOperator = new Random().Next(0, countOperators - 1);
					formula += _operators[selectOperator];
					var generateAnError = new Random().Next(0, 1);
					if (generateAnError == 0)
						formula += new Random().Next(-1000, 1000).ToString();
					else
					{
						char letter = letters[new Random().Next(letters.Length)];
						formula += letter.ToString();
					}
				}

				formulas.Add(formula);
			}

			return formulas;
		}
	}
}
