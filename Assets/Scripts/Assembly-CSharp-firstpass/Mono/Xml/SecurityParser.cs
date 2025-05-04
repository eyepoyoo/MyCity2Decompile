using System;
using System.IO;
using System.Security;
using UnityEngine;

namespace Mono.Xml
{
	// Token: 0x02000045 RID: 69
	public class SecurityParser : SmallXmlParser, SmallXmlParser.IContentHandler
	{
		// Token: 0x060003B5 RID: 949 RVA: 0x00015910 File Offset: 0x00013B10
		public SecurityParser()
		{
			this.stack = new AmuzoBetterStack<SecurityElement>();
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00015924 File Offset: 0x00013B24
		public void LoadXml(string xml)
		{
			this.root = null;
			this.stack.Clear();
			base.Parse(new StringReader(xml), this);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00015948 File Offset: 0x00013B48
		public SecurityElement ToXml()
		{
			return this.root;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00015950 File Offset: 0x00013B50
		public override void Release()
		{
			base.Release();
			this.stack.Release();
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00015964 File Offset: 0x00013B64
		public void OnStartParsing(SmallXmlParser parser)
		{
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00015968 File Offset: 0x00013B68
		public void OnProcessingInstruction(string name, string text)
		{
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0001596C File Offset: 0x00013B6C
		public void OnIgnorableWhitespace(string s)
		{
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00015970 File Offset: 0x00013B70
		public void OnStartElement(string name, SmallXmlParser.IAttrList attrs)
		{
			SecurityElement securityElement = new SecurityElement(name);
			if (this.root == null)
			{
				this.root = securityElement;
				this.current = securityElement;
			}
			else
			{
				SecurityElement securityElement2 = this.stack.Peek();
				securityElement2.AddChild(securityElement);
			}
			this.stack.Push(securityElement);
			this.current = securityElement;
			int length = attrs.Length;
			for (int i = 0; i < length; i++)
			{
				this.current.AddAttribute(attrs.GetName(i), attrs.GetValue(i));
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x000159FC File Offset: 0x00013BFC
		public void OnEndElement(string name)
		{
			this.current = this.stack.Pop();
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00015A10 File Offset: 0x00013C10
		public void OnChars(string ch)
		{
			if (this.current == null)
			{
				return;
			}
			try
			{
				this.current.Text = ch;
			}
			catch (Exception)
			{
				Debug.LogWarning("Warning: The following XML is invalid \"" + ch + "\"");
			}
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00015A74 File Offset: 0x00013C74
		public void OnEndParsing(SmallXmlParser parser)
		{
		}

		// Token: 0x04000202 RID: 514
		private SecurityElement root;

		// Token: 0x04000203 RID: 515
		private SecurityElement current;

		// Token: 0x04000204 RID: 516
		private AmuzoBetterStack<SecurityElement> stack;
	}
}
