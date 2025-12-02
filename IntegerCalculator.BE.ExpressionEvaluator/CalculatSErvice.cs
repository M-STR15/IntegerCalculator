using IntegerCalculator.BE.EventLog.Services;
using IntegerCalculator.BE.ExpressionEvaluator.Models;
using System.Numerics;

namespace IntegerCalculator.BE.ExpressionEvaluator
{
	public class CalculatService
	{
		public List<string> CalculationSteps { get; set; } = new();
		public IEventLogService EventLog { get; }
		private int _stepNumber = 0;

		private ICollection<char> _operators = new List<char> { '*', '/', '+', '-' };

		public CalculatService(IEventLogService eventLog)
		{
			CalculationSteps.Clear();
			EventLog = eventLog;
		}
		/// <summary>
		/// Vyhodnotí aritmetický výraz zadaný v parametru <paramref name="expression"/>.
		/// Postupně provede výpočty podle precedence operátorů: nejprve násobení ('*'),
		/// poté dělení ('/'), následně sčítání ('+') a odčítání ('-').
		/// </summary>
		/// <param name="expression">Řetězec obsahující aritmetický výraz k vyhodnocení.</param>
		/// <returns>
		/// Řetězec představující výsledek výrazu po provedení všech operací.
		/// V případě chyby může být vrácen částečný nebo původní výraz; detaily chyby jsou zalogovány.
		/// </returns>
		/// <remarks>
		/// Metoda ořezává počáteční a koncové mezery voláním <see cref="string.Trim()"/> a
		/// následně volá interní metodu <see cref="calculationOperator(ref string, char)"/>
		/// pro každý operátor v pořadí precedence. Výjimky uvnitř jsou zachyceny a ohlášeny
		/// pomocí služby <see cref="IEventLogService"/>.
		/// </remarks>
		/// <exception cref="Exception">
		/// Jakákoli neošetřená výjimka během výpočtu je zachycena a zalogována; metoda však výjimku
		/// znovu nevyhazuje.
		/// </exception>
		public ExpressionResult EvaluateExpression(string expression)
		{
			expression = expression.Replace(" ", "");
			_stepNumber = default;
			CalculationSteps = new();
			CalculationSteps.Add($"Vstupní výraz: '{expression}'");

			foreach (var opChar in _operators)
			{
				calculationOperator(ref expression, opChar, true);
			}

			CalculationSteps.Add($"Výsledek: '{expression}'");
			var expressionResult = new ExpressionResult
			{
				Result = expression,
				CalculationSteps = CalculationSteps
			};

			return expressionResult;
		}


		private void calculationOperator(ref string expression, char opChar, bool withHistory = false)
		{
			var isCompleteCalculation = false;
			try
			{
				while (!isCompleteCalculation)
				{
					var operation = findOperations(expression, opChar);

					if (operation != null)
					{
						var lengBeforeOperation = operation.StartOriginalIndex;
						var firstIndexAfterOperation = operation.EndOriginalIndex;

						var textBefore = expression.Substring(0, lengBeforeOperation);
						var textAfter = expression.Substring(firstIndexAfterOperation);

						expression =
						  textBefore			// před úsekem
						   + operation.Value	// nový text
						   + textAfter;			// za úsekem

						isCompleteCalculation = isCalculationComplete(expression, opChar);
						var isExistNextOperation = existNextOperation(expression);
						if (isExistNextOperation && withHistory)
						{
							_stepNumber++;
							CalculationSteps.Add($"{_stepNumber} krok výpočtu: '{expression}'");
						}
					}
					else
					{
						isCompleteCalculation = true;
					}
				}
			}
			catch (Exception ex)
			{
				var message = $"Chyba při výpočtu výrazu '{expression}' s operátorem '{opChar}': {ex.Message}";
				EventLog.LogError(Guid.Parse("ed931086-e49f-460a-b6d8-23584a3c7dd5"), ex, message);
			}
		}

		private bool existNextOperation(string expression) => _operators.Any(z => expression.Contains(z));

		private bool isCalculationComplete(string expression, char opChar)
		{
			var isResult = BigInteger.TryParse(expression, out _);
			var isOperatorInExpession = expression.Contains(opChar);

			var isContinueCaltulationActulOperator = !isResult && isOperatorInExpession;

			return isResult || !isContinueCaltulationActulOperator;
		}

		private static Operation? findOperations(string expression, char opChar)
		{
			string cleaned = expression.Replace(" ", ""); // odstraníme mezery pro hledání operandů

			for (int i = 0; i < cleaned.Length; i++)
			{
				if (cleaned[i] == opChar)
				{
					// levý operand
					int leftStart = i - 1;
					while (leftStart >= 0 && (char.IsDigit(cleaned[leftStart]) || cleaned[leftStart] == '.'))
						leftStart--;

					string leftStr = cleaned.Substring(leftStart + 1, i - leftStart - 1);

					// pravý operand
					int rightEnd = i + 1;
					while (rightEnd < cleaned.Length && (char.IsDigit(cleaned[rightEnd]) || cleaned[rightEnd] == '.'))
						rightEnd++;

					string rightStr = cleaned.Substring(i + 1, rightEnd - i - 1);

					// uložíme oblast podle indexů v původním stringu (s mezerami)
					int startOriginalIndex = leftStart + 1;
					int endOriginalIndex = rightEnd + rightStr.Length - 1;
					var lengCalculationPart = endOriginalIndex - startOriginalIndex;
					var calculationPart = expression.Substring(startOriginalIndex, lengCalculationPart);
					return new Operation(double.Parse(leftStr), double.Parse(rightStr), startOriginalIndex, endOriginalIndex, opChar, calculationPart);
				}
			}

			return null;
		}
	}
}
