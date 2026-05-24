using System;
using UnityEngine;

// Token: 0x0200002A RID: 42
public class GameClockSettings : ScriptableObject
{
	// Token: 0x040001D5 RID: 469
	public float gameRealSecondsPerGameMinute = 3f;

	// Token: 0x040001D6 RID: 470
	public float gameRealSecondsPerGameMinuteWhileNarrating = 10f;

	// Token: 0x040001D7 RID: 471
	public float shelterRealSecondsPerGameHour = 0.5f;

	// Token: 0x040001D8 RID: 472
	public float restingRealSecondsPerGameHour = 0.5f;

	// Token: 0x040001D9 RID: 473
	public float restAccelRealDuration = 3f;

	// Token: 0x040001DA RID: 474
	public float restDecelRealDuration = 1f;

	// Token: 0x040001DB RID: 475
	public float shelterTimeAccelStartTime = 1f;

	// Token: 0x040001DC RID: 476
	public float shelterTimeAccelEndTime = 2f;

	// Token: 0x040001DD RID: 477
	[Space]
	public Vector2 inLevelPathLengthBounds = new Vector2(15f, 60f);

	// Token: 0x040001DE RID: 478
	public Vector2 inLevelPathDurationBoundsInMinutes = new Vector2(3f, 14f);

	// Token: 0x040001DF RID: 479
	[Space]
	public Vector2 betweenLevelPathLengthBounds = new Vector2(300f, 800f);

	// Token: 0x040001E0 RID: 480
	public Vector2 betweenLevelPathDurationBoundsInMinutes = new Vector2(45f, 300f);

	// Token: 0x040001E1 RID: 481
	public float easterEggPathTimeScalar = 0.75f;

	// Token: 0x040001E2 RID: 482
	[Space]
	[Info("This is an arbitrary range since it's daysNormDelta * 10,000. Actual time passed per frame is around 0.000027.")]
	public Range deltaRangeForVisualEffects = new Range(0.3f, 10f);

	// Token: 0x040001E3 RID: 483
	public float timeSpeedNormPowerForVisualEffects = 1f;

	// Token: 0x040001E4 RID: 484
	public Range visualEffectsAnimSpeedRange = new Range(10f, 100f);
}
