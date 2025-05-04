using System;
using Mono.Xml;

// Token: 0x0200004C RID: 76
public class Xml
{
	// Token: 0x1700007F RID: 127
	// (get) Token: 0x060003FE RID: 1022 RVA: 0x000169D8 File Offset: 0x00014BD8
	private static SecurityParser _pParser
	{
		get
		{
			if (Xml._parser == null)
			{
				Xml._parser = new SecurityParser();
			}
			return Xml._parser;
		}
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x000169F4 File Offset: 0x00014BF4
	public static void Release()
	{
		Xml._pParser.Release();
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x00016A00 File Offset: 0x00014C00
	public static MonoXmlElement Load(string xml)
	{
		Xml._pParser.LoadXml(xml);
		return new MonoXmlElement(Xml._pParser.ToXml());
	}

	// Token: 0x04000216 RID: 534
	private static SecurityParser _parser;
}
