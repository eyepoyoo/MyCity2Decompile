using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LEGO.CoreSDK.Native;

namespace LEGO.CoreSDK.Legal
{
	// Token: 0x0200004B RID: 75
	public class Policies : IPolicies
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x00005338 File Offset: 0x00003538
		public Policies(INativeAPI nativeAPI, ILocaleFactory localeFactory, IFilesystem fileSystem, IPolicyView policyView)
		{
			this._nativeAPI = nativeAPI;
			this._localeFactory = localeFactory;
			this._fileSystem = fileSystem;
			this._policyView = policyView;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005360 File Offset: 0x00003560
		public IPolicyView Show(Action completionHandler, PolicyType? policyType = null)
		{
			IEnumerable<Locale> enumerable = from x in this._fileSystem.ContentsOfBundledDirectory("Legal/Texts")
				select x.Name into x
				select this._localeFactory.Create(x);
			Locale locale = this._localeFactory.Create(this._nativeAPI.GetDeviceLocale());
			Locale locale2 = this.DetermineLocale(enumerable, locale, enumerable.First<Locale>());
			this._policyView.Show(delegate(Locale l)
			{
				string text = l.Language + "-" + l.Region.ToLower();
				string text2 = this._fileSystem.ReadAllTextOfBundledFile("Legal/Texts/" + text);
				if (text2 == null)
				{
					throw new FileNotFoundException("Failed to find legal policy for locale: '" + text + "'");
				}
				return new LegalPolicies(text2);
			}, locale2, enumerable, policyType, completionHandler);
			return this._policyView;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000053F8 File Offset: 0x000035F8
		private Locale DetermineLocale(IEnumerable<Locale> supportedLocales, Locale deviceLocale, Locale fallbackLocale)
		{
			if (supportedLocales.Contains(deviceLocale))
			{
				return deviceLocale;
			}
			Locale locale = supportedLocales.FirstOrDefault((Locale x) => x.Region == deviceLocale.Region);
			if (locale.Equals(default(Locale)))
			{
				return fallbackLocale;
			}
			return locale;
		}

		// Token: 0x0400009C RID: 156
		private INativeAPI _nativeAPI;

		// Token: 0x0400009D RID: 157
		private ILocaleFactory _localeFactory;

		// Token: 0x0400009E RID: 158
		private IFilesystem _fileSystem;

		// Token: 0x0400009F RID: 159
		private IPolicyView _policyView;
	}
}
