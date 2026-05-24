using System;
using UnityEngine;

// Token: 0x0200017A RID: 378
public class CloudGroupSettings : ScriptableObject
{
	// Token: 0x04000F16 RID: 3862
	public float baseAnimSpeed = 0.0024f;

	// Token: 0x04000F17 RID: 3863
	[Range(0f, 0.5f)]
	public float fadeDurationNorm = 0.4f;
}
