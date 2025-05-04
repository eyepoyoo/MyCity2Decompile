using System;

namespace LEGO.CoreSDK.VPC
{
	// Token: 0x02000009 RID: 9
	public interface IParentalGate
	{
		// Token: 0x06000013 RID: 19
		void Guard(Action<Result> completionHandler);
	}
}
