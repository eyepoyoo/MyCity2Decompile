using System;

namespace LEGO.CoreSDK
{
	// Token: 0x02000036 RID: 54
	public static class LogLevelMethods
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00003B18 File Offset: 0x00001D18
		public static bool ShouldLogMessageWithLevel(this LogLevel logLevel, LogLevel otherLogLevel)
		{
			return logLevel <= otherLogLevel;
		}
	}
}
