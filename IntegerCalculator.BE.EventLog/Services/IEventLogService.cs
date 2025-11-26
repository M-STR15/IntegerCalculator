namespace IntegerCalculator.BE.EventLog.Services
{
	public interface IEventLogService
	{
		void LogError(Guid guidId, Exception exception, string? message = null, object? inputObject = null);

		Task LogErrorAsync(Guid guidId, Exception exception, object? inputObject = null);

		Task LogErrorAsync(Guid guidId, Exception exception, string? message = null, object? inputObject = null);

		void LogInformation(Guid guidId, Exception exception, string? message = null, object? inputObject = null);

		Task LogInformationAsync(Guid guidId, string? message = null, object? inputObject = null);

		Task LogInformationAsync(Guid guidId, Exception exception, object? inputObject = null);

		Task LogInformationAsync(Guid guidId, Exception exception, string? message = null, object? inputObject = null);

		void LogWarning(Guid guidId, Exception exception, string? message = null, object? inputObject = null);

		Task LogWarningAsync(Guid guidId, Exception exception, object? inputObject = null);

		Task LogWarningAsync(Guid guidId, Exception exception, string? message = null, object? inputObject = null);
	}
}