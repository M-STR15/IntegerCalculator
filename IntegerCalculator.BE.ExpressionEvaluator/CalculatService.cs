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

		public ExpressionResult EvaluateExpression(string expression)
		{
			expression = expression.Replace(" ", "");
			_stepNumber = 0;
			CalculationSteps = new();
			CalculationSteps.Add($"Vstupní výraz: '{expression}'");

			try
			{
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
									double value = operation.Operator switch
									{
										'*' => operation.Left * operation.Right,
										'/' => operation.Left / operation.Right,
										'+' => operation.Left + operation.Right,
										'-' => operation.Left - operation.Right,
										_ => throw new Exception("Neznámý operátor")
									};

									expression = expression.Substring(0, operation.StartOriginalIndex)
											   + value.ToString(System.Globalization.CultureInfo.InvariantCulture)
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

				CalculationSteps.Add($"Výsledek: '{expression}'");
			}
			catch (Exception ex)
			{
				EventLog.LogError(Guid.NewGuid(), ex, $"Chyba při výpočtu výrazu '{expression}': {ex.Message}");
			}

			return new ExpressionResult
			{
				Result = expression,
				CalculationSteps = CalculationSteps
			};
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
