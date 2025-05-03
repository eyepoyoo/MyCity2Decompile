using System.Collections.Generic;

namespace GameDefines
{
	public class CurrencyDefines
	{
		public enum CurrencyType
		{
			NORMAL_1 = 0,
			NORMAL_2 = 1,
			NORMAL_3 = 2,
			PREMIUM = 3,
			CLICKS = 4
		}

		public static readonly CurrencyType[] _allTypes = new CurrencyType[5]
		{
			CurrencyType.NORMAL_1,
			CurrencyType.NORMAL_2,
			CurrencyType.NORMAL_3,
			CurrencyType.PREMIUM,
			CurrencyType.CLICKS
		};

		public static readonly Dictionary<CurrencyType, Currency> _currencies = new Dictionary<CurrencyType, Currency>
		{
			{
				CurrencyType.NORMAL_1,
				new Currency(CurrencyType.NORMAL_1)
			},
			{
				CurrencyType.NORMAL_2,
				new Currency(CurrencyType.NORMAL_2)
			},
			{
				CurrencyType.NORMAL_3,
				new Currency(CurrencyType.NORMAL_3)
			},
			{
				CurrencyType.PREMIUM,
				new Currency(CurrencyType.PREMIUM)
			},
			{
				CurrencyType.CLICKS,
				new Currency(CurrencyType.CLICKS)
			}
		};
	}
}
