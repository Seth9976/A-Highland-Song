using System;

namespace Steamworks
{
	// Token: 0x02000181 RID: 385
	public abstract class CallResult
	{
		// Token: 0x060008B8 RID: 2232
		internal abstract Type GetCallbackType();

		// Token: 0x060008B9 RID: 2233
		internal abstract void OnRunCallResult(IntPtr pvParam, bool bFailed, ulong hSteamAPICall);

		// Token: 0x060008BA RID: 2234
		internal abstract void SetUnregistered();
	}
}
