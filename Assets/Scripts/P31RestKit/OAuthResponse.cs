using System;
using System.Collections.Generic;

namespace Prime31
{
	// Token: 0x0200000B RID: 11
	public class OAuthResponse
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00003B30 File Offset: 0x00001D30
		public OAuthResponse(string alltext)
		{
			this.responseText = alltext;
			this._params = new Dictionary<string, string>();
			string[] array = alltext.Split(new char[] { '&' });
			foreach (string text in array)
			{
				string[] array3 = text.Split(new char[] { '=' });
				this._params.Add(array3[0], array3[1]);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00003BA7 File Offset: 0x00001DA7
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00003BAF File Offset: 0x00001DAF
		public string responseText { get; set; }

		// Token: 0x17000006 RID: 6
		public string this[string ix]
		{
			get
			{
				return this._params[ix];
			}
		}

		// Token: 0x04000014 RID: 20
		private Dictionary<string, string> _params;
	}
}
