using System;

namespace LitJson
{
	// Token: 0x02000023 RID: 35
	public class JsonException : ApplicationException
	{
		// Token: 0x06000257 RID: 599 RVA: 0x0000E860 File Offset: 0x0000CA60
		public JsonException()
		{
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000E868 File Offset: 0x0000CA68
		internal JsonException(ParserToken token)
			: base(string.Format("Invalid token '{0}' in input string", token))
		{
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000E880 File Offset: 0x0000CA80
		internal JsonException(ParserToken token, Exception inner_exception)
			: base(string.Format("Invalid token '{0}' in input string", token), inner_exception)
		{
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000E89C File Offset: 0x0000CA9C
		internal JsonException(int c)
			: base(string.Format("Invalid character '{0}' in input string", (char)c))
		{
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000E8B8 File Offset: 0x0000CAB8
		internal JsonException(int c, Exception inner_exception)
			: base(string.Format("Invalid character '{0}' in input string", (char)c), inner_exception)
		{
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000E8D4 File Offset: 0x0000CAD4
		public JsonException(string message)
			: base(message)
		{
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000E8E0 File Offset: 0x0000CAE0
		public JsonException(string message, Exception inner_exception)
			: base(message, inner_exception)
		{
		}
	}
}
