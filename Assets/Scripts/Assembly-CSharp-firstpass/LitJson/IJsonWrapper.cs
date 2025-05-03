using System;
using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
	// Token: 0x02000020 RID: 32
	public interface IJsonWrapper : IDictionary, IList, ICollection, IEnumerable, IOrderedDictionary
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060001D5 RID: 469
		bool IsArray { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060001D6 RID: 470
		bool IsBoolean { get; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060001D7 RID: 471
		bool IsDouble { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060001D8 RID: 472
		bool IsInt { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060001D9 RID: 473
		bool IsLong { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060001DA RID: 474
		bool IsObject { get; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060001DB RID: 475
		bool IsString { get; }

		// Token: 0x060001DC RID: 476
		bool GetBoolean();

		// Token: 0x060001DD RID: 477
		double GetDouble();

		// Token: 0x060001DE RID: 478
		int GetInt();

		// Token: 0x060001DF RID: 479
		JsonType GetJsonType();

		// Token: 0x060001E0 RID: 480
		long GetLong();

		// Token: 0x060001E1 RID: 481
		string GetString();

		// Token: 0x060001E2 RID: 482
		void SetBoolean(bool val);

		// Token: 0x060001E3 RID: 483
		void SetDouble(double val);

		// Token: 0x060001E4 RID: 484
		void SetInt(int val);

		// Token: 0x060001E5 RID: 485
		void SetJsonType(JsonType type);

		// Token: 0x060001E6 RID: 486
		void SetLong(long val);

		// Token: 0x060001E7 RID: 487
		void SetString(string val);

		// Token: 0x060001E8 RID: 488
		string ToJson();

		// Token: 0x060001E9 RID: 489
		void ToJson(JsonWriter writer);
	}
}
