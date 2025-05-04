using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace Mono.Xml
{
	// Token: 0x0200003B RID: 59
	public class MiniParser
	{
		// Token: 0x0600037F RID: 895 RVA: 0x00014C10 File Offset: 0x00012E10
		public MiniParser()
		{
			this.twoCharBuff = new int[2];
			this.splitCData = false;
			this.Reset();
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00014CB0 File Offset: 0x00012EB0
		public void Reset()
		{
			this.line = 0;
			this.col = 0;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00014CC0 File Offset: 0x00012EC0
		protected static bool StrEquals(string str, StringBuilder sb, int sbStart, int len)
		{
			if (len != str.Length)
			{
				return false;
			}
			for (int i = 0; i < len; i++)
			{
				if (str[i] != sb[sbStart + i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00014D08 File Offset: 0x00012F08
		protected void FatalErr(string descr)
		{
			throw new MiniParser.XMLError(descr, this.line, this.col);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00014D1C File Offset: 0x00012F1C
		protected static int Xlat(int charCode, int state)
		{
			int num = state * MiniParser.INPUT_RANGE;
			int num2 = System.Math.Min(MiniParser.tbl.Length - num, MiniParser.INPUT_RANGE);
			while (--num2 >= 0)
			{
				ushort num3 = MiniParser.tbl[num];
				if (charCode == num3 >> 12)
				{
					return (int)(num3 & 4095);
				}
				num++;
			}
			return 4095;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00014D78 File Offset: 0x00012F78
		public void Parse(MiniParser.IReader reader, MiniParser.IHandler handler)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (handler == null)
			{
				handler = new MiniParser.HandlerAdapter();
			}
			MiniParser.AttrListImpl attrListImpl = new MiniParser.AttrListImpl();
			string text = null;
			Stack stack = new Stack();
			string text2 = null;
			this.line = 1;
			this.col = 0;
			int num = 0;
			int num2 = 0;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			int num3 = 0;
			handler.OnStartParsing(this);
			for (;;)
			{
				this.col++;
				num = reader.Read();
				if (num == -1)
				{
					break;
				}
				int num4 = "<>/?=&'\"![ ]\t\r\n".IndexOf((char)num) & 15;
				if (num4 != 13)
				{
					if (num4 == 12)
					{
						num4 = 10;
					}
					if (num4 == 14)
					{
						this.col = 0;
						this.line++;
						num4 = 10;
					}
					int num5 = MiniParser.Xlat(num4, num2);
					num2 = num5 & 255;
					if (num != 10 || (num2 != 14 && num2 != 15))
					{
						num5 >>= 8;
						if (num2 >= 128)
						{
							if (num2 == 255)
							{
								this.FatalErr("State dispatch error.");
							}
							else
							{
								this.FatalErr(MiniParser.errors[num2 ^ 128]);
							}
						}
						switch (num5)
						{
						case 0:
							goto IL_019F;
						case 1:
						{
							text2 = stringBuilder.ToString();
							stringBuilder = new StringBuilder();
							string text3 = null;
							if (stack.Count == 0 || text2 != (text3 = stack.Pop() as string))
							{
								if (text3 == null)
								{
									this.FatalErr("Tag stack underflow");
								}
								else
								{
									this.FatalErr(string.Format("Expected end tag '{0}' but found '{1}'", text2, text3));
								}
							}
							handler.OnEndElement(text2);
							break;
						}
						case 2:
							text2 = stringBuilder.ToString();
							stringBuilder = new StringBuilder();
							if (num == 47 || num == 62)
							{
								goto IL_019F;
							}
							break;
						case 3:
							text = stringBuilder.ToString();
							stringBuilder = new StringBuilder();
							break;
						case 4:
							if (text == null)
							{
								this.FatalErr("Internal error.");
							}
							attrListImpl.Add(text, stringBuilder.ToString());
							stringBuilder = new StringBuilder();
							text = null;
							break;
						case 5:
							handler.OnChars(stringBuilder.ToString());
							stringBuilder = new StringBuilder();
							break;
						case 6:
						{
							string text4 = "CDATA[";
							flag2 = false;
							flag3 = false;
							if (num == 45)
							{
								num = reader.Read();
								if (num != 45)
								{
									this.FatalErr("Invalid comment");
								}
								this.col++;
								flag2 = true;
								this.twoCharBuff[0] = -1;
								this.twoCharBuff[1] = -1;
							}
							else if (num != 91)
							{
								flag3 = true;
								num3 = 0;
							}
							else
							{
								for (int i = 0; i < text4.Length; i++)
								{
									if (reader.Read() != (int)text4[i])
									{
										this.col += i + 1;
										break;
									}
								}
								this.col += text4.Length;
								flag = true;
							}
							break;
						}
						case 7:
						{
							int num6 = 0;
							num = 93;
							while (num == 93)
							{
								num = reader.Read();
								num6++;
							}
							if (num != 62)
							{
								for (int j = 0; j < num6; j++)
								{
									stringBuilder.Append(']');
								}
								stringBuilder.Append((char)num);
								num2 = 18;
							}
							else
							{
								for (int k = 0; k < num6 - 2; k++)
								{
									stringBuilder.Append(']');
								}
								flag = false;
							}
							this.col += num6;
							break;
						}
						case 8:
							this.FatalErr(string.Format("Error {0}", num2));
							break;
						case 9:
							break;
						case 10:
							stringBuilder = new StringBuilder();
							if (num != 60)
							{
								goto IL_0465;
							}
							break;
						case 11:
							goto IL_0465;
						case 12:
							if (flag2)
							{
								if (num == 62 && this.twoCharBuff[0] == 45 && this.twoCharBuff[1] == 45)
								{
									flag2 = false;
									num2 = 0;
								}
								else
								{
									this.twoCharBuff[0] = this.twoCharBuff[1];
									this.twoCharBuff[1] = num;
								}
							}
							else if (flag3)
							{
								if (num == 60 || num == 62)
								{
									num3 ^= 1;
								}
								if (num == 62 && num3 != 0)
								{
									flag3 = false;
									num2 = 0;
								}
							}
							else
							{
								if (this.splitCData && stringBuilder.Length > 0 && flag)
								{
									handler.OnChars(stringBuilder.ToString());
									stringBuilder = new StringBuilder();
								}
								flag = false;
								stringBuilder.Append((char)num);
							}
							break;
						case 13:
						{
							num = reader.Read();
							int num7 = this.col + 1;
							if (num == 35)
							{
								int num8 = 10;
								int num9 = 0;
								int num10 = 0;
								num = reader.Read();
								num7++;
								if (num == 120)
								{
									num = reader.Read();
									num7++;
									num8 = 16;
								}
								NumberStyles numberStyles = ((num8 != 16) ? NumberStyles.Integer : NumberStyles.HexNumber);
								for (;;)
								{
									int num11 = -1;
									if (char.IsNumber((char)num))
									{
										goto Block_43; //?
									}
									if ("abcdef".IndexOf(char.ToLower((char)num)) != -1)
									{
										goto Block_43;
									}
									IL_05F9:
									if (num11 == -1)
									{
										break;
									}
									num9 *= num8;
									num9 += num11;
									num10++;
									num = reader.Read();
									num7++;
									continue;
									Block_43:
									try
									{
                                            MEOW:
                                                num11 = int.Parse(new string((char)num, 1), numberStyles);
									}
									catch (FormatException)
									{
										num11 = -1;
									}
									goto IL_05F9;
								}
								if (num == 59 && num10 > 0)
								{
									stringBuilder.Append((char)num9);
								}
								else
								{
									this.FatalErr("Bad char ref");
								}
							}
							else
							{
								string text5 = "aglmopqstu";
								string text6 = "&'\"><";
								int num12 = 0;
								int num13 = 15;
								int num14 = 0;
								int length = stringBuilder.Length;
								for (;;)
								{
									if (num12 != 15)
									{
										num12 = text5.IndexOf((char)num) & 15;
									}
									if (num12 == 15)
									{
										this.FatalErr(MiniParser.errors[7]);
									}
									stringBuilder.Append((char)num);
									int num15 = (int)"Ｕ㾏侏ཟｸ\ue1f4⊙\ueeff\ueeffｏ"[num12];
									int num16 = (num15 >> 4) & 15;
									int num17 = num15 & 15;
									int num18 = num15 >> 12;
									int num19 = (num15 >> 8) & 15;
									num = reader.Read();
									num7++;
									num12 = 15;
									if (num16 != 15 && num == (int)text5[num16])
									{
										if (num18 < 14)
										{
											num13 = num18;
										}
										num14 = 12;
									}
									else if (num17 != 15 && num == (int)text5[num17])
									{
										if (num19 < 14)
										{
											num13 = num19;
										}
										num14 = 8;
									}
									else if (num == 59)
									{
										if (num13 != 15 && num14 != 0 && ((num15 >> num14) & 15) == 14)
										{
											break;
										}
										continue;
									}
									num12 = 0;
								}
								int num20 = num7 - this.col - 1;
								if (num20 > 0 && num20 < 5 && (MiniParser.StrEquals("amp", stringBuilder, length, num20) || MiniParser.StrEquals("apos", stringBuilder, length, num20) || MiniParser.StrEquals("quot", stringBuilder, length, num20) || MiniParser.StrEquals("lt", stringBuilder, length, num20) || MiniParser.StrEquals("gt", stringBuilder, length, num20)))
								{
									stringBuilder.Length = length;
									stringBuilder.Append(text6[num13]);
								}
								else
								{
									this.FatalErr(MiniParser.errors[7]);
								}
							}
							this.col = num7;
							break;
						}
						default:
							this.FatalErr(string.Format("Unexpected action code - {0}.", num5));
							break;
						}
						continue;
						IL_019F:
						handler.OnStartElement(text2, attrListImpl);
						if (num != 47)
						{
							stack.Push(text2);
						}
						else
						{
							handler.OnEndElement(text2);
						}
						attrListImpl.Clear();
						continue;
						IL_0465:
						stringBuilder.Append((char)num);
					}
				}
			}
			if (num2 != 0)
			{
				this.FatalErr("Unexpected EOF");
			}
			handler.OnEndParsing(this);
		}

		// Token: 0x040001D4 RID: 468
		private static readonly int INPUT_RANGE = 13;

		// Token: 0x040001D5 RID: 469
		private static readonly ushort[] tbl = new ushort[]
		{
			2305, 43264, 63616, 10368, 6272, 14464, 18560, 22656, 26752, 34944,
			39040, 47232, 30848, 2177, 10498, 6277, 14595, 18561, 22657, 26753,
			35088, 39041, 43137, 47233, 30849, 64004, 4352, 43266, 64258, 2177,
			10369, 14465, 18561, 22657, 26753, 34945, 39041, 47233, 30849, 14597,
			2307, 10499, 6403, 18691, 22787, 26883, 35075, 39171, 43267, 47363,
			30979, 63747, 64260, 8710, 4615, 41480, 2177, 14465, 18561, 22657,
			26753, 34945, 39041, 47233, 30849, 6400, 2307, 10499, 14595, 18691,
			22787, 26883, 35075, 39171, 43267, 47363, 30979, 63747, 6400, 2177,
			10369, 14465, 18561, 22657, 26753, 34945, 39041, 43137, 47233, 30849,
			63617, 2561, 23818, 11274, 7178, 15370, 19466, 27658, 35850, 39946,
			43783, 48138, 31754, 64522, 64265, 8198, 4103, 43272, 2177, 14465,
			18561, 22657, 26753, 34945, 39041, 47233, 30849, 64265, 17163, 43276,
			2178, 10370, 6274, 14466, 22658, 26754, 34946, 39042, 47234, 30850,
			2317, 23818, 11274, 7178, 15370, 19466, 27658, 35850, 39946, 44042,
			48138, 31754, 64522, 26894, 30991, 43275, 2180, 10372, 6276, 14468,
			18564, 22660, 34948, 39044, 47236, 63620, 17163, 43276, 2178, 10370,
			6274, 14466, 22658, 26754, 34946, 39042, 47234, 30850, 63618, 9474,
			35088, 2182, 6278, 14470, 18566, 22662, 26758, 39046, 43142, 47238,
			30854, 63622, 25617, 23822, 2830, 11022, 6926, 15118, 19214, 35598,
			39694, 43790, 47886, 31502, 64270, 29713, 23823, 2831, 11023, 6927,
			15119, 19215, 27407, 35599, 39695, 43791, 47887, 64271, 38418, 6400,
			1555, 9747, 13843, 17939, 22035, 26131, 34323, 42515, 46611, 30227,
			62995, 8198, 4103, 43281, 64265, 2177, 14465, 18561, 22657, 26753,
			34945, 39041, 47233, 30849, 46858, 3090, 11282, 7186, 15378, 19474,
			23570, 27666, 35858, 39954, 44050, 31762, 64530, 3091, 11283, 7187,
			15379, 19475, 23571, 27667, 35859, 39955, 44051, 48147, 31763, 64531,
			ushort.MaxValue, ushort.MaxValue
		};

		// Token: 0x040001D6 RID: 470
		protected static string[] errors = new string[] { "Expected element", "Invalid character in tag", "No '='", "Invalid character entity", "Invalid attr value", "Empty tag", "No end tag", "Bad entity ref" };

		// Token: 0x040001D7 RID: 471
		protected int line;

		// Token: 0x040001D8 RID: 472
		protected int col;

		// Token: 0x040001D9 RID: 473
		protected int[] twoCharBuff;

		// Token: 0x040001DA RID: 474
		protected bool splitCData;

		// Token: 0x0200003C RID: 60
		public interface IReader
		{
			// Token: 0x06000386 RID: 902
			int Read();
		}

		// Token: 0x0200003D RID: 61
		public interface IAttrList
		{
			// Token: 0x1700006B RID: 107
			// (get) Token: 0x06000387 RID: 903
			int Length { get; }

			// Token: 0x1700006C RID: 108
			// (get) Token: 0x06000388 RID: 904
			bool IsEmpty { get; }

			// Token: 0x06000389 RID: 905
			string GetName(int i);

			// Token: 0x0600038A RID: 906
			string GetValue(int i);

			// Token: 0x0600038B RID: 907
			string GetValue(string name);

			// Token: 0x0600038C RID: 908
			void ChangeValue(string name, string newValue);

			// Token: 0x1700006D RID: 109
			// (get) Token: 0x0600038D RID: 909
			string[] Names { get; }

			// Token: 0x1700006E RID: 110
			// (get) Token: 0x0600038E RID: 910
			string[] Values { get; }
		}

		// Token: 0x0200003E RID: 62
		public interface IMutableAttrList : MiniParser.IAttrList
		{
			// Token: 0x0600038F RID: 911
			void Clear();

			// Token: 0x06000390 RID: 912
			void Add(string name, string value);

			// Token: 0x06000391 RID: 913
			void CopyFrom(MiniParser.IAttrList attrs);

			// Token: 0x06000392 RID: 914
			void Remove(int i);

			// Token: 0x06000393 RID: 915
			void Remove(string name);
		}

		// Token: 0x0200003F RID: 63
		public interface IHandler
		{
			// Token: 0x06000394 RID: 916
			void OnStartParsing(MiniParser parser);

			// Token: 0x06000395 RID: 917
			void OnStartElement(string name, MiniParser.IAttrList attrs);

			// Token: 0x06000396 RID: 918
			void OnEndElement(string name);

			// Token: 0x06000397 RID: 919
			void OnChars(string ch);

			// Token: 0x06000398 RID: 920
			void OnEndParsing(MiniParser parser);
		}

		// Token: 0x02000040 RID: 64
		public class HandlerAdapter : MiniParser.IHandler
		{
			// Token: 0x0600039A RID: 922 RVA: 0x00015624 File Offset: 0x00013824
			public void OnStartParsing(MiniParser parser)
			{
			}

			// Token: 0x0600039B RID: 923 RVA: 0x00015628 File Offset: 0x00013828
			public void OnStartElement(string name, MiniParser.IAttrList attrs)
			{
			}

			// Token: 0x0600039C RID: 924 RVA: 0x0001562C File Offset: 0x0001382C
			public void OnEndElement(string name)
			{
			}

			// Token: 0x0600039D RID: 925 RVA: 0x00015630 File Offset: 0x00013830
			public void OnChars(string ch)
			{
			}

			// Token: 0x0600039E RID: 926 RVA: 0x00015634 File Offset: 0x00013834
			public void OnEndParsing(MiniParser parser)
			{
			}
		}

		// Token: 0x02000041 RID: 65
		private enum CharKind : byte
		{
			// Token: 0x040001DC RID: 476
			LEFT_BR,
			// Token: 0x040001DD RID: 477
			RIGHT_BR,
			// Token: 0x040001DE RID: 478
			SLASH,
			// Token: 0x040001DF RID: 479
			PI_MARK,
			// Token: 0x040001E0 RID: 480
			EQ,
			// Token: 0x040001E1 RID: 481
			AMP,
			// Token: 0x040001E2 RID: 482
			SQUOTE,
			// Token: 0x040001E3 RID: 483
			DQUOTE,
			// Token: 0x040001E4 RID: 484
			BANG,
			// Token: 0x040001E5 RID: 485
			LEFT_SQBR,
			// Token: 0x040001E6 RID: 486
			SPACE,
			// Token: 0x040001E7 RID: 487
			RIGHT_SQBR,
			// Token: 0x040001E8 RID: 488
			TAB,
			// Token: 0x040001E9 RID: 489
			CR,
			// Token: 0x040001EA RID: 490
			EOL,
			// Token: 0x040001EB RID: 491
			CHARS,
			// Token: 0x040001EC RID: 492
			UNKNOWN = 31
		}

		// Token: 0x02000042 RID: 66
		private enum ActionCode : byte
		{
			// Token: 0x040001EE RID: 494
			START_ELEM,
			// Token: 0x040001EF RID: 495
			END_ELEM,
			// Token: 0x040001F0 RID: 496
			END_NAME,
			// Token: 0x040001F1 RID: 497
			SET_ATTR_NAME,
			// Token: 0x040001F2 RID: 498
			SET_ATTR_VAL,
			// Token: 0x040001F3 RID: 499
			SEND_CHARS,
			// Token: 0x040001F4 RID: 500
			START_CDATA,
			// Token: 0x040001F5 RID: 501
			END_CDATA,
			// Token: 0x040001F6 RID: 502
			ERROR,
			// Token: 0x040001F7 RID: 503
			STATE_CHANGE,
			// Token: 0x040001F8 RID: 504
			FLUSH_CHARS_STATE_CHANGE,
			// Token: 0x040001F9 RID: 505
			ACC_CHARS_STATE_CHANGE,
			// Token: 0x040001FA RID: 506
			ACC_CDATA,
			// Token: 0x040001FB RID: 507
			PROC_CHAR_REF,
			// Token: 0x040001FC RID: 508
			UNKNOWN = 15
		}

		// Token: 0x02000043 RID: 67
		public class AttrListImpl : MiniParser.IAttrList, MiniParser.IMutableAttrList
		{
			// Token: 0x0600039F RID: 927 RVA: 0x00015638 File Offset: 0x00013838
			public AttrListImpl()
				: this(0)
			{
			}

			// Token: 0x060003A0 RID: 928 RVA: 0x00015644 File Offset: 0x00013844
			public AttrListImpl(int initialCapacity)
			{
				if (initialCapacity <= 0)
				{
					this.names = new ArrayList();
					this.values = new ArrayList();
				}
				else
				{
					this.names = new ArrayList(initialCapacity);
					this.values = new ArrayList(initialCapacity);
				}
			}

			// Token: 0x060003A1 RID: 929 RVA: 0x00015694 File Offset: 0x00013894
			public AttrListImpl(MiniParser.IAttrList attrs)
				: this((attrs == null) ? 0 : attrs.Length)
			{
				if (attrs != null)
				{
					this.CopyFrom(attrs);
				}
			}

			// Token: 0x1700006F RID: 111
			// (get) Token: 0x060003A2 RID: 930 RVA: 0x000156BC File Offset: 0x000138BC
			public int Length
			{
				get
				{
					return this.names.Count;
				}
			}

			// Token: 0x17000070 RID: 112
			// (get) Token: 0x060003A3 RID: 931 RVA: 0x000156CC File Offset: 0x000138CC
			public bool IsEmpty
			{
				get
				{
					return this.Length != 0;
				}
			}

			// Token: 0x060003A4 RID: 932 RVA: 0x000156DC File Offset: 0x000138DC
			public string GetName(int i)
			{
				string text = null;
				if (i >= 0 && i < this.Length)
				{
					text = this.names[i] as string;
				}
				return text;
			}

			// Token: 0x060003A5 RID: 933 RVA: 0x00015714 File Offset: 0x00013914
			public string GetValue(int i)
			{
				string text = null;
				if (i >= 0 && i < this.Length)
				{
					text = this.values[i] as string;
				}
				return text;
			}

			// Token: 0x060003A6 RID: 934 RVA: 0x0001574C File Offset: 0x0001394C
			public string GetValue(string name)
			{
				return this.GetValue(this.names.IndexOf(name));
			}

			// Token: 0x060003A7 RID: 935 RVA: 0x00015760 File Offset: 0x00013960
			public void ChangeValue(string name, string newValue)
			{
				int num = this.names.IndexOf(name);
				if (num >= 0 && num < this.Length)
				{
					this.values[num] = newValue;
				}
			}

			// Token: 0x17000071 RID: 113
			// (get) Token: 0x060003A8 RID: 936 RVA: 0x0001579C File Offset: 0x0001399C
			public string[] Names
			{
				get
				{
					return this.names.ToArray(typeof(string)) as string[];
				}
			}

			// Token: 0x17000072 RID: 114
			// (get) Token: 0x060003A9 RID: 937 RVA: 0x000157B8 File Offset: 0x000139B8
			public string[] Values
			{
				get
				{
					return this.values.ToArray(typeof(string)) as string[];
				}
			}

			// Token: 0x060003AA RID: 938 RVA: 0x000157D4 File Offset: 0x000139D4
			public void Clear()
			{
				this.names.Clear();
				this.values.Clear();
			}

			// Token: 0x060003AB RID: 939 RVA: 0x000157EC File Offset: 0x000139EC
			public void Add(string name, string value)
			{
				this.names.Add(name);
				this.values.Add(value);
			}

			// Token: 0x060003AC RID: 940 RVA: 0x00015808 File Offset: 0x00013A08
			public void Remove(int i)
			{
				if (i >= 0)
				{
					this.names.RemoveAt(i);
					this.values.RemoveAt(i);
				}
			}

			// Token: 0x060003AD RID: 941 RVA: 0x0001582C File Offset: 0x00013A2C
			public void Remove(string name)
			{
				this.Remove(this.names.IndexOf(name));
			}

			// Token: 0x060003AE RID: 942 RVA: 0x00015840 File Offset: 0x00013A40
			public void CopyFrom(MiniParser.IAttrList attrs)
			{
				if (attrs != null && this == attrs)
				{
					this.Clear();
					int length = attrs.Length;
					for (int i = 0; i < length; i++)
					{
						this.Add(attrs.GetName(i), attrs.GetValue(i));
					}
				}
			}

			// Token: 0x040001FD RID: 509
			protected ArrayList names;

			// Token: 0x040001FE RID: 510
			protected ArrayList values;
		}

		// Token: 0x02000044 RID: 68
		public class XMLError : Exception
		{
			// Token: 0x060003AF RID: 943 RVA: 0x00015890 File Offset: 0x00013A90
			public XMLError()
				: this("Unknown")
			{
			}

			// Token: 0x060003B0 RID: 944 RVA: 0x000158A0 File Offset: 0x00013AA0
			public XMLError(string descr)
				: this(descr, -1, -1)
			{
			}

			// Token: 0x060003B1 RID: 945 RVA: 0x000158AC File Offset: 0x00013AAC
			public XMLError(string descr, int line, int column)
				: base(descr)
			{
				this.descr = descr;
				this.line = line;
				this.column = column;
			}

			// Token: 0x17000073 RID: 115
			// (get) Token: 0x060003B2 RID: 946 RVA: 0x000158CC File Offset: 0x00013ACC
			public int Line
			{
				get
				{
					return this.line;
				}
			}

			// Token: 0x17000074 RID: 116
			// (get) Token: 0x060003B3 RID: 947 RVA: 0x000158D4 File Offset: 0x00013AD4
			public int Column
			{
				get
				{
					return this.column;
				}
			}

			// Token: 0x060003B4 RID: 948 RVA: 0x000158DC File Offset: 0x00013ADC
			public override string ToString()
			{
				return string.Format("{0} @ (line = {1}, col = {2})", this.descr, this.line, this.column);
			}

			// Token: 0x040001FF RID: 511
			protected string descr;

			// Token: 0x04000200 RID: 512
			protected int line;

			// Token: 0x04000201 RID: 513
			protected int column;
		}
	}
}
