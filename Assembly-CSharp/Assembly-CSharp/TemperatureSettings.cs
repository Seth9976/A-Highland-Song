using System;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class TemperatureSettings : ScriptableObject
{
	// Token: 0x040002CF RID: 719
	public float freezingTemperature = 1f;

	// Token: 0x040002D0 RID: 720
	public float coldTemperature = 6f;

	// Token: 0x040002D1 RID: 721
	public float chillyTemperature = 12f;

	// Token: 0x040002D2 RID: 722
	[Space]
	public float baseTemperature;

	// Token: 0x040002D3 RID: 723
	public AnimationCurve additiveTemperatureByHeight;

	// Token: 0x040002D4 RID: 724
	public AnimationCurve additiveTemperatureByTimeOfDay;
}
