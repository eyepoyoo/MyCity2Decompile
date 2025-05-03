using System;

namespace LEGO.CoreSDK.Legal
{
	// Token: 0x02000050 RID: 80
	public interface IPolicies
	{
		// Token: 0x060000E1 RID: 225
		IPolicyView Show(Action completionHandler, PolicyType? policyType = null);
	}
}
