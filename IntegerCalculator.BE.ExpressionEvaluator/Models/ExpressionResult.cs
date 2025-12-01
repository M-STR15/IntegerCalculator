namespace IntegerCalculator.BE.ExpressionEvaluator.Models
{
	public class ExpressionResult
	{
		public string Result { get; set; } = string.Empty;
		public List<string> CalculationSteps { get; set; } = new List<string>();
	}
}
