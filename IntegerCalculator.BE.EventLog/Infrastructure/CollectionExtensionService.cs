using IntegerCalculator.BE.EventLog.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IntegerCalculator.BE.EventLog.Infrastructure
{
	public static class CollectionExtensionService
	{
		public static IServiceCollection AddIntegerCalculatorBEEventLog(this IServiceCollection services)
		{
			services.AddSingleton<IEventLogService, EventLogService>();

			return services;
		}
	}
}
