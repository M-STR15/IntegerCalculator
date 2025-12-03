using IntegerCalculator.BE.ExpressionEvaluator;
using IntegerCalculator.Tests.BE.ExpressionsEvaluatorTestt.Fakes;
using System.Globalization;
using System.Numerics;

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
		[InlineData("2+5/5", "3")]
		[InlineData("2-5/5", "1")]
		[InlineData("5-5", "0")]
		[InlineData("5-15", "-10")]
		[InlineData("1+1+2+3-4/4*2", "5")]
		[InlineData("1+-1", "0")]
		[InlineData("1--1", "2")]
		[InlineData("-72+-250", "-322")]
		[InlineData("390*100*-434", "-16926000")]
		[InlineData("-318/-671/327*-223", "0")]
		public void EvaluateExpression_OperatorPrecedence(string formula, string resultTest)
		{
			var log = new FakeEventLogService();
			var svc = new CalculatService(log);

			var result = svc.EvaluateExpression(formula);

			// parsování řetězce na double
			if (!double.TryParse(result?.Result, NumberStyles.Float, CultureInfo.InvariantCulture, out double dblValue))
			{
				Assert.True(false, "Výsledek nelze převést na číslo.");
			}

			// zaokrouhlení na celé číslo
			var roundedValue = Math.Round(dblValue, MidpointRounding.AwayFromZero);

			// převod na BigInteger
			BigInteger parsedValue = new BigInteger(roundedValue);

			// test
			Assert.True(true, "Výsledek je nyní analyzovatelný jako velké celé číslo.");
			var conValue = double.Parse(resultTest, CultureInfo.InvariantCulture);
			var controlValue = Math.Round(conValue, 5, MidpointRounding.AwayFromZero);
			var resVaue = double.Parse(result.Result, CultureInfo.InvariantCulture);
			var resultValue = Math.Round(resVaue, 5, MidpointRounding.AwayFromZero);
			Assert.Equal(controlValue, resultValue);
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
		public void EvaluateExpression_RoundingCheck()
		{
			var log = new FakeEventLogService();
			var svc = new CalculatService(log);

			var result = svc.EvaluateExpression("2/3");

			Assert.Equal("0", result?.Result);
		}

		[Theory]
		[InlineData("a + 1", "Error - Invalid character: 'a'")]
		[InlineData("2.1*2", "Error - Invalid character: '.'")]
		public void EvaluateExpression_CheckAllowedCharacters(string formula, string resultTest)
		{
			var log = new FakeEventLogService();
			var svc = new CalculatService(log);

			var result = svc.EvaluateExpression(formula);

			Assert.Equal(resultTest, result.Result);
		}

		[Fact]
		public void EvaluateExpression_CheckNullRow()
		{
			var log = new FakeEventLogService();
			var svc = new CalculatService(log);

			var result = svc.EvaluateExpression("");

			Assert.Equal(null, result);
		}
	}
}

