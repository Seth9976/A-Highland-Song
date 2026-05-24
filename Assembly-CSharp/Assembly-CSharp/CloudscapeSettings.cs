using System;
using UnityEngine;

// Token: 0x02000048 RID: 72
public class CloudscapeSettings : ScriptableObject
{
	// Token: 0x040002F0 RID: 752
	public float crossFadeSpeed = 1f;

	// Token: 0x040002F1 RID: 753
	public float windEffectSpeed = 1f;

	// Token: 0x040002F2 RID: 754
	public CloudscapeTexture[] lightCloudTextures;

	// Token: 0x040002F3 RID: 755
	public CloudscapeTexture[] heavyCloudTextures;

	// Token: 0x040002F4 RID: 756
	public CloudscapeTexture[] stormTextures;
}
