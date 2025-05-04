using System;
using System.Text.RegularExpressions;

namespace LEGO.CoreSDK
{
	// Token: 0x0200003B RID: 59
	public struct Locale
	{
		// Token: 0x06000095 RID: 149 RVA: 0x00003BE8 File Offset: 0x00001DE8
		internal Locale(string locale, string displayName)
		{
			if (string.IsNullOrEmpty(locale))
			{
				throw new LocaleException("The provided locale string was null or empty.");
			}
			Match match = Locale._localePattern.Match(locale);
			if (!match.Success)
			{
				throw new LocaleException("The locale '" + locale + "' could not be interpreted as a valid locale");
			}
			this.Language = match.Groups[1].Value.ToLower();
			this.Region = match.Groups[3].Value.ToUpper();
			this.FullLocale = this.Language + "_" + this.Region;
			this.DisplayName = displayName;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00003CA8 File Offset: 0x00001EA8
		// (set) Token: 0x06000098 RID: 152 RVA: 0x00003CB0 File Offset: 0x00001EB0
		public string Language { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00003CBC File Offset: 0x00001EBC
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00003CC4 File Offset: 0x00001EC4
		public string Region { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00003CD0 File Offset: 0x00001ED0
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00003CD8 File Offset: 0x00001ED8
		public string FullLocale { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00003CE4 File Offset: 0x00001EE4
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00003CEC File Offset: 0x00001EEC
		public string DisplayName { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00003CF8 File Offset: 0x00001EF8
		public string SiteCoreLocale
		{
			get
			{
				return this.FullLocale.Replace("_", "-");
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00003D10 File Offset: 0x00001F10
		public string Name
		{
			get
			{
				return this.DisplayName;
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003D18 File Offset: 0x00001F18
		public override string ToString()
		{
			return string.Concat(new string[] { this.Name, " (", this.Language, "_", this.Region, ")" });
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003D64 File Offset: 0x00001F64
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Locale))
			{
				return false;
			}
			Locale locale = (Locale)obj;
			return locale.Language == this.Language && locale.Region == this.Region;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003DB8 File Offset: 0x00001FB8
		public override int GetHashCode()
		{
			return this.FullLocale.GetHashCode();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003DC8 File Offset: 0x00001FC8
		public static bool operator ==(Locale c1, Locale c2)
		{
			return c1.Equals(c2);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003DD8 File Offset: 0x00001FD8
		public static bool operator !=(Locale c1, Locale c2)
		{
			return !c1.Equals(c2);
		}

		// Token: 0x0400005F RID: 95
		private const int NumberOfPartsInLocale = 2;

		// Token: 0x04000060 RID: 96
		private static readonly Regex _localePattern = new Regex("^([a-zA-Z]{2})[-_](.*[-_])?([a-zA-Z]{2})$");
	}
}
