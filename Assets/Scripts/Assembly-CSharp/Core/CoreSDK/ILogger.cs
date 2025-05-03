using System;

namespace LEGO.CoreSDK
{
	// Token: 0x02000038 RID: 56
	public interface ILogger
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600008C RID: 140
		// (set) Token: 0x0600008D RID: 141
		LogLevel LogLevel { get; set; }

		// Token: 0x0600008E RID: 142
		void Debug(object debugMessage);

		// Token: 0x0600008F RID: 143
		void Info(object infoMessage);

		// Token: 0x06000090 RID: 144
		void Warning(object warningMessage);

		// Token: 0x06000091 RID: 145
		void Error(object errorMessage);
	}
}
