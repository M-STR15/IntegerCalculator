namespace IntegerCalculator.BE.ExpressionEvaluator.Models
{
	internal class Operation
	{
		public Operation(double left, double right, int startOriginalIndex, int endOriginalIndex, char @operator, string calculationPart)
		{
			Left = left;
			Right = right;
			StartOriginalIndex = startOriginalIndex;
			EndOriginalIndex = endOriginalIndex;
			Operator = @operator;
			CalculationPart = calculationPart;
		}

		public double Left { get; private set; }
		public double Right { get; private set; }
		public int StartOriginalIndex { get; private set; }
		public int EndOriginalIndex { get; private set; }
		public char Operator { get; private set; }
		public string CalculationPart { get; set; } = string.Empty;
		public double Value => Operator switch
		{
			'+' => Left + Right,
			'-' => Left - Right,
			'*' => Left * Right,
			'/' => Left / Right,
			_ => throw new InvalidOperationException($"Neznámý operátor: {Operator}")
		};

	}
}
