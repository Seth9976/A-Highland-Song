using System;

namespace UnityEngine.Playables
{
	// Token: 0x02000444 RID: 1092
	public enum PlayState
	{
		// Token: 0x04000E22 RID: 3618
		Paused,
		// Token: 0x04000E23 RID: 3619
		Playing,
		// Token: 0x04000E24 RID: 3620
		[Obsolete("Delayed is obsolete; use a custom ScriptPlayable to implement this feature", false)]
		Delayed
	}
}
