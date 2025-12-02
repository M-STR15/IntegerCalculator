using IntegerCalculator.BE.EventLog.Services;

namespace IntegerCalculator.Tests.BE.ExpressionsEvaluatorTestt.Fakes
{
	internal class FakeEventLogService : IEventLogService
	{
		public bool ErrorLogged { get; private set; }
		public Exception? LoggedException { get; private set; }
		public string? LoggedMessage { get; private set; }

		public void LogError(Guid guidId, Exception exception, string? message = null, object? inputObject = null)
		{
			ErrorLogged = true;
			LoggedException = exception;
			LoggedMessage = message;
		}

		public Task LogErrorAsync(Guid guidId, Exception exception, object? inputObject = null)
		{
			LogError(guidId, exception, null, inputObject);
			return Task.CompletedTask;
		}

		public Task LogErrorAsync(Guid guidId, Exception exception, string? message = null, object? inputObject = null)
		{
			LogError(guidId, exception, message, inputObject);
			return Task.CompletedTask;
		}

		public void LogInformation(Guid guidId, Exception exception, string? message = null, object? inputObject = null) { }
		public Task LogInformationAsync(Guid guidId, string? message = null, object? inputObject = null) => Task.CompletedTask;
		public Task LogInformationAsync(Guid guidId, Exception exception, object? inputObject = null) => Task.CompletedTask;
		public Task LogInformationAsync(Guid guidId, Exception exception, string? message = null, object? inputObject = null) => Task.CompletedTask;
		public void LogWarning(Guid guidId, Exception exception, string? message = null, object? inputObject = null) { }
		public Task LogWarningAsync(Guid guidId, Exception exception, object? inputObject = null) => Task.CompletedTask;
		public Task LogWarningAsync(Guid guidId, Exception exception, string? message = null, object? inputObject = null) => Task.CompletedTask;
	}
}
