using IntegerCalculator.ViewModels;
using Ninject;

namespace IntegerCalculator.Stores
{
	public class ClasesStore
	{
		private IKernel _kernel { get; set; }
		public ClasesStore(IKernel kernel)
		{
			_kernel = kernel;
		}

		public HandCalculatorViewModel HandCalculatorViewModel => _kernel.Get<HandCalculatorViewModel>();
	}
}
