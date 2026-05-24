using System;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class WaterCameraSettings : ScriptableObject
{
	// Token: 0x040003CB RID: 971
	[Info("Size of the render texture. This affects GPU performance.")]
	public float resolutionScaleDesktop = 0.5f;

	// Token: 0x040003CC RID: 972
	public float resolutionScaleSwitch = 0.3f;
}
