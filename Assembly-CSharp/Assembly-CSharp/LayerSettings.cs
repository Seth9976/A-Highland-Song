using System;
using UnityEngine;

// Token: 0x020000D6 RID: 214
public class LayerSettings : ScriptableObject
{
	// Token: 0x040008C4 RID: 2244
	public Range layerCollideNearbyRange = new Range(-1.5f, 1.5f);

	// Token: 0x040008C5 RID: 2245
	public float layerDropDownMin = -2.5f;

	// Token: 0x040008C6 RID: 2246
	public float layerCollideSafetyMin = -3.5f;

	// Token: 0x040008C7 RID: 2247
	public Range layerCollideCurrentRange = new Range(-1.5f, 0.5f);

	// Token: 0x040008C8 RID: 2248
	public float layerIntersectAuthoredSlopesDist = 5f;

	// Token: 0x040008C9 RID: 2249
	public float runnerDepthChangeSmoothTime = 0.2f;

	// Token: 0x040008CA RID: 2250
	public float fixedZChangeMinSpeed = 2f;

	// Token: 0x040008CB RID: 2251
	public float finalSnapZ = 0.05f;
}
