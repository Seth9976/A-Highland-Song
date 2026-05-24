using System;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class LatencyCalibratorSettings : ScriptableObject
{
	// Token: 0x040003FB RID: 1019
	[Header("The BPM of the click track")]
	public float bpm = 60f;

	// Token: 0x040003FC RID: 1020
	[Header("The range shown to the player")]
	public float latencyRange = 0.25f;

	// Token: 0x040003FD RID: 1021
	[Header("The max number of samples taken into account when calculating latency")]
	public int numSamples = 5;

	// Token: 0x040003FE RID: 1022
	[Range(0f, 1f)]
	[Header("Multiplies the strength of previous samples. When at 1, the last sample has a strength of 0")]
	public float sampleFalloff;

	// Token: 0x040003FF RID: 1023
	[Range(0f, 1f)]
	[Header("Auto-continue when player gets within a decent standard deviation range")]
	public float standardDeviationMax = 0.05f;
}
