using System;
using System.Collections.Generic;

namespace LEGO.CoreSDK.Legal
{
	// Token: 0x0200004F RID: 79
	public interface IPolicyView
	{
		// Token: 0x060000DF RID: 223
		void Show(Func<Locale, LegalPolicies> getPolicies, Locale currentLocale, IEnumerable<Locale> supportedLocales, PolicyType? policyType, Action completionHandler);

		// Token: 0x060000E0 RID: 224
		void Close();
	}
}
