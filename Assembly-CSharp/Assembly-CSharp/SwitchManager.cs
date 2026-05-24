using System;
using UnityEngine;

// Token: 0x020000A7 RID: 167
public class SwitchManager : MonoSingleton<SwitchManager>
{
	// Token: 0x06000574 RID: 1396 RVA: 0x0002B7F3 File Offset: 0x000299F3
	private void Awake()
	{
		if (MonoSingleton<SwitchManager>.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		base.gameObject.transform.SetParent(null);
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x0400063B RID: 1595
	private static bool _hasSetUpMount;
}
