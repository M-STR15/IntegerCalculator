using IntegerCalculator.BE.EventLog.Services;
using Ninject;

namespace IntegerCalculator.BE.EventLog.Infrastructure
{
	public static class CollectionExtensionService
	{
		public static IKernel AddIntegerCalculatorBEEventLog(this IKernel kernel)
		{
			kernel.Bind<IEventLogService>().To<EventLogService>().InSingletonScope();

			return kernel;
		}
	}
}
