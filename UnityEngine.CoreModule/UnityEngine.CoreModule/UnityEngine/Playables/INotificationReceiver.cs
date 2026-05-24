using System;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x02000433 RID: 1075
	[RequiredByNativeCode]
	public interface INotificationReceiver
	{
		// Token: 0x0600256D RID: 9581
		[RequiredByNativeCode]
		void OnNotify(Playable origin, INotification notification, object context);
	}
}
