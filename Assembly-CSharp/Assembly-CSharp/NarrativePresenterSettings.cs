using System;
using UnityEngine;

// Token: 0x02000090 RID: 144
public class NarrativePresenterSettings : ScriptableObject
{
	// Token: 0x040005B3 RID: 1459
	public DialogueBubbleSettings bubbleSettings;

	// Token: 0x040005B4 RID: 1460
	public float subtitlesDefaultY = 50f;

	// Token: 0x040005B5 RID: 1461
	public float subtitlesYWithRestMenuVisible = 200f;

	// Token: 0x040005B6 RID: 1462
	public float subtitlesYBlackout = 500f;

	// Token: 0x040005B7 RID: 1463
	[Space]
	public float playerSkippableTimeOffset = -0.2f;

	// Token: 0x040005B8 RID: 1464
	public float absoluteMinimumTimeBeforeSkip = 0.25f;

	// Token: 0x040005B9 RID: 1465
	public TextReadSettings textReadSettings;

	// Token: 0x040005BA RID: 1466
	public TextReadSettings subtitleToAudioMinimumTiming;

	// Token: 0x040005BB RID: 1467
	public TextReadSettings subtitleWithoutAudioTiming;

	// Token: 0x040005BC RID: 1468
	public float skipDialogueScalar = 3f;

	// Token: 0x040005BD RID: 1469
	public Range playerReadingSpeedScalarRange = new Range(0.3f, 2f);

	// Token: 0x040005BE RID: 1470
	public Range textAnimSpeedScalarRange = new Range(0.7f, 1.5f);
}
