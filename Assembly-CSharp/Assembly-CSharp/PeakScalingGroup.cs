using System;
using UnityEngine;

// Token: 0x0200019A RID: 410
public class PeakScalingGroup : MonoInstancer<PeakScalingGroup>
{
	// Token: 0x06000D5E RID: 3422 RVA: 0x0006AD08 File Offset: 0x00068F08
	private void Awake()
	{
		this.worldScalingOrigin = base.transform.position + this.localScalingOrigin;
	}

	// Token: 0x06000D5F RID: 3423 RVA: 0x0006AD28 File Offset: 0x00068F28
	public static void UpdateAll(float transition)
	{
		PeakScalingGroup._groupsScale = Mathf.Lerp(1f, 0.1f, Mathf.SmoothStep(0f, 1f, transition));
		foreach (PeakScalingGroup peakScalingGroup in MonoInstancer<PeakScalingGroup>.all)
		{
			if (Vector3.Distance(peakScalingGroup.transform.position, Runner.instance.transform.position) >= 50f)
			{
				peakScalingGroup.transform.position = peakScalingGroup.worldScalingOrigin - PeakScalingGroup._groupsScale * peakScalingGroup.localScalingOrigin;
				Vector3 localScale = peakScalingGroup.transform.localScale;
				localScale.z = PeakScalingGroup._groupsScale;
				peakScalingGroup.transform.localScale = localScale;
			}
		}
	}

	// Token: 0x04001040 RID: 4160
	[Info("Flattens contents in Z when in peak view to prevent perspective distortion to split them apart. Please include both art and gameplay objects so they stay together.")]
	public Vector3 localScalingOrigin;

	// Token: 0x04001041 RID: 4161
	[NonSerialized]
	public Vector3 worldScalingOrigin;

	// Token: 0x04001042 RID: 4162
	private static float _groupsScale = 1f;
}
