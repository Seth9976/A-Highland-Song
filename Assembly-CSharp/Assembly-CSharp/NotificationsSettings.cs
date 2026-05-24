using System;
using UnityEngine;

// Token: 0x0200012D RID: 301
public class NotificationsSettings : ScriptableObject
{
	// Token: 0x04000C59 RID: 3161
	public float xFromRight = 200f;

	// Token: 0x04000C5A RID: 3162
	public float bottomMargin = 50f;

	// Token: 0x04000C5B RID: 3163
	public float visibleTime = 10f;

	// Token: 0x04000C5C RID: 3164
	public float delayBetweenQueuedNotifications = 1f;

	// Token: 0x04000C5D RID: 3165
	public float delayBeforeGaelic = 0.5f;

	// Token: 0x04000C5E RID: 3166
	public float popInDuration = 0.4f;

	// Token: 0x04000C5F RID: 3167
	public float popOutDuration = 0.4f;

	// Token: 0x04000C60 RID: 3168
	public AnimationCurve popInCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04000C61 RID: 3169
	public AnimationCurve popOutCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04000C62 RID: 3170
	public float offscreenOffset = 300f;

	// Token: 0x04000C63 RID: 3171
	public PeakIconDatabase peakIconDatabase;

	// Token: 0x04000C64 RID: 3172
	public Sprite defaultIcon;
}
