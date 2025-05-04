using System;
using System.Collections;
using System.Security;

// Token: 0x0200004D RID: 77
public class MonoXmlElement
{
	// Token: 0x06000401 RID: 1025 RVA: 0x00016A1C File Offset: 0x00014C1C
	public MonoXmlElement(SecurityElement node)
	{
		this._node = node;
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00016A2C File Offset: 0x00014C2C
	public MonoXmlElement GetChildAt(string path)
	{
		if (path == null)
		{
			return null;
		}
		string[] array = path.Split(new char[] { '/' });
		int num = 0;
		MonoXmlElement monoXmlElement = new MonoXmlElement(this._node);
		do
		{
			string text = array[num++];
			monoXmlElement = monoXmlElement.GetChild(text);
		}
		while (num < array.Length && monoXmlElement != null);
		return monoXmlElement;
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x00016A84 File Offset: 0x00014C84
	public MonoXmlElement GetChild(string name)
	{
		for (int i = 0; i < this._pCount; i++)
		{
			SecurityElement child = this.GetChild(i);
			if (child.Tag == name)
			{
				return new MonoXmlElement(child);
			}
		}
		return null;
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x00016ACC File Offset: 0x00014CCC
	public MonoXmlElement GetChildAtIndex(int index)
	{
		if (index < 0 || index >= this._pCount)
		{
			return null;
		}
		return new MonoXmlElement(this.GetChild(index));
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x00016AF0 File Offset: 0x00014CF0
	private SecurityElement GetChild(int index)
	{
		if (index < 0 || index >= this._pCount)
		{
			return null;
		}
		return this._node.Children[index] as SecurityElement;
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000406 RID: 1030 RVA: 0x00016B28 File Offset: 0x00014D28
	public int _pCount
	{
		get
		{
			if (this._node == null)
			{
				return 0;
			}
			if (this._node.Children == null)
			{
				return 0;
			}
			return this._node.Children.Count;
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x06000407 RID: 1031 RVA: 0x00016B64 File Offset: 0x00014D64
	public string _pTag
	{
		get
		{
			if (this._node == null)
			{
				return null;
			}
			return this._node.Tag;
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06000408 RID: 1032 RVA: 0x00016B80 File Offset: 0x00014D80
	public string _pText
	{
		get
		{
			if (this._node == null)
			{
				return null;
			}
			return this._node.Text;
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06000409 RID: 1033 RVA: 0x00016B9C File Offset: 0x00014D9C
	public Hashtable _pAttributes
	{
		get
		{
			if (this._node == null)
			{
				return null;
			}
			return this._node.Attributes;
		}
	}

	// Token: 0x04000217 RID: 535
	private SecurityElement _node;
}
