using System;

namespace LEGO.CoreSDK
{
	// Token: 0x02000054 RID: 84
	public class TextFile
	{
		// Token: 0x06000142 RID: 322 RVA: 0x00007144 File Offset: 0x00005344
		public TextFile(string name, string text)
		{
			this.Name = name;
			this.Text = text;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000143 RID: 323 RVA: 0x0000715C File Offset: 0x0000535C
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00007164 File Offset: 0x00005364
		public string Name { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00007170 File Offset: 0x00005370
		// (set) Token: 0x06000146 RID: 326 RVA: 0x00007178 File Offset: 0x00005378
		public string Text { get; private set; }
	}
}
