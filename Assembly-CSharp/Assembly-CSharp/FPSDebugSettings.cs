using System;
using UnityEngine;

// Token: 0x020001C0 RID: 448
public class FPSDebugSettings : ScriptableObject
{
	// Token: 0x0400119F RID: 4511
	public float fpsGraphHistoryTime = 1f;

	// Token: 0x040011A0 RID: 4512
	public Rect fpsPos = new Rect(10f, 10f, 100f, 400f);

	// Token: 0x040011A1 RID: 4513
	public bool showInEditor = true;

	// Token: 0x040011A2 RID: 4514
	public bool showInDevBuilds = true;

	// Token: 0x040011A3 RID: 4515
	public bool showInReleaseBuilds;
}
