using System;
using System.Collections.Generic;
using System.Text;

namespace Prime31
{
	// Token: 0x02000016 RID: 22
	public class JsonFormatter
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00005D40 File Offset: 0x00003F40
		public static string prettyPrint(string input)
		{
			string text;
			try
			{
				text = new JsonFormatter().print(input);
			}
			catch (Exception)
			{
				text = null;
			}
			return text;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005D78 File Offset: 0x00003F78
		private static void buildIndents(int indents, StringBuilder output)
		{
			for (indents = indents; indents > 0; indents--)
			{
				output.Append("\t");
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00005D9A File Offset: 0x00003F9A
		private bool inString()
		{
			return this.inDoubleString || this.inSingleString;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005DB0 File Offset: 0x00003FB0
		public string print(string input)
		{
			StringBuilder stringBuilder = new StringBuilder(input.Length * 2);
			foreach (char c in input)
			{
				switch (c)
				{
				case ' ':
					if (this.inString())
					{
						stringBuilder.Append(c);
					}
					break;
				default:
					switch (c)
					{
					case ':':
						if (!this.inString())
						{
							this.inVariableAssignment = true;
							stringBuilder.Append(c);
							stringBuilder.Append(" ");
						}
						else
						{
							stringBuilder.Append(c);
						}
						break;
					default:
						switch (c)
						{
						case '[':
							stringBuilder.Append(c);
							if (!this.inString())
							{
								this.context.Push(JsonFormatter.JsonContextType.Array);
							}
							break;
						default:
							switch (c)
							{
							case '{':
								if (!this.inString())
								{
									if (this.inVariableAssignment || (this.context.Count > 0 && this.context.Peek() != JsonFormatter.JsonContextType.Array))
									{
										stringBuilder.Append(Environment.NewLine);
										JsonFormatter.buildIndents(this.context.Count, stringBuilder);
									}
									stringBuilder.Append(c);
									this.context.Push(JsonFormatter.JsonContextType.Object);
									stringBuilder.Append(Environment.NewLine);
									JsonFormatter.buildIndents(this.context.Count, stringBuilder);
								}
								else
								{
									stringBuilder.Append(c);
								}
								break;
							default:
								if (c != '\'')
								{
									if (c != ',')
									{
										stringBuilder.Append(c);
									}
									else
									{
										stringBuilder.Append(c);
										if (!this.inString())
										{
											stringBuilder.Append(" ");
										}
										if (!this.inString() && this.context.Peek() != JsonFormatter.JsonContextType.Array)
										{
											JsonFormatter.buildIndents(this.context.Count, stringBuilder);
											stringBuilder.Append(Environment.NewLine);
											JsonFormatter.buildIndents(this.context.Count, stringBuilder);
											this.inVariableAssignment = false;
										}
									}
								}
								else
								{
									if (!this.inDoubleString && this.prevChar != '\\')
									{
										this.inSingleString = !this.inSingleString;
									}
									stringBuilder.Append(c);
								}
								break;
							case '}':
								if (!this.inString())
								{
									stringBuilder.Append(Environment.NewLine);
									this.context.Pop();
									JsonFormatter.buildIndents(this.context.Count, stringBuilder);
									stringBuilder.Append(c);
								}
								else
								{
									stringBuilder.Append(c);
								}
								break;
							}
							break;
						case ']':
							if (!this.inString())
							{
								stringBuilder.Append(c);
								this.context.Pop();
							}
							else
							{
								stringBuilder.Append(c);
							}
							break;
						}
						break;
					case '=':
						stringBuilder.Append(c);
						break;
					}
					break;
				case '"':
					if (!this.inSingleString && this.prevChar != '\\')
					{
						this.inDoubleString = !this.inDoubleString;
					}
					stringBuilder.Append(c);
					break;
				}
				this.prevChar = c;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400003C RID: 60
		private const int defaultIndent = 0;

		// Token: 0x0400003D RID: 61
		private const string indent = "\t";

		// Token: 0x0400003E RID: 62
		private const string space = " ";

		// Token: 0x0400003F RID: 63
		private bool inDoubleString = false;

		// Token: 0x04000040 RID: 64
		private bool inSingleString = false;

		// Token: 0x04000041 RID: 65
		private bool inVariableAssignment = false;

		// Token: 0x04000042 RID: 66
		private char prevChar = '\0';

		// Token: 0x04000043 RID: 67
		private Stack<JsonFormatter.JsonContextType> context = new Stack<JsonFormatter.JsonContextType>();

		// Token: 0x02000017 RID: 23
		private enum JsonContextType
		{
			// Token: 0x04000045 RID: 69
			Object,
			// Token: 0x04000046 RID: 70
			Array
		}
	}
}
