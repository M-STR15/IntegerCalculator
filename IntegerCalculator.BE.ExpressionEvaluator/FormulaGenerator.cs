namespace IntegerCalculator.BE.ExpressionEvaluator
{
	public class FormulaGenerator : IFormulaGenerator
	{
		private IList<char> _operators;

		public FormulaGenerator()
		{
			_operators = new List<char> { '/', '*', '+', '-' };
		}

		public async Task<IList<string>> GenerateFormulasAsync(int numberOfFormula)
		{
			return await Task.Run(() =>
			{
				var formulas = new List<string>();
				string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
				var rnd = new Random();

				for (int i = 0; i < numberOfFormula; i++)
				{
					var formula = GenerateFormulaAsync().Result;
					formulas.Add(formula);
				}

				return (IList<string>)formulas;
			});
		}

		public async Task<string> GenerateFormulaAsync()
		{
			return await Task.Run(() =>
		   {
			   string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			   var rnd = new Random();


			   var countOperators = _operators.Count;
			   var numberOfOperators = rnd.Next(1, countOperators);

			   var formula = rnd.Next(-1000, 1000).ToString();

			   for (int o = 0; o < numberOfOperators; o++)
			   {
				   var selectOperator = rnd.Next(0, countOperators);
				   formula += _operators[selectOperator];

				   bool generateAnError = rnd.NextDouble() < 0.1;

				   if (generateAnError)
				   {
					   char letter = letters[rnd.Next(letters.Length)];
					   formula += letter;
				   }
				   else
				   {
					   formula += rnd.Next(-1000, 1000).ToString();
				   }
			   }

			   return formula;
		   });
		}
	}
}
