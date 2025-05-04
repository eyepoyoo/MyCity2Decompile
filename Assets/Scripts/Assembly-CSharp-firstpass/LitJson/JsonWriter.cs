using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace LitJson
{
	// Token: 0x0200002A RID: 42
	public class JsonWriter
	{
		// Token: 0x0600027A RID: 634 RVA: 0x0000F93C File Offset: 0x0000DB3C
		public JsonWriter()
		{
			this.inst_string_builder = new StringBuilder();
			this.writer = new StringWriter(this.inst_string_builder);
			this.Init();
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000F974 File Offset: 0x0000DB74
		public JsonWriter(StringBuilder sb)
			: this(new StringWriter(sb))
		{
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000F984 File Offset: 0x0000DB84
		public JsonWriter(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
			this.Init();
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0000F9C4 File Offset: 0x0000DBC4
		// (set) Token: 0x0600027F RID: 639 RVA: 0x0000F9CC File Offset: 0x0000DBCC
		public int IndentValue
		{
			get
			{
				return this.indent_value;
			}
			set
			{
				this.indentation = this.indentation / this.indent_value * value;
				this.indent_value = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000F9EC File Offset: 0x0000DBEC
		// (set) Token: 0x06000281 RID: 641 RVA: 0x0000F9F4 File Offset: 0x0000DBF4
		public bool PrettyPrint
		{
			get
			{
				return this.pretty_print;
			}
			set
			{
				this.pretty_print = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000FA00 File Offset: 0x0000DC00
		public TextWriter TextWriter
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000FA08 File Offset: 0x0000DC08
		// (set) Token: 0x06000284 RID: 644 RVA: 0x0000FA10 File Offset: 0x0000DC10
		public bool Validate
		{
			get
			{
				return this.validate;
			}
			set
			{
				this.validate = value;
			}
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000FA1C File Offset: 0x0000DC1C
		private void DoValidation(Condition cond)
		{
			if (!this.context.ExpectingValue)
			{
				this.context.Count++;
			}
			if (!this.validate)
			{
				return;
			}
			if (this.has_reached_end)
			{
				throw new JsonException("A complete JSON symbol has already been written");
			}
			switch (cond)
			{
			case Condition.InArray:
				if (!this.context.InArray)
				{
					throw new JsonException("Can't close an array here");
				}
				break;
			case Condition.InObject:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't close an object here");
				}
				break;
			case Condition.NotAProperty:
				if (this.context.InObject && !this.context.ExpectingValue)
				{
					throw new JsonException("Expected a property");
				}
				break;
			case Condition.Property:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't add a property here");
				}
				break;
			case Condition.Value:
				if (!this.context.InArray && (!this.context.InObject || !this.context.ExpectingValue))
				{
					throw new JsonException("Can't add a value here");
				}
				break;
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000FB80 File Offset: 0x0000DD80
		private void Init()
		{
			this.has_reached_end = false;
			this.hex_seq = new char[4];
			this.indentation = 0;
			this.indent_value = 1;
			this.pretty_print = false;
			this.validate = true;
			this.ctx_stack = new Stack<WriterContext>();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000FBE4 File Offset: 0x0000DDE4
		private static void IntToHex(int n, char[] hex)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = n % 16;
				if (num < 10)
				{
					hex[3 - i] = (char)(48 + num);
				}
				else
				{
					hex[3 - i] = (char)(65 + (num - 10));
				}
				n >>= 4;
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000FC34 File Offset: 0x0000DE34
		private void Indent()
		{
			if (this.pretty_print)
			{
				this.indentation += this.indent_value;
			}
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000FC54 File Offset: 0x0000DE54
		private void Put(string str)
		{
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				for (int i = 0; i < this.indentation; i++)
				{
					this.writer.Write('\t');
				}
			}
			this.writer.Write(str);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000FCAC File Offset: 0x0000DEAC
		private void PutNewline()
		{
			this.PutNewline(true);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000FCB8 File Offset: 0x0000DEB8
		private void PutNewline(bool add_comma)
		{
			if (add_comma && !this.context.ExpectingValue && this.context.Count > 1)
			{
				this.writer.Write(',');
			}
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				this.writer.Write('\n');
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000FD24 File Offset: 0x0000DF24
		private void PutString(string str)
		{
			this.Put(string.Empty);
			this.writer.Write('"');
			int length = str.Length;
			for (int i = 0; i < length; i++)
			{
				char c = str[i];
				switch (c)
				{
				case '\b':
					this.writer.Write("\\b");
					break;
				case '\t':
					this.writer.Write("\\t");
					break;
				case '\n':
					this.writer.Write("\\n");
					break;
				default:
					if (c != '"' && c != '\\')
					{
						if (str[i] >= ' ' && str[i] <= '~')
						{
							this.writer.Write(str[i]);
						}
						else
						{
							JsonWriter.IntToHex((int)str[i], this.hex_seq);
							this.writer.Write("\\u");
							this.writer.Write(this.hex_seq);
						}
					}
					else
					{
						this.writer.Write('\\');
						this.writer.Write(str[i]);
					}
					break;
				case '\f':
					this.writer.Write("\\f");
					break;
				case '\r':
					this.writer.Write("\\r");
					break;
				}
			}
			this.writer.Write('"');
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000FEA0 File Offset: 0x0000E0A0
		private void Unindent()
		{
			if (this.pretty_print)
			{
				this.indentation -= this.indent_value;
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000FEC0 File Offset: 0x0000E0C0
		public override string ToString()
		{
			if (this.inst_string_builder == null)
			{
				return string.Empty;
			}
			return this.inst_string_builder.ToString();
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000FEE0 File Offset: 0x0000E0E0
		public void Reset()
		{
			this.has_reached_end = false;
			this.ctx_stack.Clear();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
			if (this.inst_string_builder != null)
			{
				this.inst_string_builder.Remove(0, this.inst_string_builder.Length);
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000FF40 File Offset: 0x0000E140
		public void Write(bool boolean)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put((!boolean) ? "false" : "true");
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000FF84 File Offset: 0x0000E184
		public void Write(decimal number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000FFBC File Offset: 0x0000E1BC
		public void Write(double number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			string text = Convert.ToString(number, JsonWriter.number_format);
			this.Put(text);
			if (text.IndexOf('.') == -1 && text.IndexOf('E') == -1)
			{
				this.writer.Write(".0");
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00010024 File Offset: 0x0000E224
		public void Write(int number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0001005C File Offset: 0x0000E25C
		public void Write(long number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00010094 File Offset: 0x0000E294
		public void Write(string str)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			if (str == null)
			{
				this.Put("null");
			}
			else
			{
				this.PutString(str);
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x000100D8 File Offset: 0x0000E2D8
		public void Write(ulong number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00010110 File Offset: 0x0000E310
		public void WriteArrayEnd()
		{
			this.DoValidation(Condition.InArray);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("]");
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00010184 File Offset: 0x0000E384
		public void WriteArrayStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("[");
			this.context = new WriterContext();
			this.context.InArray = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x06000299 RID: 665 RVA: 0x000101D8 File Offset: 0x0000E3D8
		public void WriteObjectEnd()
		{
			this.DoValidation(Condition.InObject);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("}");
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0001024C File Offset: 0x0000E44C
		public void WriteObjectStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("{");
			this.context = new WriterContext();
			this.context.InObject = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000102A0 File Offset: 0x0000E4A0
		public void WritePropertyName(string property_name)
		{
			this.DoValidation(Condition.Property);
			this.PutNewline();
			this.PutString(property_name);
			if (this.pretty_print)
			{
				if (property_name.Length > this.context.Padding)
				{
					this.context.Padding = property_name.Length;
				}
				this.writer.Write(" : ");
			}
			else
			{
				this.writer.Write(':');
			}
			this.context.ExpectingValue = true;
		}

		// Token: 0x04000166 RID: 358
		private static NumberFormatInfo number_format = NumberFormatInfo.InvariantInfo;

		// Token: 0x04000167 RID: 359
		private WriterContext context;

		// Token: 0x04000168 RID: 360
		private Stack<WriterContext> ctx_stack;

		// Token: 0x04000169 RID: 361
		private bool has_reached_end;

		// Token: 0x0400016A RID: 362
		private char[] hex_seq;

		// Token: 0x0400016B RID: 363
		private int indentation;

		// Token: 0x0400016C RID: 364
		private int indent_value;

		// Token: 0x0400016D RID: 365
		private StringBuilder inst_string_builder;

		// Token: 0x0400016E RID: 366
		private bool pretty_print;

		// Token: 0x0400016F RID: 367
		private bool validate;

		// Token: 0x04000170 RID: 368
		private TextWriter writer;
	}
}
