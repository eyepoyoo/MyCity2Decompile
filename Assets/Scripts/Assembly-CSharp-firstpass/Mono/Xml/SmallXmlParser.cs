using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Mono.Xml
{
	// Token: 0x02000047 RID: 71
	public class SmallXmlParser
	{
		// Token: 0x060003C9 RID: 969 RVA: 0x00015AF4 File Offset: 0x00013CF4
		public virtual void Release()
		{
			this.attributes.Release();
			this.elementNames.Release();
			this.xmlSpaces.Release();
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00015B18 File Offset: 0x00013D18
		private Exception Error(string msg)
		{
			return new SmallXmlParserException(msg, this.line, this.column);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00015B2C File Offset: 0x00013D2C
		private Exception UnexpectedEndError()
		{
			return this.Error(string.Format("Unexpected end of stream. Element stack content is {0}", string.Join(",", this.elementNames.ToArray())));
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00015B54 File Offset: 0x00013D54
		private bool IsNameChar(char c, bool start)
		{
			if (c == '-' || c == '.')
			{
				return !start;
			}
			if (c == ':' || c == '_')
			{
				return true;
			}
			if (c > 'Ā')
			{
				if (c == 'ۥ' || c == 'ۦ' || c == 'ՙ')
				{
					return true;
				}
				if ('ʻ' <= c && c <= 'ˁ')
				{
					return true;
				}
			}
			switch (char.GetUnicodeCategory(c))
			{
			case UnicodeCategory.UppercaseLetter:
			case UnicodeCategory.LowercaseLetter:
			case UnicodeCategory.TitlecaseLetter:
			case UnicodeCategory.OtherLetter:
			case UnicodeCategory.LetterNumber:
				return true;
			case UnicodeCategory.ModifierLetter:
			case UnicodeCategory.NonSpacingMark:
			case UnicodeCategory.SpacingCombiningMark:
			case UnicodeCategory.EnclosingMark:
			case UnicodeCategory.DecimalDigitNumber:
				return !start;
			default:
				return false;
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00015C20 File Offset: 0x00013E20
		private bool IsWhitespace(int c)
		{
			switch (c)
			{
			case 9:
			case 10:
			case 13:
				break;
			default:
				if (c != 32)
				{
					return false;
				}
				break;
			}
			return true;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00015C5C File Offset: 0x00013E5C
		public void SkipWhitespaces()
		{
			this.SkipWhitespaces(false);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00015C68 File Offset: 0x00013E68
		private void HandleWhitespaces()
		{
			while (this.IsWhitespace(this.Peek()))
			{
				this.buffer.Append((char)this.Read());
			}
			if (this.Peek() != 60 && this.Peek() >= 0)
			{
				this.isWhitespace = false;
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00015CC0 File Offset: 0x00013EC0
		public void SkipWhitespaces(bool expected)
		{
			for (;;)
			{
				int num = this.Peek();
				switch (num)
				{
				case 9:
				case 10:
				case 13:
					break;
				default:
					if (num != 32)
					{
						goto Block_0;
					}
					break;
				}
				this.Read();
				if (expected)
				{
					expected = false;
				}
			}
			Block_0:
			if (expected)
			{
				throw this.Error("Whitespace is expected.");
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00015D2C File Offset: 0x00013F2C
		private int Peek()
		{
			return this.reader.Peek();
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00015D3C File Offset: 0x00013F3C
		private int Read()
		{
			int num = this.reader.Read();
			if (num == 10)
			{
				this.resetColumn = true;
			}
			if (this.resetColumn)
			{
				this.line++;
				this.resetColumn = false;
				this.column = 1;
			}
			else
			{
				this.column++;
			}
			return num;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00015DA0 File Offset: 0x00013FA0
		public void Expect(int c)
		{
			int num = this.Read();
			if (num < 0)
			{
				throw this.UnexpectedEndError();
			}
			if (num != c)
			{
				throw this.Error(string.Format("Expected '{0}' but got {1}", (char)c, (char)num));
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00015DE8 File Offset: 0x00013FE8
		private string ReadUntil(char until, bool handleReferences)
		{
			while (this.Peek() >= 0)
			{
				char c = (char)this.Read();
				if (c == until)
				{
					string text = this.buffer.ToString();
					this.buffer.Length = 0;
					return text;
				}
				if (handleReferences && c == '&')
				{
					this.ReadReference();
				}
				else
				{
					this.buffer.Append(c);
				}
			}
			throw this.UnexpectedEndError();
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00015E60 File Offset: 0x00014060
		public string ReadName()
		{
			int num = 0;
			if (this.Peek() < 0 || !this.IsNameChar((char)this.Peek(), true))
			{
				throw this.Error("XML name start character is expected.");
			}
			for (int i = this.Peek(); i >= 0; i = this.Peek())
			{
				char c = (char)i;
				if (!this.IsNameChar(c, false))
				{
					break;
				}
				if (num == this.nameBuffer.Length)
				{
					char[] array = new char[num * 2];
					Array.Copy(this.nameBuffer, 0, array, 0, num);
					this.nameBuffer = array;
				}
				this.nameBuffer[num++] = c;
				this.Read();
			}
			if (num == 0)
			{
				throw this.Error("Valid XML name is expected.");
			}
			return new string(this.nameBuffer, 0, num);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00015F2C File Offset: 0x0001412C
		public void Parse(TextReader input, SmallXmlParser.IContentHandler handler)
		{
			this.reader = input;
			this.handler = handler;
			handler.OnStartParsing(this);
			while (this.Peek() >= 0)
			{
				this.ReadContent();
			}
			this.HandleBufferedContent();
			if (this.elementNames.Count > 0)
			{
				throw this.Error(string.Format("Insufficient close tag: {0}", this.elementNames.Peek()));
			}
			handler.OnEndParsing(this);
			this.Cleanup();
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00015FA8 File Offset: 0x000141A8
		private void Cleanup()
		{
			this.line = 1;
			this.column = 0;
			this.handler = null;
			this.reader.Dispose();
			this.reader = null;
			this.elementNames.Clear();
			this.xmlSpaces.Clear();
			this.attributes.Clear();
			this.buffer.Length = 0;
			this.xmlSpace = null;
			this.isWhitespace = false;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00016018 File Offset: 0x00014218
		public void ReadContent()
		{
			if (this.IsWhitespace(this.Peek()))
			{
				if (this.buffer.Length == 0)
				{
					this.isWhitespace = true;
				}
				this.HandleWhitespaces();
			}
			if (this.Peek() != 60)
			{
				this.ReadCharacters();
				return;
			}
			this.Read();
			int num = this.Peek();
			if (num != 33)
			{
				if (num != 47)
				{
					string text;
					if (num != 63)
					{
						this.HandleBufferedContent();
						text = this.ReadName();
						while (this.Peek() != 62 && this.Peek() != 47)
						{
							this.ReadAttribute(this.attributes);
						}
						this.handler.OnStartElement(text, this.attributes);
						this.attributes.Clear();
						this.SkipWhitespaces();
						if (this.Peek() == 47)
						{
							this.Read();
							this.handler.OnEndElement(text);
						}
						else
						{
							this.elementNames.Push(text);
							this.xmlSpaces.Push(this.xmlSpace);
						}
						this.Expect(62);
						return;
					}
					this.HandleBufferedContent();
					this.Read();
					text = this.ReadName();
					this.SkipWhitespaces();
					string text2 = string.Empty;
					if (this.Peek() != 63)
					{
						for (;;)
						{
							text2 += this.ReadUntil('?', false);
							if (this.Peek() == 62)
							{
								break;
							}
							text2 += "?";
						}
					}
					this.handler.OnProcessingInstruction(text, text2);
					this.Expect(62);
					return;
				}
				else
				{
					this.HandleBufferedContent();
					if (this.elementNames.Count == 0)
					{
						throw this.UnexpectedEndError();
					}
					this.Read();
					string text = this.ReadName();
					this.SkipWhitespaces();
					string text3 = this.elementNames.Pop();
					this.xmlSpaces.Pop();
					if (this.xmlSpaces.Count > 0)
					{
						this.xmlSpace = this.xmlSpaces.Peek();
					}
					else
					{
						this.xmlSpace = null;
					}
					if (text != text3)
					{
						throw this.Error(string.Format("End tag mismatch: expected {0} but found {1}", text3, text));
					}
					this.handler.OnEndElement(text);
					this.Expect(62);
					return;
				}
			}
			else
			{
				this.Read();
				if (this.Peek() == 91)
				{
					this.Read();
					if (this.ReadName() != "CDATA")
					{
						throw this.Error("Invalid declaration markup");
					}
					this.Expect(91);
					this.ReadCDATASection();
					return;
				}
				else
				{
					if (this.Peek() == 45)
					{
						this.ReadComment();
						return;
					}
					if (this.ReadName() == "DOCTYPE")
					{
						this.ReadDOCTYPESection();
						return;
					}
					throw this.Error("Invalid declaration markup.");
				}
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x000162E0 File Offset: 0x000144E0
		private void HandleBufferedContent()
		{
			if (this.buffer.Length == 0)
			{
				return;
			}
			if (this.isWhitespace)
			{
				this.handler.OnIgnorableWhitespace(this.buffer.ToString());
			}
			else
			{
				this.handler.OnChars(this.buffer.ToString());
			}
			this.buffer.Length = 0;
			this.isWhitespace = false;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00016350 File Offset: 0x00014550
		private void ReadCharacters()
		{
			this.isWhitespace = false;
			for (;;)
			{
				int num = this.Peek();
				int num2 = num;
				if (num2 == -1)
				{
					break;
				}
				if (num2 != 38)
				{
					if (num2 == 60)
					{
						return;
					}
					this.buffer.Append((char)this.Read());
				}
				else
				{
					this.Read();
					this.ReadReference();
				}
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000163BC File Offset: 0x000145BC
		private void ReadReference()
		{
			if (this.Peek() != 35)
			{
				string text = this.ReadName();
				this.Expect(59);
				string text2 = text;
				switch (text2)
				{
				case "amp":
					this.buffer.Append('&');
					return;
				case "quot":
					this.buffer.Append('"');
					return;
				case "apos":
					this.buffer.Append('\'');
					return;
				case "lt":
					this.buffer.Append('<');
					return;
				case "gt":
					this.buffer.Append('>');
					return;
				case "copy":
					this.buffer.Append('©');
					return;
				case "trade":
					this.buffer.Append('™');
					return;
				case "reg":
					this.buffer.Append('®');
					return;
				}
				throw this.Error("General non-predefined entity reference is not supported in this parser.");
			}
			this.Read();
			this.ReadCharacterReference();
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00016564 File Offset: 0x00014764
		private int ReadCharacterReference()
		{
			int num = 0;
			if (this.Peek() == 120)
			{
				this.Read();
				for (int i = this.Peek(); i >= 0; i = this.Peek())
				{
					if (48 <= i && i <= 57)
					{
						num <<= 4 + i - 48;
					}
					else if (65 <= i && i <= 70)
					{
						num <<= 4 + i - 65 + 10;
					}
					else
					{
						if (97 > i || i > 102)
						{
							break;
						}
						num <<= 4 + i - 97 + 10;
					}
					this.Read();
				}
			}
			else
			{
				for (int j = this.Peek(); j >= 0; j = this.Peek())
				{
					if (48 > j || j > 57)
					{
						break;
					}
					num <<= 4 + j - 48;
					this.Read();
				}
			}
			return num;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00016664 File Offset: 0x00014864
		private void ReadAttribute(SmallXmlParser.AttrListImpl a)
		{
			this.SkipWhitespaces(true);
			if (this.Peek() == 47 || this.Peek() == 62)
			{
				return;
			}
			string text = this.ReadName();
			this.SkipWhitespaces();
			this.Expect(61);
			this.SkipWhitespaces();
			int num = this.Read();
			string text2;
			if (num != 34)
			{
				if (num != 39)
				{
					throw this.Error("Invalid attribute value markup.");
				}
				text2 = this.ReadUntil('\'', true);
			}
			else
			{
				text2 = this.ReadUntil('"', true);
			}
			if (text == "xml:space")
			{
				this.xmlSpace = text2;
			}
			a.Add(text, text2);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00016714 File Offset: 0x00014914
		private void ReadCDATASection()
		{
			int num = 0;
			while (this.Peek() >= 0)
			{
				char c = (char)this.Read();
				if (c == ']')
				{
					num++;
				}
				else
				{
					if (c == '>' && num > 1)
					{
						for (int i = num; i > 2; i--)
						{
							this.buffer.Append(']');
						}
						return;
					}
					for (int j = 0; j < num; j++)
					{
						this.buffer.Append(']');
					}
					num = 0;
					this.buffer.Append(c);
				}
			}
			throw this.UnexpectedEndError();
		}

		// Token: 0x060003DF RID: 991 RVA: 0x000167B8 File Offset: 0x000149B8
		private void ReadDOCTYPESection()
		{
			int num = 0;
			while (this.Peek() >= 0)
			{
				char c = (char)this.Read();
				if (c == ']')
				{
					num++;
				}
				else
				{
					if (c == '>' && num > 0)
					{
						return;
					}
					num = 0;
				}
			}
			throw this.UnexpectedEndError();
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00016810 File Offset: 0x00014A10
		private void ReadComment()
		{
			this.Expect(45);
			this.Expect(45);
			for (;;)
			{
				if (this.Read() == 45)
				{
					if (this.Read() == 45)
					{
						break;
					}
				}
			}
			if (this.Read() != 62)
			{
				throw this.Error("'--' is not allowed inside comment markup.");
			}
		}

		// Token: 0x04000205 RID: 517
		private SmallXmlParser.IContentHandler handler;

		// Token: 0x04000206 RID: 518
		private TextReader reader;

		// Token: 0x04000207 RID: 519
		private AmuzoBetterStack<string> elementNames = new AmuzoBetterStack<string>();

		// Token: 0x04000208 RID: 520
		private AmuzoBetterStack<string> xmlSpaces = new AmuzoBetterStack<string>();

		// Token: 0x04000209 RID: 521
		private string xmlSpace;

		// Token: 0x0400020A RID: 522
		private StringBuilder buffer = new StringBuilder(200);

		// Token: 0x0400020B RID: 523
		private char[] nameBuffer = new char[30];

		// Token: 0x0400020C RID: 524
		private bool isWhitespace;

		// Token: 0x0400020D RID: 525
		private SmallXmlParser.AttrListImpl attributes = new SmallXmlParser.AttrListImpl();

		// Token: 0x0400020E RID: 526
		private int line = 1;

		// Token: 0x0400020F RID: 527
		private int column;

		// Token: 0x04000210 RID: 528
		private bool resetColumn;

		// Token: 0x02000048 RID: 72
		public interface IContentHandler
		{
			// Token: 0x060003E1 RID: 993
			void OnStartParsing(SmallXmlParser parser);

			// Token: 0x060003E2 RID: 994
			void OnEndParsing(SmallXmlParser parser);

			// Token: 0x060003E3 RID: 995
			void OnStartElement(string name, SmallXmlParser.IAttrList attrs);

			// Token: 0x060003E4 RID: 996
			void OnEndElement(string name);

			// Token: 0x060003E5 RID: 997
			void OnProcessingInstruction(string name, string text);

			// Token: 0x060003E6 RID: 998
			void OnChars(string text);

			// Token: 0x060003E7 RID: 999
			void OnIgnorableWhitespace(string text);
		}

		// Token: 0x02000049 RID: 73
		public interface IAttrList
		{
			// Token: 0x17000075 RID: 117
			// (get) Token: 0x060003E8 RID: 1000
			int Length { get; }

			// Token: 0x17000076 RID: 118
			// (get) Token: 0x060003E9 RID: 1001
			bool IsEmpty { get; }

			// Token: 0x060003EA RID: 1002
			string GetName(int i);

			// Token: 0x060003EB RID: 1003
			string GetValue(int i);

			// Token: 0x060003EC RID: 1004
			string GetValue(string name);

			// Token: 0x17000077 RID: 119
			// (get) Token: 0x060003ED RID: 1005
			string[] Names { get; }

			// Token: 0x17000078 RID: 120
			// (get) Token: 0x060003EE RID: 1006
			string[] Values { get; }
		}

		// Token: 0x0200004A RID: 74
		private class AttrListImpl : SmallXmlParser.IAttrList
		{
			// Token: 0x17000079 RID: 121
			// (get) Token: 0x060003F0 RID: 1008 RVA: 0x00016894 File Offset: 0x00014A94
			public int Length
			{
				get
				{
					return this.attrNames.Count;
				}
			}

			// Token: 0x1700007A RID: 122
			// (get) Token: 0x060003F1 RID: 1009 RVA: 0x000168A4 File Offset: 0x00014AA4
			public bool IsEmpty
			{
				get
				{
					return this.attrNames.Count == 0;
				}
			}

			// Token: 0x060003F2 RID: 1010 RVA: 0x000168B4 File Offset: 0x00014AB4
			public string GetName(int i)
			{
				return this.attrNames[i];
			}

			// Token: 0x060003F3 RID: 1011 RVA: 0x000168C4 File Offset: 0x00014AC4
			public string GetValue(int i)
			{
				return this.attrValues[i];
			}

			// Token: 0x060003F4 RID: 1012 RVA: 0x000168D4 File Offset: 0x00014AD4
			public string GetValue(string name)
			{
				for (int i = 0; i < this.attrNames.Count; i++)
				{
					if (this.attrNames[i] == name)
					{
						return this.attrValues[i];
					}
				}
				return null;
			}

			// Token: 0x1700007B RID: 123
			// (get) Token: 0x060003F5 RID: 1013 RVA: 0x00016924 File Offset: 0x00014B24
			public string[] Names
			{
				get
				{
					return this.attrNames.ToArray();
				}
			}

			// Token: 0x1700007C RID: 124
			// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00016934 File Offset: 0x00014B34
			public string[] Values
			{
				get
				{
					return this.attrValues.ToArray();
				}
			}

			// Token: 0x060003F7 RID: 1015 RVA: 0x00016944 File Offset: 0x00014B44
			internal void Clear()
			{
				this.attrNames.Clear();
				this.attrValues.Clear();
			}

			// Token: 0x060003F8 RID: 1016 RVA: 0x0001695C File Offset: 0x00014B5C
			internal void Add(string name, string value)
			{
				this.attrNames.Add(name);
				this.attrValues.Add(value);
			}

			// Token: 0x060003F9 RID: 1017 RVA: 0x00016978 File Offset: 0x00014B78
			public void Release()
			{
				this.attrNames.Release();
				this.attrValues.Release();
			}

			// Token: 0x04000212 RID: 530
			private AmuzoBetterList<string> attrNames = new AmuzoBetterList<string>();

			// Token: 0x04000213 RID: 531
			private AmuzoBetterList<string> attrValues = new AmuzoBetterList<string>();
		}
	}
}
