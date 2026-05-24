using System;
using UnityEngine;

// Token: 0x020000DA RID: 218
public class TransitionSettings : ScriptableObject
{
	// Token: 0x04000923 RID: 2339
	public TransitionSettings.Transition climbOntoWall;

	// Token: 0x04000924 RID: 2340
	public TransitionSettings.Transition climbOntoWallFromMidAir;

	// Token: 0x04000925 RID: 2341
	public TransitionSettings.Transition climbOffWall;

	// Token: 0x04000926 RID: 2342
	public TransitionSettings.Transition climbDownFromLedge;

	// Token: 0x04000927 RID: 2343
	public TransitionSettings.Transition climbingJump;

	// Token: 0x04000928 RID: 2344
	public TransitionSettings.Transition climbUpAndOver;

	// Token: 0x04000929 RID: 2345
	public TransitionSettings.Transition bridgeGap;

	// Token: 0x0400092A RID: 2346
	public TransitionSettings.Transition clamber0_5m;

	// Token: 0x0400092B RID: 2347
	public TransitionSettings.Transition clamber1m;

	// Token: 0x0400092C RID: 2348
	public TransitionSettings.Transition clamber2m;

	// Token: 0x0400092D RID: 2349
	public TransitionSettings.Transition scrambleToClimb;

	// Token: 0x0400092E RID: 2350
	public TransitionSettings.Transition clamberDown2m;

	// Token: 0x0400092F RID: 2351
	public TransitionSettings.Transition clamberDown0_5m;

	// Token: 0x02000304 RID: 772
	[Serializable]
	public class Transition
	{
		// Token: 0x04001776 RID: 6006
		public FrameAnimation anim;

		// Token: 0x04001777 RID: 6007
		public Range positionAdjustTimeRange;

		// Token: 0x04001778 RID: 6008
		public Range highMomentumAnimRange = new Range(0f, 1f);

		// Token: 0x04001779 RID: 6009
		public Vocalisation vocalisation;

		// Token: 0x0400177A RID: 6010
		public float vocalisationTime;

		// Token: 0x0400177B RID: 6011
		public Vocalisation vocalisationEnd;

		// Token: 0x0400177C RID: 6012
		public float endFocusY;
	}
}
