
namespace IntegerCalculator.BE.EventLog.Models
{
	public class EventLog
	{
		public string ClassName { get; private set; } = string.Empty;
		public string MethodName { get; private set; } = string.Empty;
		public string Version { get; private set; }
		public Guid GuidId { get; set; }
		public string Message { get; set; } = string.Empty;
		private Exception? _exception;

		public Exception? Exception
		{
			get => _exception;
			set
			{
				if (_exception != value)
				{
					_exception = value;
					ClassName = _exception?.TargetSite?.DeclaringType?.Name ?? string.Empty;
					MethodName = _exception?.TargetSite?.Name ?? string.Empty;
				}
			}
		}

		public EventLog()
		{
			//Version = VersionSWHelper.GetVersionSW();
		}
	}
}