using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200019C RID: 412
public class BasePolyAndSlope : MonoBehaviour
{
	// Token: 0x04001044 RID: 4164
	public SurfaceType fallbackSurfaceType = SurfaceType.Rock;

	// Token: 0x04001045 RID: 4165
	[NonSerialized]
	public List<Splat> splats = new List<Splat>();
}
