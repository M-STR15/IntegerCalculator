using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Configuration;
using System.IO;
using eventLog = IntegerCalculator.BE.EventLog.Models.EventLog;

namespace IntegerCalculator.BE.EventLog.Services
{
	public partial class EventLogService : IEventLogService
	{
		public EventLogService()
		{
			var outputTempleteFormat = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
			var directory = Path.GetDirectoryName(config.FilePath);
			var logFilePath = Path.Combine(directory, "Loging", "log-.json");

#if DEBUG
			Log.Logger = new LoggerConfiguration()
						.WriteTo.Console(outputTemplate: outputTempleteFormat)
						.WriteTo.File(new JsonFormatter(), logFilePath, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Debug)
						.CreateLogger();
#else
			Log.Logger = new LoggerConfiguration()
						.WriteTo.Console(outputTemplate: outputTempleteFormat)
						.WriteTo.File(new JsonFormatter(), logFilePath, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information)
						.CreateLogger();
#endif
		}

		public void LogFatal(Guid guidId, Exception exception, string? message = null, object? inputObject = null)
		{
			if (inputObject != null)
				exception.Data.Add("InputObject", inputObject);

			var eventLog = getEventLog(guidId, exception, message);
			Log.Fatal("{@EventLog}", eventLog);
		}

		/// <summary>
		/// Loguje chybu
		/// </summary>
		/// <param name="guidId">Unikátní označení, pod kterým bude evidovaná daná událost.</param>
		/// <param name="message">Zpráva k dané události.</param>
		public void LogError(Guid guidId, Exception exception, string? message = null, object? inputObject = null)
		{
			if (inputObject != null)
				exception.Data.Add("InputObject", inputObject);

			var eventLog = getEventLog(guidId, exception, message);
			Log.Error("{@EventLog}", eventLog);
		}

		/// <summary>
		/// Loguje varování
		/// </summary>
		/// <param name="guidId">Unikátní označení, pod kterým bude evidovaná daná událost.</param>
		/// <param name="message">Zpráva k dané události.</param>
		public void LogWarning(Guid guidId, Exception exception, string? message = null, object? inputObject = null)
		{
			if (inputObject != null)
				exception.Data.Add("InputObject", inputObject);

			var eventLog = getEventLog(guidId, exception, message);
			Log.Warning("{@EventLog}", eventLog);
		}

		/// <summary>
		/// Loguje informaci
		/// </summary>
		/// <param name="guidId">Unikátní označení, pod kterým bude evidovaná daná událost.</param>
		/// <param name="message">Zpráva k dané události.</param>
		public void LogInformation(Guid guidId, Exception? exception, string? message = null, object? inputObject = null)
		{
			if (inputObject != null && exception != null)
				exception.Data.Add("InputObject", inputObject);

			var eventLog = getEventLog(guidId, exception, message);
			Log.Information("{@EventLog}", eventLog);
		}

		public void LogDebug(Guid guidId, Exception exception, string? message = null, object? inputObject = null)
		{
			if (inputObject != null)
				exception.Data.Add("InputObject", inputObject);

			var eventLog = getEventLog(guidId, exception, message);
			Log.Debug("{@EventLog}", eventLog);
		}

		private eventLog getEventLog(Guid guidId, Exception? exception, string? message = null)
		{
			return new eventLog
			{
				GuidId = guidId,
				Message = string.IsNullOrEmpty(message) ? (exception?.Message ?? "") : message,
				Exception = exception,
			};
		}
	}
}