using System;
using UnityEngine;

// Token: 0x02000080 RID: 128
public class RunTrackSettings : ScriptableObject
{
	// Token: 0x040004EC RID: 1260
	public int failureRewindBars = 1;

	// Token: 0x040004ED RID: 1261
	public float defaultBeatDuration = 0.5f;

	// Token: 0x040004EE RID: 1262
	public float minimumRunLeadInTimeBeforeObstacle = 2f;

	// Token: 0x040004EF RID: 1263
	public float minimumXSpaceRequireToStart = 250f;
}
