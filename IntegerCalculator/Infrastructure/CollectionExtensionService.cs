using IntegerCalculator.BE.EventLog.Services;
using IntegerCalculator.Stores;
using IntegerCalculator.ViewModels;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegerCalculator.Infrastructure
{
	public static class CollectionExtensionService
	{
		public static IKernel AddIntegerCalculatorInfrastructure(this IKernel kernel)
		{
			kernel.Bind<ClasesStore>().To<ClasesStore>().InSingletonScope()
				.WithConstructorArgument("kernel", kernel);

			configWindows(kernel);
			configVieModels(kernel);
			return kernel;
		}

		private static IKernel configWindows(this IKernel kernel)
		{
			kernel.Bind<MainWindow>().To<MainWindow>().InSingletonScope();

			return kernel;
		}

		private static IKernel configVieModels(this IKernel kernel)
		{
			kernel.Bind<MainViewModel>().To<MainViewModel>().InSingletonScope();
			kernel.Bind<HandCalculatorViewModel>().To<HandCalculatorViewModel>().InSingletonScope();

			return kernel;
		}
	}
}
