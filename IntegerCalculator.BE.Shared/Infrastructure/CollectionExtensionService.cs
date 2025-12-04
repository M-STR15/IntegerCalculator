using IntegerCalculator.BE.Shared.Helpers;
using Ninject;

namespace IntegerCalculator.BE.Shared.Infrastructure
{
	public static class CollectionExtensionService
	{
		public static IKernel AddIntegerCalculatorBESharedInfrastructure(this IKernel kernel)
		{
			kernel.Bind<VersionSWHelper>().To<VersionSWHelper>().InSingletonScope();

			return kernel;
		}
	}
}
