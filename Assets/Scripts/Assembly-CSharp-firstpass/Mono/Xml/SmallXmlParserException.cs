using System;

namespace Mono.Xml
{
	// Token: 0x0200004B RID: 75
	public class SmallXmlParserException : SystemException
	{
		// Token: 0x060003FA RID: 1018 RVA: 0x00016990 File Offset: 0x00014B90
		public SmallXmlParserException(string msg, int line, int column)
			: base(string.Format("{0}. At ({1},{2})", msg, line, column))
		{
			this.line = line;
			this.column = column;
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x000169C0 File Offset: 0x00014BC0
		public int Line
		{
			get
			{
				return this.line;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x000169C8 File Offset: 0x00014BC8
		public int Column
		{
			get
			{
				return this.column;
			}
		}

		// Token: 0x04000214 RID: 532
		private int line;

		// Token: 0x04000215 RID: 533
		private int column;
	}
}
