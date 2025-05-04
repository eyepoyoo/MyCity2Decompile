using System;
using UnityEngine;

namespace LEGO.CoreSDK.Unity
{
	// Token: 0x02000037 RID: 55
	public class Log : ILogger
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003B34 File Offset: 0x00001D34
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00003B3C File Offset: 0x00001D3C
		public LogLevel LogLevel
		{
			get
			{
				return this._LogLevel;
			}
			set
			{
				this._LogLevel = value;
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003B48 File Offset: 0x00001D48
		public void Debug(object debugMessage)
		{
			if (this.LogLevel.ShouldLogMessageWithLevel(LogLevel.Debug))
			{
				global::UnityEngine.Debug.Log(debugMessage);
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003B64 File Offset: 0x00001D64
		public void Info(object infoMessage)
		{
			if (this.LogLevel.ShouldLogMessageWithLevel(LogLevel.Info))
			{
				global::UnityEngine.Debug.Log(infoMessage);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003B80 File Offset: 0x00001D80
		public void Warning(object warningMessage)
		{
			if (this.LogLevel.ShouldLogMessageWithLevel(LogLevel.Warning))
			{
				global::UnityEngine.Debug.LogWarning(warningMessage);
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003B9C File Offset: 0x00001D9C
		public void Error(object errorMessage)
		{
			if (this.LogLevel.ShouldLogMessageWithLevel(LogLevel.Error))
			{
				global::UnityEngine.Debug.LogError(errorMessage);
			}
		}

		// Token: 0x0400005D RID: 93
		private LogLevel _LogLevel = LogLevel.Error;
	}
}
