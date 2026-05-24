using System;
using UnityEngine;

// Token: 0x020000DC RID: 220
public class WallSlideSettings : ScriptableObject
{
	// Token: 0x0400093A RID: 2362
	public float maxSlideAngle = 95f;

	// Token: 0x0400093B RID: 2363
	public float acceleration = 10f;

	// Token: 0x0400093C RID: 2364
	public float maxSpeed = 40f;

	// Token: 0x0400093D RID: 2365
	public float speedForDamage = 20f;

	// Token: 0x0400093E RID: 2366
	public float staminaDuration = 5f;

	// Token: 0x0400093F RID: 2367
	public float jumpDistScalar = 1f;

	// Token: 0x04000940 RID: 2368
	public float jumpHeightScalar = 0.35f;

	// Token: 0x04000941 RID: 2369
	public float jumpTargetYOffset = -6f;

	// Token: 0x04000942 RID: 2370
	public float minSlideBeforeClimb = 0.6f;

	// Token: 0x04000943 RID: 2371
	public float hurtDistance = 10f;
}
