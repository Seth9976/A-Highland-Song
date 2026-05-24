using System;
using UnityEngine;

// Token: 0x020000D3 RID: 211
public class FallSettings : ScriptableObject
{
	// Token: 0x0400088E RID: 2190
	public float fallBounceRestitution = 1f;

	// Token: 0x0400088F RID: 2191
	public float fallBounceDamping = 0.5f;

	// Token: 0x04000890 RID: 2192
	public float fallBounceRestitutionTumble = 1f;

	// Token: 0x04000891 RID: 2193
	public float fallBounceDampingTumble = 0.5f;

	// Token: 0x04000892 RID: 2194
	public float fallTerminalSpeed = 20f;

	// Token: 0x04000893 RID: 2195
	public float fallHeightMinor = 10f;

	// Token: 0x04000894 RID: 2196
	public float fallHeightMajor = 25f;

	// Token: 0x04000895 RID: 2197
	public float fallHeightDeath = 40f;

	// Token: 0x04000896 RID: 2198
	public float tumbleBigBounceChance = 0.2f;

	// Token: 0x04000897 RID: 2199
	public float tumbleBigBounceRestitution = 0.6f;

	// Token: 0x04000898 RID: 2200
	public int snowLandingParticleCount = 40;

	// Token: 0x04000899 RID: 2201
	public int snowLandingFloatyParticleCount = 40;

	// Token: 0x0400089A RID: 2202
	public Range tumbleSpeedRange = new Range(60f, 180f);

	// Token: 0x0400089B RID: 2203
	public float slopeBounceMinAngle = 20f;

	// Token: 0x0400089C RID: 2204
	public float slopeBounceMinSpeed = 10f;

	// Token: 0x0400089D RID: 2205
	public float minCollisionSpeedToStartTumble = 15f;
}
