using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

// Token: 0x02000052 RID: 82
public class JSONObject
{
	// Token: 0x060000E7 RID: 231 RVA: 0x000058D0 File Offset: 0x00003AD0
	public JSONObject(JSONObject.Type t)
	{
		this.type = t;
		if (t != JSONObject.Type.OBJECT)
		{
			if (t == JSONObject.Type.ARRAY)
			{
				this.list = new List<JSONObject>();
			}
		}
		else
		{
			this.list = new List<JSONObject>();
			this.keys = new List<string>();
		}
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x0000592C File Offset: 0x00003B2C
	public JSONObject(bool b)
	{
		this.type = JSONObject.Type.BOOL;
		this.b = b;
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x00005944 File Offset: 0x00003B44
	public JSONObject(float f)
	{
		this.type = JSONObject.Type.NUMBER;
		this.n = f;
	}

	// Token: 0x060000EA RID: 234 RVA: 0x0000595C File Offset: 0x00003B5C
	public JSONObject(int i)
	{
		this.type = JSONObject.Type.NUMBER;
		this.i = (long)i;
		this.useInt = true;
		this.n = (float)i;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00005990 File Offset: 0x00003B90
	public JSONObject(long l)
	{
		this.type = JSONObject.Type.NUMBER;
		this.i = l;
		this.useInt = true;
		this.n = (float)l;
	}

	// Token: 0x060000EC RID: 236 RVA: 0x000059B8 File Offset: 0x00003BB8
	public JSONObject(Dictionary<string, string> dic)
	{
		this.type = JSONObject.Type.OBJECT;
		this.keys = new List<string>();
		this.list = new List<JSONObject>();
		foreach (KeyValuePair<string, string> keyValuePair in dic)
		{
			this.keys.Add(keyValuePair.Key);
			this.list.Add(JSONObject.CreateStringObject(keyValuePair.Value));
		}
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00005A60 File Offset: 0x00003C60
	public JSONObject(Dictionary<string, JSONObject> dic)
	{
		this.type = JSONObject.Type.OBJECT;
		this.keys = new List<string>();
		this.list = new List<JSONObject>();
		foreach (KeyValuePair<string, JSONObject> keyValuePair in dic)
		{
			this.keys.Add(keyValuePair.Key);
			this.list.Add(keyValuePair.Value);
		}
	}

	// Token: 0x060000EE RID: 238 RVA: 0x00005B04 File Offset: 0x00003D04
	public JSONObject(JSONObject.AddJSONContents content)
	{
		content(this);
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00005B14 File Offset: 0x00003D14
	public JSONObject(JSONObject[] objs)
	{
		this.type = JSONObject.Type.ARRAY;
		this.list = new List<JSONObject>(objs);
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x00005B30 File Offset: 0x00003D30
	public JSONObject()
	{
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00005B38 File Offset: 0x00003D38
	public JSONObject(string str, int maxDepth = -2, bool storeExcessLevels = false, bool strict = false)
	{
		this.Parse(str, maxDepth, storeExcessLevels, strict);
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x060000F3 RID: 243 RVA: 0x00005B7C File Offset: 0x00003D7C
	public bool isContainer
	{
		get
		{
			return this.type == JSONObject.Type.ARRAY || this.type == JSONObject.Type.OBJECT;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x060000F4 RID: 244 RVA: 0x00005B98 File Offset: 0x00003D98
	public int Count
	{
		get
		{
			if (this.list == null)
			{
				return -1;
			}
			return this.list.Count;
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x060000F5 RID: 245 RVA: 0x00005BB4 File Offset: 0x00003DB4
	public float f
	{
		get
		{
			return this.n;
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x060000F6 RID: 246 RVA: 0x00005BBC File Offset: 0x00003DBC
	public static JSONObject nullJO
	{
		get
		{
			return JSONObject.Create(JSONObject.Type.NULL);
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x060000F7 RID: 247 RVA: 0x00005BC4 File Offset: 0x00003DC4
	public static JSONObject obj
	{
		get
		{
			return JSONObject.Create(JSONObject.Type.OBJECT);
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x060000F8 RID: 248 RVA: 0x00005BCC File Offset: 0x00003DCC
	public static JSONObject arr
	{
		get
		{
			return JSONObject.Create(JSONObject.Type.ARRAY);
		}
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00005BD4 File Offset: 0x00003DD4
	public static JSONObject StringObject(string val)
	{
		return JSONObject.CreateStringObject(val);
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00005BDC File Offset: 0x00003DDC
	public void Absorb(JSONObject obj)
	{
		this.list.AddRange(obj.list);
		this.keys.AddRange(obj.keys);
		this.str = obj.str;
		this.n = obj.n;
		this.useInt = obj.useInt;
		this.i = obj.i;
		this.b = obj.b;
		this.type = obj.type;
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00005C54 File Offset: 0x00003E54
	public static JSONObject Create()
	{
		return new JSONObject();
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00005C5C File Offset: 0x00003E5C
	public static JSONObject Create(JSONObject.Type t)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.type = t;
		if (t != JSONObject.Type.OBJECT)
		{
			if (t == JSONObject.Type.ARRAY)
			{
				jsonobject.list = new List<JSONObject>();
			}
		}
		else
		{
			jsonobject.list = new List<JSONObject>();
			jsonobject.keys = new List<string>();
		}
		return jsonobject;
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00005CB8 File Offset: 0x00003EB8
	public static JSONObject Create(bool val)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.type = JSONObject.Type.BOOL;
		jsonobject.b = val;
		return jsonobject;
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00005CDC File Offset: 0x00003EDC
	public static JSONObject Create(float val)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.type = JSONObject.Type.NUMBER;
		jsonobject.n = val;
		return jsonobject;
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00005D00 File Offset: 0x00003F00
	public static JSONObject Create(int val)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.type = JSONObject.Type.NUMBER;
		jsonobject.n = (float)val;
		jsonobject.useInt = true;
		jsonobject.i = (long)val;
		return jsonobject;
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00005D34 File Offset: 0x00003F34
	public static JSONObject Create(long val)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.type = JSONObject.Type.NUMBER;
		jsonobject.n = (float)val;
		jsonobject.useInt = true;
		jsonobject.i = val;
		return jsonobject;
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00005D68 File Offset: 0x00003F68
	public static JSONObject CreateStringObject(string val)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.type = JSONObject.Type.STRING;
		jsonobject.str = val;
		return jsonobject;
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00005D8C File Offset: 0x00003F8C
	public static JSONObject CreateBakedObject(string val)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.type = JSONObject.Type.BAKED;
		jsonobject.str = val;
		return jsonobject;
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00005DB0 File Offset: 0x00003FB0
	public static JSONObject Create(string val, int maxDepth = -2, bool storeExcessLevels = false, bool strict = false)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.Parse(val, maxDepth, storeExcessLevels, strict);
		return jsonobject;
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00005DD0 File Offset: 0x00003FD0
	public static JSONObject Create(JSONObject.AddJSONContents content)
	{
		JSONObject jsonobject = JSONObject.Create();
		content(jsonobject);
		return jsonobject;
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00005DEC File Offset: 0x00003FEC
	public static JSONObject Create(Dictionary<string, string> dic)
	{
		JSONObject jsonobject = JSONObject.Create();
		jsonobject.type = JSONObject.Type.OBJECT;
		jsonobject.keys = new List<string>();
		jsonobject.list = new List<JSONObject>();
		foreach (KeyValuePair<string, string> keyValuePair in dic)
		{
			jsonobject.keys.Add(keyValuePair.Key);
			jsonobject.list.Add(JSONObject.CreateStringObject(keyValuePair.Value));
		}
		return jsonobject;
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00005E94 File Offset: 0x00004094
	private void Parse(string str, int maxDepth = -2, bool storeExcessLevels = false, bool strict = false)
	{
		if (!string.IsNullOrEmpty(str))
		{
			str = str.Trim(JSONObject.WHITESPACE);
			if (strict && str[0] != '[' && str[0] != '{')
			{
				this.type = JSONObject.Type.NULL;
				return;
			}
			if (str.Length > 0)
			{
				if (string.Compare(str, "true", true) == 0)
				{
					this.type = JSONObject.Type.BOOL;
					this.b = true;
				}
				else if (string.Compare(str, "false", true) == 0)
				{
					this.type = JSONObject.Type.BOOL;
					this.b = false;
				}
				else if (string.Compare(str, "null", true) == 0)
				{
					this.type = JSONObject.Type.NULL;
				}
				else if (str == "\"INFINITY\"")
				{
					this.type = JSONObject.Type.NUMBER;
					this.n = float.PositiveInfinity;
				}
				else if (str == "\"NEGINFINITY\"")
				{
					this.type = JSONObject.Type.NUMBER;
					this.n = float.NegativeInfinity;
				}
				else if (str == "\"NaN\"")
				{
					this.type = JSONObject.Type.NUMBER;
					this.n = float.NaN;
				}
				else if (str[0] == '"')
				{
					this.type = JSONObject.Type.STRING;
					this.str = str.Substring(1, str.Length - 2);
				}
				else
				{
					int num = 1;
					int num2 = 0;
					char c = str[num2];
					if (c != '[')
					{
						if (c != '{')
						{
							try
							{
								this.n = Convert.ToSingle(str);
								if (!str.Contains("."))
								{
									this.i = Convert.ToInt64(str);
									this.useInt = true;
								}
								this.type = JSONObject.Type.NUMBER;
							}
							catch (FormatException)
							{
								this.type = JSONObject.Type.NULL;
							}
							return;
						}
						this.type = JSONObject.Type.OBJECT;
						this.keys = new List<string>();
						this.list = new List<JSONObject>();
					}
					else
					{
						this.type = JSONObject.Type.ARRAY;
						this.list = new List<JSONObject>();
					}
					string text = string.Empty;
					bool flag = false;
					bool flag2 = false;
					int num3 = 0;
					while (++num2 < str.Length)
					{
						if (Array.IndexOf<char>(JSONObject.WHITESPACE, str[num2]) <= -1)
						{
							if (str[num2] == '\\')
							{
								num2++;
							}
							else
							{
								if (str[num2] == '"')
								{
									if (flag)
									{
										if (!flag2 && num3 == 0 && this.type == JSONObject.Type.OBJECT)
										{
											text = str.Substring(num + 1, num2 - num - 1);
										}
										flag = false;
									}
									else
									{
										if (num3 == 0 && this.type == JSONObject.Type.OBJECT)
										{
											num = num2;
										}
										flag = true;
									}
								}
								if (!flag)
								{
									if (this.type == JSONObject.Type.OBJECT && num3 == 0 && str[num2] == ':')
									{
										num = num2 + 1;
										flag2 = true;
									}
									if (str[num2] == '[' || str[num2] == '{')
									{
										num3++;
									}
									else if (str[num2] == ']' || str[num2] == '}')
									{
										num3--;
									}
									if ((str[num2] == ',' && num3 == 0) || num3 < 0)
									{
										flag2 = false;
										string text2 = str.Substring(num, num2 - num).Trim(JSONObject.WHITESPACE);
										if (text2.Length > 0)
										{
											if (this.type == JSONObject.Type.OBJECT)
											{
												this.keys.Add(text);
											}
											if (maxDepth != -1)
											{
												this.list.Add(JSONObject.Create(text2, (maxDepth >= -1) ? (maxDepth - 1) : (-2), false, false));
											}
											else if (storeExcessLevels)
											{
												this.list.Add(JSONObject.CreateBakedObject(text2));
											}
										}
										num = num2 + 1;
									}
								}
							}
						}
					}
				}
			}
			else
			{
				this.type = JSONObject.Type.NULL;
			}
		}
		else
		{
			this.type = JSONObject.Type.NULL;
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000107 RID: 263 RVA: 0x000062A8 File Offset: 0x000044A8
	public bool IsNumber
	{
		get
		{
			return this.type == JSONObject.Type.NUMBER;
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000108 RID: 264 RVA: 0x000062B4 File Offset: 0x000044B4
	public bool IsNull
	{
		get
		{
			return this.type == JSONObject.Type.NULL;
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x06000109 RID: 265 RVA: 0x000062C0 File Offset: 0x000044C0
	public bool IsString
	{
		get
		{
			return this.type == JSONObject.Type.STRING;
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x0600010A RID: 266 RVA: 0x000062CC File Offset: 0x000044CC
	public bool IsBool
	{
		get
		{
			return this.type == JSONObject.Type.BOOL;
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x0600010B RID: 267 RVA: 0x000062D8 File Offset: 0x000044D8
	public bool IsArray
	{
		get
		{
			return this.type == JSONObject.Type.ARRAY;
		}
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x0600010C RID: 268 RVA: 0x000062E4 File Offset: 0x000044E4
	public bool IsObject
	{
		get
		{
			return this.type == JSONObject.Type.OBJECT || this.type == JSONObject.Type.BAKED;
		}
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00006300 File Offset: 0x00004500
	public void Add(bool val)
	{
		this.Add(JSONObject.Create(val));
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00006310 File Offset: 0x00004510
	public void Add(float val)
	{
		this.Add(JSONObject.Create(val));
	}

	// Token: 0x0600010F RID: 271 RVA: 0x00006320 File Offset: 0x00004520
	public void Add(int val)
	{
		this.Add(JSONObject.Create(val));
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00006330 File Offset: 0x00004530
	public void Add(string str)
	{
		this.Add(JSONObject.CreateStringObject(str));
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00006340 File Offset: 0x00004540
	public void Add(JSONObject.AddJSONContents content)
	{
		this.Add(JSONObject.Create(content));
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00006350 File Offset: 0x00004550
	public void Add(JSONObject obj)
	{
		if (obj)
		{
			if (this.type != JSONObject.Type.ARRAY)
			{
				this.type = JSONObject.Type.ARRAY;
				if (this.list == null)
				{
					this.list = new List<JSONObject>();
				}
			}
			this.list.Add(obj);
		}
	}

	// Token: 0x06000113 RID: 275 RVA: 0x000063A0 File Offset: 0x000045A0
	public void AddField(string name, bool val)
	{
		this.AddField(name, JSONObject.Create(val));
	}

	// Token: 0x06000114 RID: 276 RVA: 0x000063B0 File Offset: 0x000045B0
	public void AddField(string name, float val)
	{
		this.AddField(name, JSONObject.Create(val));
	}

	// Token: 0x06000115 RID: 277 RVA: 0x000063C0 File Offset: 0x000045C0
	public void AddField(string name, int val)
	{
		this.AddField(name, JSONObject.Create(val));
	}

	// Token: 0x06000116 RID: 278 RVA: 0x000063D0 File Offset: 0x000045D0
	public void AddField(string name, long val)
	{
		this.AddField(name, JSONObject.Create(val));
	}

	// Token: 0x06000117 RID: 279 RVA: 0x000063E0 File Offset: 0x000045E0
	public void AddField(string name, JSONObject.AddJSONContents content)
	{
		this.AddField(name, JSONObject.Create(content));
	}

	// Token: 0x06000118 RID: 280 RVA: 0x000063F0 File Offset: 0x000045F0
	public void AddField(string name, string val)
	{
		this.AddField(name, JSONObject.CreateStringObject(val));
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00006400 File Offset: 0x00004600
	public void AddField(string name, JSONObject obj)
	{
		if (obj)
		{
			if (this.type != JSONObject.Type.OBJECT)
			{
				if (this.keys == null)
				{
					this.keys = new List<string>();
				}
				if (this.type == JSONObject.Type.ARRAY)
				{
					for (int i = 0; i < this.list.Count; i++)
					{
						this.keys.Add(i + string.Empty);
					}
				}
				else if (this.list == null)
				{
					this.list = new List<JSONObject>();
				}
				this.type = JSONObject.Type.OBJECT;
			}
			this.keys.Add(name);
			this.list.Add(obj);
		}
	}

	// Token: 0x0600011A RID: 282 RVA: 0x000064B8 File Offset: 0x000046B8
	public void SetField(string name, string val)
	{
		this.SetField(name, JSONObject.CreateStringObject(val));
	}

	// Token: 0x0600011B RID: 283 RVA: 0x000064C8 File Offset: 0x000046C8
	public void SetField(string name, bool val)
	{
		this.SetField(name, JSONObject.Create(val));
	}

	// Token: 0x0600011C RID: 284 RVA: 0x000064D8 File Offset: 0x000046D8
	public void SetField(string name, float val)
	{
		this.SetField(name, JSONObject.Create(val));
	}

	// Token: 0x0600011D RID: 285 RVA: 0x000064E8 File Offset: 0x000046E8
	public void SetField(string name, int val)
	{
		this.SetField(name, JSONObject.Create(val));
	}

	// Token: 0x0600011E RID: 286 RVA: 0x000064F8 File Offset: 0x000046F8
	public void SetField(string name, JSONObject obj)
	{
		if (this.HasField(name))
		{
			this.list.Remove(this[name]);
			this.keys.Remove(name);
		}
		this.AddField(name, obj);
	}

	// Token: 0x0600011F RID: 287 RVA: 0x0000653C File Offset: 0x0000473C
	public void RemoveField(string name)
	{
		if (this.keys.IndexOf(name) > -1)
		{
			this.list.RemoveAt(this.keys.IndexOf(name));
			this.keys.Remove(name);
		}
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00006580 File Offset: 0x00004780
	public bool GetField(out bool field, string name, bool fallback)
	{
		field = fallback;
		return this.GetField(ref field, name, null);
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00006590 File Offset: 0x00004790
	public bool GetField(ref bool field, string name, JSONObject.FieldNotFound fail = null)
	{
		if (this.type == JSONObject.Type.OBJECT)
		{
			int num = this.keys.IndexOf(name);
			if (num >= 0)
			{
				field = this.list[num].b;
				return true;
			}
		}
		if (fail != null)
		{
			fail(name);
		}
		return false;
	}

	// Token: 0x06000122 RID: 290 RVA: 0x000065E0 File Offset: 0x000047E0
	public bool GetField(out float field, string name, float fallback)
	{
		field = fallback;
		return this.GetField(ref field, name, null);
	}

	// Token: 0x06000123 RID: 291 RVA: 0x000065F0 File Offset: 0x000047F0
	public bool GetField(ref float field, string name, JSONObject.FieldNotFound fail = null)
	{
		if (this.type == JSONObject.Type.OBJECT)
		{
			int num = this.keys.IndexOf(name);
			if (num >= 0)
			{
				field = this.list[num].n;
				return true;
			}
		}
		if (fail != null)
		{
			fail(name);
		}
		return false;
	}

	// Token: 0x06000124 RID: 292 RVA: 0x00006640 File Offset: 0x00004840
	public bool GetField(out int field, string name, int fallback)
	{
		field = fallback;
		return this.GetField(ref field, name, null);
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00006650 File Offset: 0x00004850
	public bool GetField(ref int field, string name, JSONObject.FieldNotFound fail = null)
	{
		if (this.IsObject)
		{
			int num = this.keys.IndexOf(name);
			if (num >= 0)
			{
				field = (int)this.list[num].n;
				return true;
			}
		}
		if (fail != null)
		{
			fail(name);
		}
		return false;
	}

	// Token: 0x06000126 RID: 294 RVA: 0x000066A0 File Offset: 0x000048A0
	public bool GetField(out long field, string name, long fallback)
	{
		field = fallback;
		return this.GetField(ref field, name, null);
	}

	// Token: 0x06000127 RID: 295 RVA: 0x000066B0 File Offset: 0x000048B0
	public bool GetField(ref long field, string name, JSONObject.FieldNotFound fail = null)
	{
		if (this.IsObject)
		{
			int num = this.keys.IndexOf(name);
			if (num >= 0)
			{
				field = (long)this.list[num].n;
				return true;
			}
		}
		if (fail != null)
		{
			fail(name);
		}
		return false;
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00006700 File Offset: 0x00004900
	public bool GetField(out uint field, string name, uint fallback)
	{
		field = fallback;
		return this.GetField(ref field, name, null);
	}

	// Token: 0x06000129 RID: 297 RVA: 0x00006710 File Offset: 0x00004910
	public bool GetField(ref uint field, string name, JSONObject.FieldNotFound fail = null)
	{
		if (this.IsObject)
		{
			int num = this.keys.IndexOf(name);
			if (num >= 0)
			{
				field = (uint)this.list[num].n;
				return true;
			}
		}
		if (fail != null)
		{
			fail(name);
		}
		return false;
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00006760 File Offset: 0x00004960
	public bool GetField(out string field, string name, string fallback)
	{
		field = fallback;
		return this.GetField(ref field, name, null);
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00006770 File Offset: 0x00004970
	public bool GetField(ref string field, string name, JSONObject.FieldNotFound fail = null)
	{
		if (this.IsObject)
		{
			int num = this.keys.IndexOf(name);
			if (num >= 0)
			{
				field = this.list[num].str;
				return true;
			}
		}
		if (fail != null)
		{
			fail(name);
		}
		return false;
	}

	// Token: 0x0600012C RID: 300 RVA: 0x000067C0 File Offset: 0x000049C0
	public void GetField(string name, JSONObject.GetFieldResponse response, JSONObject.FieldNotFound fail = null)
	{
		if (response != null && this.IsObject)
		{
			int num = this.keys.IndexOf(name);
			if (num >= 0)
			{
				response(this.list[num]);
				return;
			}
		}
		if (fail != null)
		{
			fail(name);
		}
	}

	// Token: 0x0600012D RID: 301 RVA: 0x00006814 File Offset: 0x00004A14
	public JSONObject GetField(string name)
	{
		if (this.IsObject)
		{
			for (int i = 0; i < this.keys.Count; i++)
			{
				if (this.keys[i] == name)
				{
					return this.list[i];
				}
			}
		}
		return null;
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00006870 File Offset: 0x00004A70
	public bool HasFields(string[] names)
	{
		if (!this.IsObject)
		{
			return false;
		}
		for (int i = 0; i < names.Length; i++)
		{
			if (!this.keys.Contains(names[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600012F RID: 303 RVA: 0x000068B4 File Offset: 0x00004AB4
	public bool HasField(string name)
	{
		if (!this.IsObject)
		{
			return false;
		}
		for (int i = 0; i < this.keys.Count; i++)
		{
			if (this.keys[i] == name)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00006904 File Offset: 0x00004B04
	public void Clear()
	{
		this.type = JSONObject.Type.NULL;
		if (this.list != null)
		{
			this.list.Clear();
		}
		if (this.keys != null)
		{
			this.keys.Clear();
		}
		this.str = string.Empty;
		this.n = 0f;
		this.b = false;
	}

	// Token: 0x06000131 RID: 305 RVA: 0x00006964 File Offset: 0x00004B64
	public JSONObject Copy()
	{
		return JSONObject.Create(this.Print(false), -2, false, false);
	}

	// Token: 0x06000132 RID: 306 RVA: 0x00006978 File Offset: 0x00004B78
	public void Merge(JSONObject obj)
	{
		JSONObject.MergeRecur(this, obj);
	}

	// Token: 0x06000133 RID: 307 RVA: 0x00006984 File Offset: 0x00004B84
	private static void MergeRecur(JSONObject left, JSONObject right)
	{
		if (left.type == JSONObject.Type.NULL)
		{
			left.Absorb(right);
		}
		else if (left.type == JSONObject.Type.OBJECT && right.type == JSONObject.Type.OBJECT)
		{
			for (int i = 0; i < right.list.Count; i++)
			{
				string text = right.keys[i];
				if (right[i].isContainer)
				{
					if (left.HasField(text))
					{
						JSONObject.MergeRecur(left[text], right[i]);
					}
					else
					{
						left.AddField(text, right[i]);
					}
				}
				else if (left.HasField(text))
				{
					left.SetField(text, right[i]);
				}
				else
				{
					left.AddField(text, right[i]);
				}
			}
		}
		else if (left.type == JSONObject.Type.ARRAY && right.type == JSONObject.Type.ARRAY)
		{
			if (right.Count > left.Count)
			{
				return;
			}
			for (int j = 0; j < right.list.Count; j++)
			{
				if (left[j].type == right[j].type)
				{
					if (left[j].isContainer)
					{
						JSONObject.MergeRecur(left[j], right[j]);
					}
					else
					{
						left[j] = right[j];
					}
				}
			}
		}
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00006B00 File Offset: 0x00004D00
	public void Bake()
	{
		if (this.type != JSONObject.Type.BAKED)
		{
			this.str = this.Print(false);
			this.type = JSONObject.Type.BAKED;
		}
	}

	// Token: 0x06000135 RID: 309 RVA: 0x00006B30 File Offset: 0x00004D30
	public IEnumerable BakeAsync()
	{
		if (this.type != JSONObject.Type.BAKED)
		{
			foreach (string s in this.PrintAsync(false))
			{
				if (s == null)
				{
					yield return s;
				}
				else
				{
					this.str = s;
				}
			}
			this.type = JSONObject.Type.BAKED;
		}
		yield break;
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00006B54 File Offset: 0x00004D54
	public string Print(bool pretty = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		this.Stringify(0, stringBuilder, pretty);
		return stringBuilder.ToString();
	}

	// Token: 0x06000137 RID: 311 RVA: 0x00006B78 File Offset: 0x00004D78
	public IEnumerable<string> PrintAsync(bool pretty = false)
	{
		StringBuilder builder = new StringBuilder();
		JSONObject.printWatch.Reset();
		JSONObject.printWatch.Start();
		foreach (object obj in this.StringifyAsync(0, builder, pretty))
		{
			IEnumerable e = (IEnumerable)obj;
			yield return null;
		}
		yield return builder.ToString();
		yield break;
	}

	// Token: 0x06000138 RID: 312 RVA: 0x00006BAC File Offset: 0x00004DAC
	private IEnumerable StringifyAsync(int depth, StringBuilder builder, bool pretty = false)
	{
		int num;
		depth = (num = depth) + 1;
		if (num > 100)
		{
			yield break;
		}
		if (JSONObject.printWatch.Elapsed.TotalSeconds > 0.00800000037997961)
		{
			JSONObject.printWatch.Reset();
			yield return null;
			JSONObject.printWatch.Start();
		}
		switch (this.type)
		{
		case JSONObject.Type.NULL:
			builder.Append("null");
			break;
		case JSONObject.Type.STRING:
			builder.AppendFormat("\"{0}\"", this.str);
			break;
		case JSONObject.Type.NUMBER:
			if (this.useInt)
			{
				builder.Append(this.i.ToString());
			}
			else if (float.IsInfinity(this.n))
			{
				builder.Append("\"INFINITY\"");
			}
			else if (float.IsNegativeInfinity(this.n))
			{
				builder.Append("\"NEGINFINITY\"");
			}
			else if (float.IsNaN(this.n))
			{
				builder.Append("\"NaN\"");
			}
			else
			{
				builder.Append(this.n.ToString());
			}
			break;
		case JSONObject.Type.OBJECT:
			builder.Append("{");
			if (this.list.Count > 0)
			{
				if (pretty)
				{
					builder.Append("\r\n");
				}
				for (int i = 0; i < this.list.Count; i++)
				{
					string key = this.keys[i];
					JSONObject obj = this.list[i];
					if (obj)
					{
						if (pretty)
						{
							for (int j = 0; j < depth; j++)
							{
								builder.Append("\t");
							}
						}
						builder.AppendFormat("\"{0}\":", key);
						foreach (object obj2 in obj.StringifyAsync(depth, builder, pretty))
						{
							IEnumerable e = (IEnumerable)obj2;
							yield return e;
						}
						builder.Append(",");
						if (pretty)
						{
							builder.Append("\r\n");
						}
					}
				}
				if (pretty)
				{
					builder.Length -= 2;
				}
				else
				{
					builder.Length--;
				}
			}
			if (pretty && this.list.Count > 0)
			{
				builder.Append("\r\n");
				for (int k = 0; k < depth - 1; k++)
				{
					builder.Append("\t");
				}
			}
			builder.Append("}");
			break;
		case JSONObject.Type.ARRAY:
			builder.Append("[");
			if (this.list.Count > 0)
			{
				if (pretty)
				{
					builder.Append("\r\n");
				}
				for (int l = 0; l < this.list.Count; l++)
				{
					if (this.list[l])
					{
						if (pretty)
						{
							for (int m = 0; m < depth; m++)
							{
								builder.Append("\t");
							}
						}
						foreach (object obj3 in this.list[l].StringifyAsync(depth, builder, pretty))
						{
							IEnumerable e2 = (IEnumerable)obj3;
							yield return e2;
						}
						builder.Append(",");
						if (pretty)
						{
							builder.Append("\r\n");
						}
					}
				}
				if (pretty)
				{
					builder.Length -= 2;
				}
				else
				{
					builder.Length--;
				}
			}
			if (pretty && this.list.Count > 0)
			{
				builder.Append("\r\n");
				for (int n = 0; n < depth - 1; n++)
				{
					builder.Append("\t");
				}
			}
			builder.Append("]");
			break;
		case JSONObject.Type.BOOL:
			if (this.b)
			{
				builder.Append("true");
			}
			else
			{
				builder.Append("false");
			}
			break;
		case JSONObject.Type.BAKED:
			builder.Append(this.str);
			break;
		}
		yield break;
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00006BFC File Offset: 0x00004DFC
	private void Stringify(int depth, StringBuilder builder, bool pretty = false)
	{
		if (depth++ > 100)
		{
			return;
		}
		switch (this.type)
		{
		case JSONObject.Type.NULL:
			builder.Append("null");
			break;
		case JSONObject.Type.STRING:
			builder.AppendFormat("\"{0}\"", this.str);
			break;
		case JSONObject.Type.NUMBER:
			if (this.useInt)
			{
				builder.Append(this.i.ToString());
			}
			else if (float.IsInfinity(this.n))
			{
				builder.Append("\"INFINITY\"");
			}
			else if (float.IsNegativeInfinity(this.n))
			{
				builder.Append("\"NEGINFINITY\"");
			}
			else if (float.IsNaN(this.n))
			{
				builder.Append("\"NaN\"");
			}
			else
			{
				builder.Append(this.n.ToString());
			}
			break;
		case JSONObject.Type.OBJECT:
			builder.Append("{");
			if (this.list.Count > 0)
			{
				if (pretty)
				{
					builder.Append("\n");
				}
				for (int i = 0; i < this.list.Count; i++)
				{
					string text = this.keys[i];
					JSONObject jsonobject = this.list[i];
					if (jsonobject)
					{
						if (pretty)
						{
							for (int j = 0; j < depth; j++)
							{
								builder.Append("\t");
							}
						}
						builder.AppendFormat("\"{0}\":", text);
						jsonobject.Stringify(depth, builder, pretty);
						builder.Append(",");
						if (pretty)
						{
							builder.Append("\n");
						}
					}
				}
				if (pretty)
				{
					builder.Length -= 2;
				}
				else
				{
					builder.Length--;
				}
			}
			if (pretty && this.list.Count > 0)
			{
				builder.Append("\n");
				for (int k = 0; k < depth - 1; k++)
				{
					builder.Append("\t");
				}
			}
			builder.Append("}");
			break;
		case JSONObject.Type.ARRAY:
			builder.Append("[");
			if (this.list.Count > 0)
			{
				if (pretty)
				{
					builder.Append("\n");
				}
				for (int l = 0; l < this.list.Count; l++)
				{
					if (this.list[l])
					{
						if (pretty)
						{
							for (int m = 0; m < depth; m++)
							{
								builder.Append("\t");
							}
						}
						this.list[l].Stringify(depth, builder, pretty);
						builder.Append(",");
						if (pretty)
						{
							builder.Append("\n");
						}
					}
				}
				if (pretty)
				{
					builder.Length -= 2;
				}
				else
				{
					builder.Length--;
				}
			}
			if (pretty && this.list.Count > 0)
			{
				builder.Append("\n");
				for (int n = 0; n < depth - 1; n++)
				{
					builder.Append("\t");
				}
			}
			builder.Append("]");
			break;
		case JSONObject.Type.BOOL:
			if (this.b)
			{
				builder.Append("true");
			}
			else
			{
				builder.Append("false");
			}
			break;
		case JSONObject.Type.BAKED:
			builder.Append(this.str);
			break;
		}
	}

	// Token: 0x17000023 RID: 35
	public JSONObject this[int index]
	{
		get
		{
			if (this.list.Count > index)
			{
				return this.list[index];
			}
			return null;
		}
		set
		{
			if (this.list.Count > index)
			{
				this.list[index] = value;
			}
		}
	}

	// Token: 0x17000024 RID: 36
	public JSONObject this[string index]
	{
		get
		{
			return this.GetField(index);
		}
		set
		{
			this.SetField(index, value);
		}
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00007030 File Offset: 0x00005230
	public override string ToString()
	{
		return this.Print(false);
	}

	// Token: 0x0600013F RID: 319 RVA: 0x0000703C File Offset: 0x0000523C
	public string ToString(bool pretty)
	{
		return this.Print(pretty);
	}

	// Token: 0x06000140 RID: 320 RVA: 0x00007048 File Offset: 0x00005248
	public Dictionary<string, string> ToDictionary()
	{
		if (this.type == JSONObject.Type.OBJECT)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			int i = 0;
			while (i < this.list.Count)
			{
				JSONObject jsonobject = this.list[i];
				switch (jsonobject.type)
				{
				case JSONObject.Type.STRING:
					dictionary.Add(this.keys[i], jsonobject.str);
					break;
				case JSONObject.Type.NUMBER:
					dictionary.Add(this.keys[i], jsonobject.n + string.Empty);
					break;
				case JSONObject.Type.BOOL:
					dictionary.Add(this.keys[i], jsonobject.b + string.Empty);
					break;
				}
				IL_00C8:
				i++;
				continue;
				goto IL_00C8;
			}
			return dictionary;
		}
		return null;
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00007138 File Offset: 0x00005338
	public static implicit operator bool(JSONObject o)
	{
		return o != null;
	}

	// Token: 0x040000AC RID: 172
	private const int MAX_DEPTH = 100;

	// Token: 0x040000AD RID: 173
	private const string INFINITY = "\"INFINITY\"";

	// Token: 0x040000AE RID: 174
	private const string NEGINFINITY = "\"NEGINFINITY\"";

	// Token: 0x040000AF RID: 175
	private const string NaN = "\"NaN\"";

	// Token: 0x040000B0 RID: 176
	private const string NEWLINE = "\r\n";

	// Token: 0x040000B1 RID: 177
	private const float maxFrameTime = 0.008f;

	// Token: 0x040000B2 RID: 178
	public static readonly char[] WHITESPACE = new char[] { ' ', '\r', '\n', '\t', '\ufeff', '\t' };

	// Token: 0x040000B3 RID: 179
	public JSONObject.Type type;

	// Token: 0x040000B4 RID: 180
	public List<JSONObject> list;

	// Token: 0x040000B5 RID: 181
	public List<string> keys;

	// Token: 0x040000B6 RID: 182
	public string str;

	// Token: 0x040000B7 RID: 183
	public float n;

	// Token: 0x040000B8 RID: 184
	public bool useInt;

	// Token: 0x040000B9 RID: 185
	public long i;

	// Token: 0x040000BA RID: 186
	public bool b;

	// Token: 0x040000BB RID: 187
	private static readonly Stopwatch printWatch = new Stopwatch();

	// Token: 0x02000053 RID: 83
	public enum Type
	{
		// Token: 0x040000BD RID: 189
		NULL,
		// Token: 0x040000BE RID: 190
		STRING,
		// Token: 0x040000BF RID: 191
		NUMBER,
		// Token: 0x040000C0 RID: 192
		OBJECT,
		// Token: 0x040000C1 RID: 193
		ARRAY,
		// Token: 0x040000C2 RID: 194
		BOOL,
		// Token: 0x040000C3 RID: 195
		BAKED
	}

	// Token: 0x02000083 RID: 131
	// (Invoke) Token: 0x06000210 RID: 528
	public delegate void AddJSONContents(JSONObject self);

	// Token: 0x02000084 RID: 132
	// (Invoke) Token: 0x06000214 RID: 532
	public delegate void FieldNotFound(string name);

	// Token: 0x02000085 RID: 133
	// (Invoke) Token: 0x06000218 RID: 536
	public delegate void GetFieldResponse(JSONObject obj);
}
