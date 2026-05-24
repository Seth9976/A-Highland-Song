using System;
using UnityEngine;

// Token: 0x020000D0 RID: 208
public class BellyWriggleSettings : ScriptableObject
{
	// Token: 0x04000852 RID: 2130
	public float speed = 5f;

	// Token: 0x04000853 RID: 2131
	public float entryDist = 1f;

	// Token: 0x04000854 RID: 2132
	public float rotationSpeed = 20f;

	// Token: 0x04000855 RID: 2133
	public float transitionRotationSpeed = 90f;

	// Token: 0x04000856 RID: 2134
	public float stuckButTryMoveTriggerTime = 0.5f;

	// Token: 0x04000857 RID: 2135
	public float minTimeBetweenStuckInkCalls = 5f;
}
