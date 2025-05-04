using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using UnityEngine;

namespace LitJson
{
	// Token: 0x02000021 RID: 33
	public class JsonData : IJsonWrapper, IDictionary, IEquatable<JsonData>, IList, ICollection, IEnumerable, IOrderedDictionary
	{
		// Token: 0x060001EA RID: 490 RVA: 0x0000D4E4 File Offset: 0x0000B6E4
		public JsonData()
		{
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000D4EC File Offset: 0x0000B6EC
		public JsonData(object obj, JsonType explicitType)
		{
			this.type = explicitType;
			switch (explicitType)
			{
			case JsonType.Object:
			case JsonType.Array:
				throw new ArgumentException("Unable to wrap the given object with JsonData of type " + explicitType);
			case JsonType.String:
				this.inst_string = (string)obj;
				break;
			case JsonType.Int:
				this.inst_int = (int)obj;
				break;
			case JsonType.Long:
				this.inst_long = (long)obj;
				break;
			case JsonType.Double:
				this.inst_double = (double)obj;
				break;
			case JsonType.Boolean:
				this.inst_boolean = (bool)obj;
				break;
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000D59C File Offset: 0x0000B79C
		public JsonData(bool boolean)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = boolean;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000D5B4 File Offset: 0x0000B7B4
		public JsonData(double number)
		{
			this.type = JsonType.Double;
			this.inst_double = number;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000D5CC File Offset: 0x0000B7CC
		public JsonData(float number)
		{
			this.type = JsonType.Double;
			this.inst_double = (double)number;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000D5E4 File Offset: 0x0000B7E4
		public JsonData(int number)
		{
			this.type = JsonType.Int;
			this.inst_int = number;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000D5FC File Offset: 0x0000B7FC
		public JsonData(long number)
		{
			this.type = JsonType.Long;
			this.inst_long = number;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000D614 File Offset: 0x0000B814
		public JsonData(object obj)
		{
			if (obj is bool)
			{
				this.type = JsonType.Boolean;
				this.inst_boolean = (bool)obj;
				return;
			}
			if (obj is double)
			{
				this.type = JsonType.Double;
				this.inst_double = (double)obj;
				return;
			}
			if (obj is int)
			{
				this.type = JsonType.Int;
				this.inst_int = (int)obj;
				return;
			}
			if (obj is long)
			{
				this.type = JsonType.Long;
				this.inst_long = (long)obj;
				return;
			}
			if (obj is int)
			{
				this.type = JsonType.Int;
				this.inst_int = (int)obj;
				return;
			}
			if (obj is long)
			{
				this.type = JsonType.Long;
				this.inst_long = (long)obj;
				return;
			}
			if (obj is string)
			{
				this.type = JsonType.String;
				this.inst_string = (string)obj;
				return;
			}
			if (obj is IList<JsonData>)
			{
				this.type = JsonType.Array;
				this.inst_array = (IList<JsonData>)obj;
				return;
			}
			if (obj is Vector3)
			{
				this.type = JsonType.Array;
				Vector3 vector = (Vector3)obj;
				this.inst_array = new List<JsonData> { vector.x, vector.y, vector.z };
				return;
			}
			throw new ArgumentException("Unable to wrap the given object with JsonData - type: " + obj.GetType());
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000D794 File Offset: 0x0000B994
		public JsonData(string str)
		{
			this.type = JsonType.String;
			this.inst_string = str;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000D7AC File Offset: 0x0000B9AC
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000D7B4 File Offset: 0x0000B9B4
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.EnsureCollection().IsSynchronized;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000D7C4 File Offset: 0x0000B9C4
		object ICollection.SyncRoot
		{
			get
			{
				return this.EnsureCollection().SyncRoot;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000D7D4 File Offset: 0x0000B9D4
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this.EnsureDictionary().IsFixedSize;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000D7E4 File Offset: 0x0000B9E4
		bool IDictionary.IsReadOnly
		{
			get
			{
				return this.EnsureDictionary().IsReadOnly;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000D7F4 File Offset: 0x0000B9F4
		ICollection IDictionary.Keys
		{
			get
			{
				this.EnsureDictionary();
				IList<string> list = new List<string>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Key);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000D870 File Offset: 0x0000BA70
		ICollection IDictionary.Values
		{
			get
			{
				this.EnsureDictionary();
				IList<JsonData> list = new List<JsonData>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Value);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000D8EC File Offset: 0x0000BAEC
		bool IJsonWrapper.IsArray
		{
			get
			{
				return this.IsArray;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060001FB RID: 507 RVA: 0x0000D8F4 File Offset: 0x0000BAF4
		bool IJsonWrapper.IsBoolean
		{
			get
			{
				return this.IsBoolean;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000D8FC File Offset: 0x0000BAFC
		bool IJsonWrapper.IsDouble
		{
			get
			{
				return this.IsDouble;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0000D904 File Offset: 0x0000BB04
		bool IJsonWrapper.IsInt
		{
			get
			{
				return this.IsInt;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000D90C File Offset: 0x0000BB0C
		bool IJsonWrapper.IsLong
		{
			get
			{
				return this.IsLong;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060001FF RID: 511 RVA: 0x0000D914 File Offset: 0x0000BB14
		bool IJsonWrapper.IsObject
		{
			get
			{
				return this.IsObject;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000D91C File Offset: 0x0000BB1C
		bool IJsonWrapper.IsString
		{
			get
			{
				return this.IsString;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0000D924 File Offset: 0x0000BB24
		bool IList.IsFixedSize
		{
			get
			{
				return this.EnsureList().IsFixedSize;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000D934 File Offset: 0x0000BB34
		bool IList.IsReadOnly
		{
			get
			{
				return this.EnsureList().IsReadOnly;
			}
		}

		// Token: 0x1700002F RID: 47
		object IDictionary.this[object key]
		{
			get
			{
				return this.EnsureDictionary()[key];
			}
			set
			{
				if (!(key is string))
				{
					throw new ArgumentException("The key has to be a string");
				}
				JsonData jsonData = this.ToJsonData(value);
				this[(string)key] = jsonData;
			}
		}

		// Token: 0x17000030 RID: 48
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				this.EnsureDictionary();
				return this.object_list[idx].Value;
			}
			set
			{
				this.EnsureDictionary();
				JsonData jsonData = this.ToJsonData(value);
				KeyValuePair<string, JsonData> keyValuePair = this.object_list[idx];
				this.inst_object[keyValuePair.Key] = jsonData;
				KeyValuePair<string, JsonData> keyValuePair2 = new KeyValuePair<string, JsonData>(keyValuePair.Key, jsonData);
				this.object_list[idx] = keyValuePair2;
			}
		}

		// Token: 0x17000031 RID: 49
		object IList.this[int index]
		{
			get
			{
				return this.EnsureList()[index];
			}
			set
			{
				this.EnsureList();
				JsonData jsonData = this.ToJsonData(value);
				this[index] = jsonData;
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000DA40 File Offset: 0x0000BC40
		void ICollection.CopyTo(Array array, int index)
		{
			this.EnsureCollection().CopyTo(array, index);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000DA50 File Offset: 0x0000BC50
		void IDictionary.Add(object key, object value)
		{
			JsonData jsonData = this.ToJsonData(value);
			this.EnsureDictionary().Add(key, jsonData);
			KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>((string)key, jsonData);
			this.object_list.Add(keyValuePair);
			this.json = null;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000DA94 File Offset: 0x0000BC94
		void IDictionary.Clear()
		{
			this.EnsureDictionary().Clear();
			this.object_list.Clear();
			this.json = null;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000DAB4 File Offset: 0x0000BCB4
		bool IDictionary.Contains(object key)
		{
			return this.EnsureDictionary().Contains(key);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000DAC4 File Offset: 0x0000BCC4
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IOrderedDictionary)this).GetEnumerator();
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000DACC File Offset: 0x0000BCCC
		void IDictionary.Remove(object key)
		{
			this.EnsureDictionary().Remove(key);
			for (int i = 0; i < this.object_list.Count; i++)
			{
				if (this.object_list[i].Key == (string)key)
				{
					this.object_list.RemoveAt(i);
					break;
				}
			}
			this.json = null;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000DB40 File Offset: 0x0000BD40
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.EnsureCollection().GetEnumerator();
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000DB50 File Offset: 0x0000BD50
		bool IJsonWrapper.GetBoolean()
		{
			if (this.type != JsonType.Boolean)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a boolean");
			}
			return this.inst_boolean;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000DB70 File Offset: 0x0000BD70
		double IJsonWrapper.GetDouble()
		{
			if (this.type != JsonType.Double)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a double");
			}
			return this.inst_double;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000DB90 File Offset: 0x0000BD90
		int IJsonWrapper.GetInt()
		{
			if (this.type != JsonType.Int)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold an int");
			}
			return this.inst_int;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000DBB0 File Offset: 0x0000BDB0
		long IJsonWrapper.GetLong()
		{
			if (this.type != JsonType.Long)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a long");
			}
			return this.inst_long;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000DBD0 File Offset: 0x0000BDD0
		string IJsonWrapper.GetString()
		{
			if (this.type != JsonType.String)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a string");
			}
			return this.inst_string;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000DBF0 File Offset: 0x0000BDF0
		void IJsonWrapper.SetBoolean(bool val)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = val;
			this.json = null;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000DC08 File Offset: 0x0000BE08
		void IJsonWrapper.SetDouble(double val)
		{
			this.type = JsonType.Double;
			this.inst_double = val;
			this.json = null;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000DC20 File Offset: 0x0000BE20
		void IJsonWrapper.SetInt(int val)
		{
			this.type = JsonType.Int;
			this.inst_int = val;
			this.json = null;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000DC38 File Offset: 0x0000BE38
		void IJsonWrapper.SetLong(long val)
		{
			this.type = JsonType.Long;
			this.inst_long = val;
			this.json = null;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000DC50 File Offset: 0x0000BE50
		void IJsonWrapper.SetString(string val)
		{
			this.type = JsonType.String;
			this.inst_string = val;
			this.json = null;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000DC68 File Offset: 0x0000BE68
		string IJsonWrapper.ToJson()
		{
			return this.ToJson(false, false);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000DC74 File Offset: 0x0000BE74
		void IJsonWrapper.ToJson(JsonWriter writer)
		{
			this.ToJson(writer);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000DC80 File Offset: 0x0000BE80
		int IList.Add(object value)
		{
			return this.Add(value);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000DC8C File Offset: 0x0000BE8C
		void IList.Clear()
		{
			this.EnsureList().Clear();
			this.json = null;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000DCA0 File Offset: 0x0000BEA0
		bool IList.Contains(object value)
		{
			return this.EnsureList().Contains(value);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000DCB0 File Offset: 0x0000BEB0
		int IList.IndexOf(object value)
		{
			return this.EnsureList().IndexOf(value);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000DCC0 File Offset: 0x0000BEC0
		void IList.Insert(int index, object value)
		{
			this.EnsureList().Insert(index, value);
			this.json = null;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000DCD8 File Offset: 0x0000BED8
		void IList.Remove(object value)
		{
			this.EnsureList().Remove(value);
			this.json = null;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000DCF0 File Offset: 0x0000BEF0
		void IList.RemoveAt(int index)
		{
			this.EnsureList().RemoveAt(index);
			this.json = null;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000DD08 File Offset: 0x0000BF08
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			this.EnsureDictionary();
			return new OrderedDictionaryEnumerator(this.object_list.GetEnumerator());
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000DD24 File Offset: 0x0000BF24
		void IOrderedDictionary.Insert(int idx, object key, object value)
		{
			string text = (string)key;
			JsonData jsonData = this.ToJsonData(value);
			this[text] = jsonData;
			KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(text, jsonData);
			this.object_list.Insert(idx, keyValuePair);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000DD60 File Offset: 0x0000BF60
		void IOrderedDictionary.RemoveAt(int idx)
		{
			this.EnsureDictionary();
			this.inst_object.Remove(this.object_list[idx].Key);
			this.object_list.RemoveAt(idx);
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000DDA0 File Offset: 0x0000BFA0
		public int Count
		{
			get
			{
				return this.EnsureCollection().Count;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000DDB0 File Offset: 0x0000BFB0
		public bool IsArray
		{
			get
			{
				return this.type == JsonType.Array;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000DDBC File Offset: 0x0000BFBC
		public bool IsBoolean
		{
			get
			{
				return this.type == JsonType.Boolean;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000DDC8 File Offset: 0x0000BFC8
		public bool IsDouble
		{
			get
			{
				return this.type == JsonType.Double;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000DDD4 File Offset: 0x0000BFD4
		public bool IsInt
		{
			get
			{
				return this.type == JsonType.Int;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000DDE0 File Offset: 0x0000BFE0
		public bool IsLong
		{
			get
			{
				return this.type == JsonType.Long;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000DDEC File Offset: 0x0000BFEC
		public bool IsObject
		{
			get
			{
				return this.type == JsonType.Object;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000DDF8 File Offset: 0x0000BFF8
		public bool IsString
		{
			get
			{
				return this.type == JsonType.String;
			}
		}

		// Token: 0x1700003A RID: 58
		public JsonData this[string prop_name]
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object[prop_name];
			}
			set
			{
				this.EnsureDictionary();
				KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(prop_name, value);
				if (this.inst_object.ContainsKey(prop_name))
				{
					for (int i = 0; i < this.object_list.Count; i++)
					{
						if (this.object_list[i].Key == prop_name)
						{
							this.object_list[i] = keyValuePair;
							break;
						}
					}
				}
				else
				{
					this.object_list.Add(keyValuePair);
				}
				this.inst_object[prop_name] = value;
				this.json = null;
			}
		}

		// Token: 0x1700003B RID: 59
		public JsonData this[int index]
		{
			get
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					return this.inst_array[index];
				}
				return this.object_list[index].Value;
			}
			set
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					this.inst_array[index] = value;
				}
				else
				{
					KeyValuePair<string, JsonData> keyValuePair = this.object_list[index];
					KeyValuePair<string, JsonData> keyValuePair2 = new KeyValuePair<string, JsonData>(keyValuePair.Key, value);
					this.object_list[index] = keyValuePair2;
					this.inst_object[keyValuePair.Key] = value;
				}
				this.json = null;
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000DF78 File Offset: 0x0000C178
		public bool Contains(string key)
		{
			this.EnsureDictionary();
			return this.inst_object.ContainsKey(key);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000DF90 File Offset: 0x0000C190
		private ICollection EnsureCollection()
		{
			if (this.type == JsonType.Array)
			{
				return (ICollection)this.inst_array;
			}
			if (this.type == JsonType.Object)
			{
				return (ICollection)this.inst_object;
			}
			throw new InvalidOperationException("The JsonData instance has to be initialized first");
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000DFD8 File Offset: 0x0000C1D8
		private IDictionary EnsureDictionary()
		{
			if (this.type == JsonType.Object)
			{
				return (IDictionary)this.inst_object;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a dictionary");
			}
			this.type = JsonType.Object;
			this.inst_object = new Dictionary<string, JsonData>();
			this.object_list = new List<KeyValuePair<string, JsonData>>();
			return (IDictionary)this.inst_object;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000E03C File Offset: 0x0000C23C
		private IList EnsureList()
		{
			if (this.type == JsonType.Array)
			{
				return (IList)this.inst_array;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a list");
			}
			this.type = JsonType.Array;
			this.inst_array = new List<JsonData>();
			return (IList)this.inst_array;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000E094 File Offset: 0x0000C294
		private JsonData ToJsonData(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is JsonData)
			{
				return (JsonData)obj;
			}
			return new JsonData(obj);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000E0C4 File Offset: 0x0000C2C4
		private static void WriteJson(IJsonWrapper obj, JsonWriter writer)
		{
			if (obj == null)
			{
				writer.Write(string.Empty);
				return;
			}
			if (obj.IsString)
			{
				writer.Write(obj.GetString());
				return;
			}
			if (obj.IsBoolean)
			{
				writer.Write(obj.GetBoolean());
				return;
			}
			if (obj.IsDouble)
			{
				writer.Write(obj.GetDouble());
				return;
			}
			if (obj.IsInt)
			{
				writer.Write(obj.GetInt());
				return;
			}
			if (obj.IsLong)
			{
				writer.Write(obj.GetLong());
				return;
			}
			if (obj.IsArray)
			{
				writer.WriteArrayStart();
				writer.IndentValue++;
				foreach (object obj2 in obj)
				{
					JsonData.WriteJson((JsonData)obj2, writer);
				}
				writer.IndentValue--;
				writer.WriteArrayEnd();
				return;
			}
			if (obj.IsObject)
			{
				writer.WriteObjectStart();
				foreach (object obj3 in obj)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
					writer.WritePropertyName((string)dictionaryEntry.Key);
					JsonData.WriteJson((JsonData)dictionaryEntry.Value, writer);
				}
				writer.WriteObjectEnd();
				return;
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000E280 File Offset: 0x0000C480
		public int Add(object value)
		{
			JsonData jsonData = this.ToJsonData(value);
			this.json = null;
			return this.EnsureList().Add(jsonData);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000E2A8 File Offset: 0x0000C4A8
		public void Clear()
		{
			if (this.IsObject)
			{
				((IDictionary)this).Clear();
				return;
			}
			if (this.IsArray)
			{
				((IList)this).Clear();
				return;
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000E2DC File Offset: 0x0000C4DC
		public bool Equals(JsonData x)
		{
			if (x == null)
			{
				return false;
			}
			if (x.type != this.type)
			{
				return false;
			}
			switch (this.type)
			{
			case JsonType.None:
				return true;
			case JsonType.Object:
				return this.inst_object.Equals(x.inst_object);
			case JsonType.Array:
				return this.inst_array.Equals(x.inst_array);
			case JsonType.String:
				return this.inst_string.Equals(x.inst_string);
			case JsonType.Int:
				return this.inst_int.Equals(x.inst_int);
			case JsonType.Long:
				return this.inst_long.Equals(x.inst_long);
			case JsonType.Double:
				return this.inst_double.Equals(x.inst_double);
			case JsonType.Boolean:
				return this.inst_boolean.Equals(x.inst_boolean);
			default:
				return false;
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000E3B8 File Offset: 0x0000C5B8
		public JsonType GetJsonType()
		{
			return this.type;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000E3C0 File Offset: 0x0000C5C0
		public void SetJsonType(JsonType type)
		{
			if (this.type == type)
			{
				return;
			}
			switch (type)
			{
			case JsonType.Object:
				this.inst_object = new Dictionary<string, JsonData>();
				this.object_list = new List<KeyValuePair<string, JsonData>>();
				break;
			case JsonType.Array:
				this.inst_array = new List<JsonData>();
				break;
			case JsonType.String:
				this.inst_string = null;
				break;
			case JsonType.Int:
				this.inst_int = 0;
				break;
			case JsonType.Long:
				this.inst_long = 0L;
				break;
			case JsonType.Double:
				this.inst_double = 0.0;
				break;
			case JsonType.Boolean:
				this.inst_boolean = false;
				break;
			}
			this.type = type;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000E484 File Offset: 0x0000C684
		public string ToJson(bool forceRecalculate = false, bool doPrettyPrint = false)
		{
			if (this.json != null && !forceRecalculate)
			{
				return this.json;
			}
			StringWriter stringWriter = new StringWriter();
			JsonData.WriteJson(this, new JsonWriter(stringWriter)
			{
				Validate = false,
				PrettyPrint = doPrettyPrint
			});
			this.json = stringWriter.ToString();
			return this.json;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000E4E0 File Offset: 0x0000C6E0
		public void ToJson(JsonWriter writer)
		{
			bool validate = writer.Validate;
			writer.Validate = false;
			JsonData.WriteJson(this, writer);
			writer.Validate = validate;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000E50C File Offset: 0x0000C70C
		public override string ToString()
		{
			switch (this.type)
			{
			case JsonType.Object:
				return "JsonData object";
			case JsonType.Array:
				return "JsonData array";
			case JsonType.String:
				return this.inst_string;
			case JsonType.Int:
				return this.inst_int.ToString();
			case JsonType.Long:
				return this.inst_long.ToString();
			case JsonType.Double:
				return this.inst_double.ToString();
			case JsonType.Boolean:
				return this.inst_boolean.ToString();
			default:
				return "Uninitialized JsonData";
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000E594 File Offset: 0x0000C794
		public static implicit operator JsonData(bool data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000E59C File Offset: 0x0000C79C
		public static implicit operator JsonData(double data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000E5A4 File Offset: 0x0000C7A4
		public static implicit operator JsonData(float data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000E5AC File Offset: 0x0000C7AC
		public static implicit operator JsonData(int data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000E5B4 File Offset: 0x0000C7B4
		public static implicit operator JsonData(long data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000E5BC File Offset: 0x0000C7BC
		public static implicit operator JsonData(string data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000E5C4 File Offset: 0x0000C7C4
		public static implicit operator JsonData(List<JsonData> data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000E5CC File Offset: 0x0000C7CC
		public static implicit operator JsonData(Vector3 data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000E5DC File Offset: 0x0000C7DC
		public static explicit operator bool(JsonData data)
		{
			if (data.type != JsonType.Boolean)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a boolean");
			}
			return data.inst_boolean;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000E5FC File Offset: 0x0000C7FC
		public static explicit operator double(JsonData data)
		{
			if (data.type != JsonType.Double)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_double;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000E61C File Offset: 0x0000C81C
		public static explicit operator float(JsonData data)
		{
			if (data.type != JsonType.Double)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return (float)data.inst_double;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000E63C File Offset: 0x0000C83C
		public static explicit operator int(JsonData data)
		{
			if (data.type != JsonType.Int)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_int;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000E65C File Offset: 0x0000C85C
		public static explicit operator long(JsonData data)
		{
			if (data.type != JsonType.Long)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a long");
			}
			return data.inst_long;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000E67C File Offset: 0x0000C87C
		public static explicit operator string(JsonData data)
		{
			if (data.type != JsonType.String)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a string");
			}
			return data.inst_string;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000E69C File Offset: 0x0000C89C
		public static explicit operator List<JsonData>(JsonData data)
		{
			if (data.type != JsonType.Array)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an array");
			}
			return (List<JsonData>)data.inst_array;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000E6CC File Offset: 0x0000C8CC
		public static explicit operator Vector3(JsonData data)
		{
			if (data.type != JsonType.Array || data.inst_array.Count != 3 || data.inst_array[0].GetJsonType() != JsonType.Double || data.inst_array[1].GetJsonType() != JsonType.Double || data.inst_array[2].GetJsonType() != JsonType.Double)
			{
				Debug.LogError(data.inst_array.Count + ", " + data.inst_array[0].GetJsonType());
				throw new InvalidCastException("This instance of JsonData can't be cast to a Vector3");
			}
			return new Vector3((float)data.inst_array[0], (float)data.inst_array[1], (float)data.inst_array[2]);
		}

		// Token: 0x04000133 RID: 307
		private IList<JsonData> inst_array;

		// Token: 0x04000134 RID: 308
		private bool inst_boolean;

		// Token: 0x04000135 RID: 309
		private double inst_double;

		// Token: 0x04000136 RID: 310
		private int inst_int;

		// Token: 0x04000137 RID: 311
		private long inst_long;

		// Token: 0x04000138 RID: 312
		private IDictionary<string, JsonData> inst_object;

		// Token: 0x04000139 RID: 313
		private string inst_string;

		// Token: 0x0400013A RID: 314
		private string json;

		// Token: 0x0400013B RID: 315
		private JsonType type;

		// Token: 0x0400013C RID: 316
		private IList<KeyValuePair<string, JsonData>> object_list;
	}
}
