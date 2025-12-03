using IntegerCalculator.BE.EventLog.Services;
using IntegerCalculator.BE.ExpressionEvaluator.Models;
using System.Globalization;

namespace IntegerCalculator.BE.ExpressionEvaluator
{
	public class CalculatService
	{
		public List<string> CalculationSteps { get; set; } = new();
		public IEventLogService EventLog { get; }
		private int _stepNumber = 0;

		private IList<char> _allowedCharacters = new List<char>
		{
			'0','1','2','3','4','5','6','7','8','9',
			'+','-','*','/'
		};

		// Operátory rozdělené podle precedence
		private readonly List<List<char>> _operatorsPrecedence = new()
	{
		new List<char> { '*', '/' }, // nejvyšší priorita
        new List<char> { '+', '-' }  // nižší priorita
    };

		public CalculatService(IEventLogService eventLog)
		{
			CalculationSteps.Clear();
			EventLog = eventLog;
		}

		private (bool IsValid, char? InvalidChar) checkAllowedCharacters(string expression)
		{
			for (int i = 0; i < expression.Length; i++)
			{
				if (!_allowedCharacters.Contains(expression[i]))
				{
					return (false, expression[i]); // první nepovolený znak a jeho index
				}
			}
			return (true, null); // všechny znaky OK
		}

		public ExpressionResult? EvaluateExpression(string expression)
		{
			try
			{
				expression = expression.Replace(" ", "");

				if (string.IsNullOrEmpty(expression))
					return default;

				var allowedCharacter = checkAllowedCharacters(expression);
				if (!allowedCharacter.IsValid)
				{
					return new ExpressionResult
					{
						Result = $"Error - Invalid character: '{allowedCharacter.InvalidChar}'"
					};
				}

				_stepNumber = 0;
				CalculationSteps = new();
				CalculationSteps.Add($"Vstupní výraz: '{expression}'");
				// Pro každý "level" precedence
				foreach (var ops in _operatorsPrecedence)
				{
					bool hasOperator;
					do
					{
						hasOperator = false;
						for (int i = 0; i < expression.Length; i++)
						{
							if (ops.Contains(expression[i]) && isBinaryOperator(expression, i))
							{
								var operation = findOperations(expression, expression[i]);
								if (operation != null)
								{
									double value = operation.Value;

									expression = expression.Substring(0, operation.StartOriginalIndex)
											   + value.ToString(CultureInfo.InvariantCulture)
											   + expression.Substring(operation.EndOriginalIndex);

									_stepNumber++;
									CalculationSteps.Add($"{_stepNumber} krok výpočtu: '{expression}'");

									hasOperator = true;
									break; // restart od začátku, aby se zachovalo pořadí zleva doprava
								}
							}
						}
					} while (hasOperator);
				}

				checkDotAndRound(ref expression);

				CalculationSteps.Add($"Výsledek: '{expression}'");

				return new ExpressionResult
				{
					Result = expression,
					CalculationSteps = CalculationSteps
				};
			}
			catch (Exception ex)
			{
				EventLog.LogError(Guid.NewGuid(), ex, $"Chyba při výpočtu výrazu '{expression}': {ex.Message}");
				return default;
			}
		}

		private void checkDotAndRound(ref string expr)
		{
			int dotIndex = expr.IndexOf('.');
			if (dotIndex >= 0)
			{
				expr = expr.Substring(0, dotIndex); // vezme jen část před tečkou
			}
		}

		private static bool isBinaryOperator(string expr, int index)
		{
			if (index == 0)
				return false;

			char left = expr[index - 1];
			return char.IsDigit(left) || left == ')' || left == '.';
		}

		private static Operation? findOperations(string expr, char opChar)
		{
			for (int i = 0; i < expr.Length; i++)
			{
				if (expr[i] != opChar)
					continue;

				if ((opChar == '+' || opChar == '-') && !isBinaryOperator(expr, i))
					continue;

				int rStart = i + 1;
				if (rStart >= expr.Length)
					continue;

				int r = rStart;
				if (r < expr.Length && (expr[r] == '+' || expr[r] == '-') && !isBinaryOperator(expr, r))
					r++;

				while (r < expr.Length && (char.IsDigit(expr[r]) || expr[r] == '.'))
					r++;

				string rightStr = expr.Substring(rStart, r - rStart);

				int lEnd = i - 1;
				if (lEnd < 0)
					continue;

				int l = lEnd;
				while (l >= 0 && (char.IsDigit(expr[l]) || expr[l] == '.'))
					l--;

				if (l >= 0 && (expr[l] == '+' || expr[l] == '-') && !isBinaryOperator(expr, l))
					l--;

				int lStart = l + 1;
				string leftStr = expr.Substring(lStart, lEnd - lStart + 1);

				if (!double.TryParse(leftStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double leftVal))
					continue;
				if (!double.TryParse(rightStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double rightVal))
					continue;

				return new Operation(leftVal, rightVal, lStart, r, opChar, expr.Substring(lStart, r - lStart));
			}

			return null;
		}
	}
}
