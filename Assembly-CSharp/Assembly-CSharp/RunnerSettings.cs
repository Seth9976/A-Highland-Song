using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020000CC RID: 204
[NullableContext(1)]
[Nullable(0)]
[CreateAssetMenu]
public class RunnerSettings : ScriptableObject
{
	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x06000763 RID: 1891 RVA: 0x00042284 File Offset: 0x00040484
	public float standardGravity
	{
		get
		{
			float num = 0.5f * this.jump.jumpDurationStandard;
			return -this.jump.jumpHeightRange.Lerp(0.5f) / (0.5f * num * num);
		}
	}

	// Token: 0x04000820 RID: 2080
	public Vector2 worldSize = new Vector2(2f, 4.65f);

	// Token: 0x04000821 RID: 2081
	public RunSettings run = Presume<RunSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\RunnerSettings.cs", 8);

	// Token: 0x04000822 RID: 2082
	public JumpSettings jump = Presume<JumpSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\RunnerSettings.cs", 9);

	// Token: 0x04000823 RID: 2083
	public FallSettings fall = Presume<FallSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\RunnerSettings.cs", 10);

	// Token: 0x04000824 RID: 2084
	public BalanceSettings balance = Presume<BalanceSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\RunnerSettings.cs", 11);

	// Token: 0x04000825 RID: 2085
	public StaminaSettings stamina = Presume<StaminaSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\RunnerSettings.cs", 12);

	// Token: 0x04000826 RID: 2086
	public ClimbSettings climb = Presume<ClimbSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\RunnerSettings.cs", 13);

	// Token: 0x04000827 RID: 2087
	public TransitionSettings transition = Presume<TransitionSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\RunnerSettings.cs", 14);

	// Token: 0x04000828 RID: 2088
	public UpAndOverSettings upAndOver = Presume<UpAndOverSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\RunnerSettings.cs", 16);

	// Token: 0x04000829 RID: 2089
	public AnimSettings anim = Presume<AnimSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\RunnerSettings.cs", 17);

	// Token: 0x0400082A RID: 2090
	public LayerSettings layer = Presume<LayerSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\RunnerSettings.cs", 18);

	// Token: 0x0400082B RID: 2091
	public WallSlideSettings wallSlide = Presume<WallSlideSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\RunnerSettings.cs", 19);

	// Token: 0x0400082C RID: 2092
	public StoneSkimmingSettings stoneSkim = Presume<StoneSkimmingSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\RunnerSettings.cs", 20);

	// Token: 0x0400082D RID: 2093
	public BellyWriggleSettings bellyWriggle = Presume<BellyWriggleSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\RunnerSettings.cs", 21);

	// Token: 0x0400082E RID: 2094
	public FinalJumpSettings finalJump = Presume<FinalJumpSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\RunnerSettings.cs", 22);

	// Token: 0x0400082F RID: 2095
	public SkiLiftSettings skiLift = Presume<SkiLiftSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\RunnerSettings.cs", 23);

	// Token: 0x04000830 RID: 2096
	public ZipLineSettings zipLine = Presume<ZipLineSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\RunnerSettings.cs", 24);

	// Token: 0x04000831 RID: 2097
	public ExplosionSettings explosion = Presume<ExplosionSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\RunnerSettings.cs", 25);

	// Token: 0x04000832 RID: 2098
	[Header("Footsteps")]
	public float footstepTimeNorm;

	// Token: 0x04000833 RID: 2099
	public float footstepCatchupSpeed = 0.5f;

	// Token: 0x04000834 RID: 2100
	[Header("Trip")]
	public AnimationCurve tripRotationAnim = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04000835 RID: 2101
	public float tripDamping = 0.95f;

	// Token: 0x04000836 RID: 2102
	[Header("Dropping down")]
	public float dropDownRayDist = 20f;

	// Token: 0x04000837 RID: 2103
	public float dropDownMaxMomentum = 0.5f;

	// Token: 0x04000838 RID: 2104
	public float cliffEdgeWobbleConfirmTime = 1f;

	// Token: 0x04000839 RID: 2105
	[Header("Flying (debug)")]
	public float flySpeed = 1f;

	// Token: 0x0400083A RID: 2106
	public float flyDepthSpeed = 1f;

	// Token: 0x0400083B RID: 2107
	public float flyDamping = 0.8f;

	// Token: 0x0400083C RID: 2108
	[Header("Oval collision")]
	public float ovalSweptHeightForJump = 2.5f;

	// Token: 0x0400083D RID: 2109
	public Vector2 ovalSweptSizeForJump = new Vector2(2f, 3.5f);

	// Token: 0x0400083E RID: 2110
	[Info("Need to leave enough space above the knee so that you can get to the edge of a slope under a step and allow Simulate to traverse it")]
	public float ovalSweptHeightForRun = 3f;

	// Token: 0x0400083F RID: 2111
	public Vector2 ovalSweptSizeForRun = new Vector2(1.2f, 2.5f);

	// Token: 0x04000840 RID: 2112
	[Header("Awkward slope angle")]
	[Info("These are coloured so we know to avoid slopes and climbables of this angle")]
	public Range awkwardSlopeAngle = new Range(45f, 60f);
}
