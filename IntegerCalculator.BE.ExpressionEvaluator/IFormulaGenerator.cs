
namespace IntegerCalculator.BE.ExpressionEvaluator
{
	public interface IFormulaGenerator
	{
		Task<string> GenerateFormulaAsync();
		Task<IList<string>> GenerateFormulasAsync(int numberOfFormula);
	}
}