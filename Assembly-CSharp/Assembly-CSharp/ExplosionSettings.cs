using System;
using UnityEngine;

// Token: 0x020000D2 RID: 210
public class ExplosionSettings : ScriptableObject
{
	// Token: 0x04000888 RID: 2184
	public float speed = 100f;

	// Token: 0x04000889 RID: 2185
	public float upwardDuration = 4f;

	// Token: 0x0400088A RID: 2186
	public float destinationFallHeight = 30f;

	// Token: 0x0400088B RID: 2187
	public float initialRotation = -90f;

	// Token: 0x0400088C RID: 2188
	public float rotationSpeed = 180f;

	// Token: 0x0400088D RID: 2189
	public float deceleration = 0.995f;
}
