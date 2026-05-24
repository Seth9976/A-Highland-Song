using System;
using UnityEngine;

// Token: 0x020001D3 RID: 467
[RequireComponent(typeof(Canvas))]
public class SLayoutCanvasTimeScalar : MonoBehaviour
{
	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x06001043 RID: 4163 RVA: 0x000792D2 File Offset: 0x000774D2
	public float timeScale
	{
		get
		{
			if (this.timeType == SLayoutCanvasTimeScalar.TimeType.Scaled)
			{
				return this.timeScaleMultiplier * Time.timeScale;
			}
			return this.timeScaleMultiplier;
		}
	}

	// Token: 0x04001213 RID: 4627
	public SLayoutCanvasTimeScalar.TimeType timeType;

	// Token: 0x04001214 RID: 4628
	public float timeScaleMultiplier = 1f;

	// Token: 0x020003EB RID: 1003
	public enum TimeType
	{
		// Token: 0x04001A55 RID: 6741
		Scaled,
		// Token: 0x04001A56 RID: 6742
		Unscaled
	}
}
