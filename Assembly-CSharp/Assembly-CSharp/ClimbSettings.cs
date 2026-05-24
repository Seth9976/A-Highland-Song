using System;
using UnityEngine;

// Token: 0x020000D1 RID: 209
public class ClimbSettings : ScriptableObject
{
	// Token: 0x04000858 RID: 2136
	public float inputUpDownThreshold = 0.5f;

	// Token: 0x04000859 RID: 2137
	public float climbStartHeightFromIdle = 5f;

	// Token: 0x0400085A RID: 2138
	public float climbStartHeightFromScramble = 3f;

	// Token: 0x0400085B RID: 2139
	public float climbStartHeightFromMidAir = 2f;

	// Token: 0x0400085C RID: 2140
	public float climbMaxReachDist = 3f;

	// Token: 0x0400085D RID: 2141
	public float climbAutoReachDist = 3f;

	// Token: 0x0400085E RID: 2142
	public float downwardSpeedup = 1.5f;

	// Token: 0x0400085F RID: 2143
	public float climbFromScrambleCheckDist = 2f;

	// Token: 0x04000860 RID: 2144
	public float climbDownStraightToGroundDist = 4f;

	// Token: 0x04000861 RID: 2145
	public float climbDownToHoldDist = 1.5f;

	// Token: 0x04000862 RID: 2146
	public Vector2 climbDownRayStart = new Vector2(-1f, 0.5f);

	// Token: 0x04000863 RID: 2147
	public Vector2 climbDownRayStart2 = new Vector2(-1f, 0.5f);

	// Token: 0x04000864 RID: 2148
	public Vector2 climbDownRay = new Vector2(-1f, -6f);

	// Token: 0x04000865 RID: 2149
	public Vector2 climbDownRay2 = new Vector2(-1f, -6f);

	// Token: 0x04000866 RID: 2150
	public float climbDownEdgeDist = 3f;

	// Token: 0x04000867 RID: 2151
	public float climbDownHookRayDistX = 2f;

	// Token: 0x04000868 RID: 2152
	public float climbDownHookRayDistY = 5f;

	// Token: 0x04000869 RID: 2153
	public float climbDownToSlopeMinDistForPrompt = 5f;

	// Token: 0x0400086A RID: 2154
	public float timeToLookDownBeforeStartClimb = 0.5f;

	// Token: 0x0400086B RID: 2155
	public float climbReachUpDuration = 0.6f;

	// Token: 0x0400086C RID: 2156
	public float climbReachDownDuration = 1f;

	// Token: 0x0400086D RID: 2157
	public float climbReachAnimDistance = 2f;

	// Token: 0x0400086E RID: 2158
	public float climbUpperRayY = 2.5f;

	// Token: 0x0400086F RID: 2159
	public float climbLowerRayY = 2.5f;

	// Token: 0x04000870 RID: 2160
	public float climbRayBackX = 2f;

	// Token: 0x04000871 RID: 2161
	public float climbRayLength = 5f;

	// Token: 0x04000872 RID: 2162
	public float climbRotationSpeed = 90f;

	// Token: 0x04000873 RID: 2163
	public float maxOverhangAngle = 135f;

	// Token: 0x04000874 RID: 2164
	public float timeBeforeWallSlide1 = 0.15f;

	// Token: 0x04000875 RID: 2165
	public float timeBeforeWallSlide2 = 0.15f;

	// Token: 0x04000876 RID: 2166
	[Header("Slipping while climbing")]
	public Range climbTimeToSlip = new Range(2f, 4f);

	// Token: 0x04000877 RID: 2167
	public float slipTimeLimit = 1f;

	// Token: 0x04000878 RID: 2168
	public float slipTimeScalarStopped = 0.2f;

	// Token: 0x04000879 RID: 2169
	public float slipTimeScalarLowStamina = 2f;

	// Token: 0x0400087A RID: 2170
	public float slipTimeScalarBadWeather = 3f;

	// Token: 0x0400087B RID: 2171
	public float slipTimeSprintScalar = 2.5f;

	// Token: 0x0400087C RID: 2172
	public float slipSwingTorque = 1f;

	// Token: 0x0400087D RID: 2173
	public float slipSwingDamping = 0.98f;

	// Token: 0x0400087E RID: 2174
	public float headToFootRaycastLength = 5f;

	// Token: 0x0400087F RID: 2175
	[Header("Climbing jump")]
	public float climbingJumpStaminaCost = 0.4f;

	// Token: 0x04000880 RID: 2176
	[Header("Hurt with low stamina")]
	public float climbingWithLowStaminaInterval = 1f;

	// Token: 0x04000881 RID: 2177
	[Header("Sprint button held")]
	public float sprintButtonSpeedup = 1.5f;

	// Token: 0x04000882 RID: 2178
	[Header("Climb off to ledge")]
	public Range climbOffToLedgeRangeX = new Range(-4f, -1f);

	// Token: 0x04000883 RID: 2179
	public Range climbOffToLedgeRangeY = new Range(-3f, 3f);

	// Token: 0x04000884 RID: 2180
	public float climbOffSearchX = -2f;

	// Token: 0x04000885 RID: 2181
	public float climbOffRunDistDuringAnim = 2f;

	// Token: 0x04000886 RID: 2182
	public Range climbOffDistRange = new Range(2f, 6f);

	// Token: 0x04000887 RID: 2183
	public float climbOffMinSpeedScalar = 0.7f;
}
