using System.Reflection;

namespace IntegerCalculator.BE.Shared.Helpers
{
	public class VersionSWHelper
	{
		public static string GetVersionSW()
		{
			//var assembly = Assembly.GetExecutingAssembly();
			//var version = FileVersionInfo.GetVersionInfo(assembly.Location);
			var version = Assembly.GetEntryAssembly()?.GetName()?.Version;
			var versionStr = string.Format($"{version?.Major}.{version?.Minor}.{version?.Build}");
			var typeRelease = " DEBUG";
#if RELEASE
			typeRelease = "";
#endif
#if RELEASE_TOS_SERVER
			typeRelease = "";
#endif
			return string.Format("v{0}.{1}", versionStr, typeRelease);
		}
	}
}