using System;
using System.Collections.Generic;
using System.IO;

namespace LitJson
{
	// Token: 0x02000027 RID: 39
	public class JsonReader
	{
		// Token: 0x06000265 RID: 613 RVA: 0x0000EF5C File Offset: 0x0000D15C
		public JsonReader(string json_text)
			: this(new StringReader(json_text), true)
		{
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000EF6C File Offset: 0x0000D16C
		public JsonReader(TextReader reader)
			: this(reader, false)
		{
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000EF78 File Offset: 0x0000D178
		private JsonReader(TextReader reader, bool owned)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.read_started = false;
			this.automaton_stack = new Stack<int>();
			this.automaton_stack.Push(65553);
			this.automaton_stack.Push(65543);
			this.lexer = new Lexer(reader);
			this.end_of_input = false;
			this.end_of_json = false;
			this.reader = reader;
			this.reader_is_owned = owned;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000F004 File Offset: 0x0000D204
		static JsonReader()
		{
			JsonReader.PopulateParseTable();
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000F00C File Offset: 0x0000D20C
		// (set) Token: 0x0600026A RID: 618 RVA: 0x0000F01C File Offset: 0x0000D21C
		public bool AllowComments
		{
			get
			{
				return this.lexer.AllowComments;
			}
			set
			{
				this.lexer.AllowComments = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000F02C File Offset: 0x0000D22C
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0000F03C File Offset: 0x0000D23C
		public bool AllowSingleQuotedStrings
		{
			get
			{
				return this.lexer.AllowSingleQuotedStrings;
			}
			set
			{
				this.lexer.AllowSingleQuotedStrings = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000F04C File Offset: 0x0000D24C
		public bool EndOfInput
		{
			get
			{
				return this.end_of_input;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000F054 File Offset: 0x0000D254
		public bool EndOfJson
		{
			get
			{
				return this.end_of_json;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000F05C File Offset: 0x0000D25C
		public JsonToken Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000270 RID: 624 RVA: 0x0000F064 File Offset: 0x0000D264
		public object Value
		{
			get
			{
				return this.token_value;
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000F06C File Offset: 0x0000D26C
		private static void PopulateParseTable()
		{
			JsonReader.parse_table = new Dictionary<int, IDictionary<int, int[]>>();
			JsonReader.TableAddRow(ParserToken.Array);
			JsonReader.TableAddCol(ParserToken.Array, 91, new int[] { 91, 65549 });
			JsonReader.TableAddRow(ParserToken.ArrayPrime);
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 34, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 91, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 93, new int[] { 93 });
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 123, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65537, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65538, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65539, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65540, new int[] { 65550, 65551, 93 });
			JsonReader.TableAddRow(ParserToken.Object);
			JsonReader.TableAddCol(ParserToken.Object, 123, new int[] { 123, 65545 });
			JsonReader.TableAddRow(ParserToken.ObjectPrime);
			JsonReader.TableAddCol(ParserToken.ObjectPrime, 34, new int[] { 65546, 65547, 125 });
			JsonReader.TableAddCol(ParserToken.ObjectPrime, 125, new int[] { 125 });
			JsonReader.TableAddRow(ParserToken.Pair);
			JsonReader.TableAddCol(ParserToken.Pair, 34, new int[] { 65552, 58, 65550 });
			JsonReader.TableAddRow(ParserToken.PairRest);
			JsonReader.TableAddCol(ParserToken.PairRest, 44, new int[] { 44, 65546, 65547 });
			JsonReader.TableAddCol(ParserToken.PairRest, 125, new int[] { 65554 });
			JsonReader.TableAddRow(ParserToken.String);
			JsonReader.TableAddCol(ParserToken.String, 34, new int[] { 34, 65541, 34 });
			JsonReader.TableAddRow(ParserToken.Text);
			JsonReader.TableAddCol(ParserToken.Text, 91, new int[] { 65548 });
			JsonReader.TableAddCol(ParserToken.Text, 123, new int[] { 65544 });
			JsonReader.TableAddRow(ParserToken.Value);
			JsonReader.TableAddCol(ParserToken.Value, 34, new int[] { 65552 });
			JsonReader.TableAddCol(ParserToken.Value, 91, new int[] { 65548 });
			JsonReader.TableAddCol(ParserToken.Value, 123, new int[] { 65544 });
			JsonReader.TableAddCol(ParserToken.Value, 65537, new int[] { 65537 });
			JsonReader.TableAddCol(ParserToken.Value, 65538, new int[] { 65538 });
			JsonReader.TableAddCol(ParserToken.Value, 65539, new int[] { 65539 });
			JsonReader.TableAddCol(ParserToken.Value, 65540, new int[] { 65540 });
			JsonReader.TableAddRow(ParserToken.ValueRest);
			JsonReader.TableAddCol(ParserToken.ValueRest, 44, new int[] { 44, 65550, 65551 });
			JsonReader.TableAddCol(ParserToken.ValueRest, 93, new int[] { 65554 });
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000F45C File Offset: 0x0000D65C
		private static void TableAddCol(ParserToken row, int col, params int[] symbols)
		{
			JsonReader.parse_table[(int)row].Add(col, symbols);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000F470 File Offset: 0x0000D670
		private static void TableAddRow(ParserToken rule)
		{
			JsonReader.parse_table.Add((int)rule, new Dictionary<int, int[]>());
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000F484 File Offset: 0x0000D684
		private void ProcessNumber(string number)
		{
			double num;
			if ((number.IndexOf('.') != -1 || number.IndexOf('e') != -1 || number.IndexOf('E') != -1) && double.TryParse(number, out num))
			{
				this.token = JsonToken.Double;
				this.token_value = num;
				return;
			}
			int num2;
			if (int.TryParse(number, out num2))
			{
				this.token = JsonToken.Int;
				this.token_value = num2;
				return;
			}
			long num3;
			if (long.TryParse(number, out num3))
			{
				this.token = JsonToken.Long;
				this.token_value = num3;
				return;
			}
			this.token = JsonToken.Int;
			this.token_value = 0;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000F534 File Offset: 0x0000D734
		private void ProcessSymbol()
		{
			if (this.current_symbol == 91)
			{
				this.token = JsonToken.ArrayStart;
				this.parser_return = true;
			}
			else if (this.current_symbol == 93)
			{
				this.token = JsonToken.ArrayEnd;
				this.parser_return = true;
			}
			else if (this.current_symbol == 123)
			{
				this.token = JsonToken.ObjectStart;
				this.parser_return = true;
			}
			else if (this.current_symbol == 125)
			{
				this.token = JsonToken.ObjectEnd;
				this.parser_return = true;
			}
			else if (this.current_symbol == 34)
			{
				if (this.parser_in_string)
				{
					this.parser_in_string = false;
					this.parser_return = true;
				}
				else
				{
					if (this.token == JsonToken.None)
					{
						this.token = JsonToken.String;
					}
					this.parser_in_string = true;
				}
			}
			else if (this.current_symbol == 65541)
			{
				this.token_value = this.lexer.StringValue;
			}
			else if (this.current_symbol == 65539)
			{
				this.token = JsonToken.Boolean;
				this.token_value = false;
				this.parser_return = true;
			}
			else if (this.current_symbol == 65540)
			{
				this.token = JsonToken.Null;
				this.parser_return = true;
			}
			else if (this.current_symbol == 65537)
			{
				this.ProcessNumber(this.lexer.StringValue);
				this.parser_return = true;
			}
			else if (this.current_symbol == 65546)
			{
				this.token = JsonToken.PropertyName;
			}
			else if (this.current_symbol == 65538)
			{
				this.token = JsonToken.Boolean;
				this.token_value = true;
				this.parser_return = true;
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000F6FC File Offset: 0x0000D8FC
		private bool ReadToken()
		{
			if (this.end_of_input)
			{
				return false;
			}
			this.lexer.NextToken();
			if (this.lexer.EndOfInput)
			{
				this.Close();
				return false;
			}
			this.current_input = this.lexer.Token;
			return true;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000F74C File Offset: 0x0000D94C
		public void Close()
		{
			if (this.end_of_input)
			{
				return;
			}
			this.end_of_input = true;
			this.end_of_json = true;
			if (this.reader_is_owned)
			{
				this.reader.Close();
			}
			this.reader = null;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000F788 File Offset: 0x0000D988
		public bool Read()
		{
			if (this.end_of_input)
			{
				return false;
			}
			if (this.end_of_json)
			{
				this.end_of_json = false;
				this.automaton_stack.Clear();
				this.automaton_stack.Push(65553);
				this.automaton_stack.Push(65543);
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.token = JsonToken.None;
			this.token_value = null;
			if (!this.read_started)
			{
				this.read_started = true;
				if (!this.ReadToken())
				{
					return false;
				}
			}
			while (!this.parser_return)
			{
				this.current_symbol = this.automaton_stack.Pop();
				this.ProcessSymbol();
				if (this.current_symbol == this.current_input)
				{
					if (!this.ReadToken())
					{
						if (this.automaton_stack.Peek() != 65553)
						{
							throw new JsonException("Input doesn't evaluate to proper JSON text");
						}
						return this.parser_return;
					}
				}
				else
				{
					int[] array;
					try
					{
						array = JsonReader.parse_table[this.current_symbol][this.current_input];
					}
					catch (KeyNotFoundException ex)
					{
						throw new JsonException((ParserToken)this.current_input, ex);
					}
					if (array[0] != 65554)
					{
						for (int i = array.Length - 1; i >= 0; i--)
						{
							this.automaton_stack.Push(array[i]);
						}
					}
				}
			}
			if (this.automaton_stack.Peek() == 65553)
			{
				this.end_of_json = true;
			}
			return true;
		}

		// Token: 0x0400014D RID: 333
		private static IDictionary<int, IDictionary<int, int[]>> parse_table;

		// Token: 0x0400014E RID: 334
		private Stack<int> automaton_stack;

		// Token: 0x0400014F RID: 335
		private int current_input;

		// Token: 0x04000150 RID: 336
		private int current_symbol;

		// Token: 0x04000151 RID: 337
		private bool end_of_json;

		// Token: 0x04000152 RID: 338
		private bool end_of_input;

		// Token: 0x04000153 RID: 339
		private Lexer lexer;

		// Token: 0x04000154 RID: 340
		private bool parser_in_string;

		// Token: 0x04000155 RID: 341
		private bool parser_return;

		// Token: 0x04000156 RID: 342
		private bool read_started;

		// Token: 0x04000157 RID: 343
		private TextReader reader;

		// Token: 0x04000158 RID: 344
		private bool reader_is_owned;

		// Token: 0x04000159 RID: 345
		private object token_value;

		// Token: 0x0400015A RID: 346
		private JsonToken token;
	}
}
