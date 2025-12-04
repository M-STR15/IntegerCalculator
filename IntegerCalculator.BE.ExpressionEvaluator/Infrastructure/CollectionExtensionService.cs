using Ninject;

namespace IntegerCalculator.BE.ExpressionEvaluator.Infrastructure
{
	public static class CollectionExtensionService
	{
		public static IKernel AddIntegerCalculatorBEExpressionEvaluatorInfrastructure(this IKernel kernel)
		{
			kernel.Bind<CalculatService>().To<CalculatService>().InSingletonScope();
			kernel.Bind<ICalculatService>().To<CalculatService>().InSingletonScope();

			return kernel;
		}
	}
}
