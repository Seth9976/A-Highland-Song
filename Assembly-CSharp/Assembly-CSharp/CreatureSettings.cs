using System;
using UnityEngine;

// Token: 0x0200017C RID: 380
[CreateAssetMenu]
public class CreatureSettings : ScriptableObject
{
	// Token: 0x04000F4E RID: 3918
	public float walkSpeed = 4f;

	// Token: 0x04000F4F RID: 3919
	public float runSpeed = 15f;

	// Token: 0x04000F50 RID: 3920
	public float slopeAngleFollowScalar = 0.5f;

	// Token: 0x04000F51 RID: 3921
	public Range jumpAngleOffset;

	// Token: 0x04000F52 RID: 3922
	public float jumpGravity = -100f;

	// Token: 0x04000F53 RID: 3923
	public float jumpDuration = 0.6f;

	// Token: 0x04000F54 RID: 3924
	public float musicRunJumpGravity = -100f;

	// Token: 0x04000F55 RID: 3925
	public Range grazeEatTime = new Range(1f, 5f);

	// Token: 0x04000F56 RID: 3926
	public float exitRunDownAngle = 30f;

	// Token: 0x04000F57 RID: 3927
	public float playerTooCloseProximity = 12f;

	// Token: 0x04000F58 RID: 3928
	public Range targetMusicRunPlayerOffsetRange = new Range(8f, 50f);

	// Token: 0x04000F59 RID: 3929
	public float targetMusicRunPlayerOffsetRangePeriod = 5f;

	// Token: 0x04000F5A RID: 3930
	public Sprite christmasHat;

	// Token: 0x04000F5B RID: 3931
	public float hatAngleOffset;

	// Token: 0x04000F5C RID: 3932
	public Vector2 hatOffset;
}
