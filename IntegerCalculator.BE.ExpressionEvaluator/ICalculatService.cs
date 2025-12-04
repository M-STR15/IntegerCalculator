using IntegerCalculator.BE.ExpressionEvaluator.Models;

namespace IntegerCalculator.BE.ExpressionEvaluator
{
	public interface ICalculatService
	{
		ExpressionResult? EvaluateExpression(string expression, bool withCalculationSteps = false);
		Task<ExpressionResult?> EvaluateExpressionAsync(string expression, bool withCalculationSteps = false);
	}
}