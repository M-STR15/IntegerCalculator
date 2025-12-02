using IntegerCalculator.BE.ExpressionEvaluator;
using IntegerCalculator.Tests.BE.ExpressionsEvaluatorTestt.Fakes;

namespace IntegerCalculator.Tests.BE.ExpressionsEvaluatorTest
{
	public class CalculatServiceTests
	{

		[Fact]
		public void EvaluateExpression_SimpleAddition_ReturnsSum()
		{
			var log = new FakeEventLogService();
			var svc = new CalculatService(log);

			var result = svc.EvaluateExpression("2+3");

			Assert.Equal("5", result.Result);
		}

		[Fact]
		public void EvaluateExpression_OperatorPrecedence_MultiplicationBeforeAddition()
		{
			var log = new FakeEventLogService();
			var svc = new CalculatService(log);

			var result = svc.EvaluateExpression("3*4+2");

			Assert.Equal("14", result.Result);
			// history should include input and final result markers
			Assert.Contains(result.CalculationSteps, s => s.StartsWith("Vstupní výraz:"));
			Assert.Contains(result.CalculationSteps, s => s.StartsWith("Výsledek:"));
		}

		[Theory]
		[InlineData("3*4+2", "14")]
		[InlineData("3 * 4 + 2", "14")]
		[InlineData("2+3*4", "14")]
		public void EvaluateExpression_OperatorPrecedence(string formula, string resultTest)
		{
			var log = new FakeEventLogService();
			var svc = new CalculatService(log);

			var result = svc.EvaluateExpression(formula);

			Assert.Equal(resultTest, result.Result);
		}


		[Fact]
		public void EvaluateExpression_DivisionAndSubtraction_WorksCorrectly()
		{
			var log = new FakeEventLogService();
			var svc = new CalculatService(log);

			var result = svc.EvaluateExpression("10/2-3");

			Assert.Equal("2", result.Result);
		}

		[Fact]
		public void EvaluateExpression_InvalidExpression_LogsError()
		{
			var log = new FakeEventLogService();
			var svc = new CalculatService(log);

			var result = svc.EvaluateExpression("2++3");

			Assert.True(log.ErrorLogged, "Očekávaná chyba protokolu bude volána pro neplatný výraz.");
		}

		[Fact]
		public void EvaluateExpression_CalculationSteps_AreRecorded()
		{
			var log = new FakeEventLogService();
			var svc = new CalculatService(log);

			var result = svc.EvaluateExpression("2+3*4");

			Assert.Equal("Vstupní výraz: '2+3*4'", result.CalculationSteps[0]);
			Assert.Equal($"Výsledek: '{result.Result}'", result.CalculationSteps[^1]);

			Assert.True(result.CalculationSteps.Count == 3);
			Assert.Contains(result.CalculationSteps, s => s.Contains("krok výpočtu"));
		}
	}
}

