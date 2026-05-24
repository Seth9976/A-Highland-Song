using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
public class FreeCameraStateSettings : ScriptableObject
{
	// Token: 0x04000123 RID: 291
	public float speed = 10f;

	// Token: 0x04000124 RID: 292
	public float zSpeed = 10f;

	// Token: 0x04000125 RID: 293
	public float maxSpeedScalar = 10f;

	// Token: 0x04000126 RID: 294
	public float acceleration = 1f;

	// Token: 0x04000127 RID: 295
	public float speedScalarFalloffSpeed = 1f;

	// Token: 0x04000128 RID: 296
	public float velocityLerpAccel = 0.9f;

	// Token: 0x04000129 RID: 297
	public float velocityLerpDecel = 0.9f;

	// Token: 0x0400012A RID: 298
	public float panRestrictionScreenProportion = 0.9f;

	// Token: 0x0400012B RID: 299
	public float bonusPanExtent = 2f;
}
