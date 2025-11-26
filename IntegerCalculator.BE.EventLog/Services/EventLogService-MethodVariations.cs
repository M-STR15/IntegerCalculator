namespace IntegerCalculator.BE.EventLog.Services
{
	public partial class EventLogService
	{
		/// <summary>
		/// Loguje chybu
		/// </summary>
		/// <param name="guidId">Unikátní označení, pod kterým bude evidovaná daná událost.</param>
		/// <param name="message">Zpráva k dané události.</param>
		/// <returns></returns>
		public async Task LogErrorAsync(Guid guidId, Exception exception, string? message = null, object? inputObject = null)
		{
			await Task.Run(() => LogError(guidId, exception, message, inputObject));
		}

		/// <summary>
		/// Loguje chybu
		/// </summary>
		/// <param name="guidId">Unikátní označení, pod kterým bude evidovaná daná událost.</param>
		/// <param name="exception">Vygenerovaný exception z dané události.</param>
		/// <returns></returns>
		public async Task LogErrorAsync(Guid guidId, Exception exception, object? inputObject = null)
		{
			await Task.Run(() => LogError(guidId, exception, null, inputObject));
		}

		/// <summary>
		/// Loguje varování
		/// </summary>
		/// <param name="guidId">Unikátní označení, pod kterým bude evidovaná daná událost.</param>
		/// <param name="message">Zpráva k dané události.</param>
		/// <returns></returns>
		public async Task LogWarningAsync(Guid guidId, Exception exception, string? message = null, object? inputObject = null)
		{
			await Task.Run(() => LogWarning(guidId, exception, message, inputObject));
		}/// <summary>

		 /// Loguje varování
		 /// </summary>
		 /// <param name="guidId">Unikátní označení, pod kterým bude evidovaná daná událost.</param>
		 /// <param name="exception">Vygenerovaný exception z dané události.</param>
		 /// <returns></returns>
		public async Task LogWarningAsync(Guid guidId, Exception exception, object? inputObject = null)
		{
			await Task.Run(() => LogWarning(guidId, exception, null, inputObject));
		}

		/// <summary>
		/// Loguje informaci
		/// </summary>
		/// <param name="guidId">Unikátní označení, pod kterým bude evidovaná daná událost.</param>
		/// <param name="message">Zpráva k dané události.</param>
		/// <returns></returns>
		public async Task LogInformationAsync(Guid guidId, string? message = null, object? inputObject = null)
		{
			await Task.Run(() => LogInformation(guidId, null, message, inputObject));
		}

		/// <summary>
		/// Loguje informaci
		/// </summary>
		/// <param name="guidId">Unikátní označení, pod kterým bude evidovaná daná událost.</param>
		/// <param name="message">Zpráva k dané události.</param>
		/// <returns></returns>
		public async Task LogInformationAsync(Guid guidId, Exception exception, string? message = null, object? inputObject = null)
		{
			await Task.Run(() => LogInformation(guidId, exception, message, inputObject));
		}

		/// <summary>
		/// Loguje informaci
		/// </summary>
		/// <param name="guidId">Unikátní označení, pod kterým bude evidovaná daná událost.</param>
		/// <param name="exception">Vygenerovaný exception z dané události.</param>
		/// <returns></returns>
		public async Task LogInformationAsync(Guid guidId, Exception exception, object? inputObject = null)
		{
			await Task.Run(() => LogInformation(guidId, exception, null, inputObject));
		}
	}
}