using System;

namespace Prime31
{
	// Token: 0x02000008 RID: 8
	public static class DateTimeExtensions
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002E80 File Offset: 0x00001080
		public static long toEpochTime(this DateTime self)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return Convert.ToInt64((self - dateTime).TotalSeconds);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002EB4 File Offset: 0x000010B4
		public static DateTime fromEpochTime(this long unixTime)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return dateTime.AddSeconds((double)unixTime);
		}
	}
}
