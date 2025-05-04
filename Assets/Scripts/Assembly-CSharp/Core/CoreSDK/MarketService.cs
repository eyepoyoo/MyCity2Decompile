using System;
using System.Collections.Generic;
using System.Linq;
using LEGO.CoreSDK.Native;
using Zenject;

namespace LEGO.CoreSDK
{
	// Token: 0x02000032 RID: 50
	internal class MarketService : IMarketService
	{
		// Token: 0x0600007A RID: 122 RVA: 0x0000393C File Offset: 0x00001B3C
		internal MarketService(ILocaleFactory localeFactory, INativeAPI nativeAPI, IEnumerable<string> supportedLocales, string fallbackLocale)
		{
			this._localeFactory = localeFactory;
			this._nativeAPI = nativeAPI;
			Asserts.Precondition(supportedLocales != null && supportedLocales.Count<string>() > 0, "The list of supported locales was not provided. Please make sure that the list is non-empty in the Initialize method of LEGOSDK.");
			string text = fallbackLocale ?? supportedLocales.First<string>();
			if (supportedLocales.Any((string x) => string.IsNullOrEmpty(x)))
			{
				throw new LocaleException("The list of supported locales contains a null or empty locale. Please make sure that all locales in the list is filled out in the form 'da_DK', i.e. 'LANGUAGE_REGION'.");
			}
			IEnumerable<string> enumerable = supportedLocales.Select((string x) => x.Replace('-', '_'));
			text = text.Replace('-', '_');
			if (!enumerable.Contains(text))
			{
				throw new LocaleException("The list of supported locales does not contain the provided fallback locale. Please make sure that it is present in the supported list.");
			}
			this.ResolvedMarket = this.ResolveCurrentLocale(enumerable, text);
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00003A1C File Offset: 0x00001C1C
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00003A10 File Offset: 0x00001C10
		public Locale ResolvedMarket { get; private set; }

		// Token: 0x0600007D RID: 125 RVA: 0x00003A24 File Offset: 0x00001C24
		private Locale ResolveCurrentLocale(IEnumerable<string> supportedLocales, string fallbackLocale)
		{
			IEnumerable<Locale> enumerable = supportedLocales.Select((string x) => this._localeFactory.Create(x));
			string deviceLocale = this._nativeAPI.GetDeviceLocale();
			if (deviceLocale == null)
			{
				throw new NoDeviceLocaleException("Failed to find a device locale");
			}
			Locale locale = this._localeFactory.Create(deviceLocale);
			Locale locale2 = this._localeFactory.Create(fallbackLocale);
			return MarketService.ResolvedLocale(enumerable, locale2, locale);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003A84 File Offset: 0x00001C84
		private static Locale ResolvedLocale(IEnumerable<Locale> supportedMarkets, Locale fallbackLocale, Locale existingLocale)
		{
			if (supportedMarkets.Any((Locale x) => x.Equals(existingLocale)))
			{
				return existingLocale;
			}
			Locale locale = supportedMarkets.FirstOrDefault((Locale x) => existingLocale.Region == x.Region);
			if (locale.Equals(default(Locale)))
			{
				return fallbackLocale;
			}
			return locale;
		}

		// Token: 0x04000051 RID: 81
		private ILocaleFactory _localeFactory;

		// Token: 0x04000052 RID: 82
		private INativeAPI _nativeAPI;

		// Token: 0x02000033 RID: 51
		public class Factory : Factory<IEnumerable<string>, string, MarketService>
		{
		}
	}
}
