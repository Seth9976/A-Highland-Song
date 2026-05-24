using System;

// Token: 0x02000128 RID: 296
public struct MarkerState
{
	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06000A0E RID: 2574 RVA: 0x00054FE2 File Offset: 0x000531E2
	public bool isValidPlacedMarker
	{
		get
		{
			return this.inkName != null;
		}
	}

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x06000A0F RID: 2575 RVA: 0x00054FF0 File Offset: 0x000531F0
	public static MarkerState unplacedOrNotAMarker
	{
		get
		{
			return default(MarkerState);
		}
	}

	// Token: 0x04000C3A RID: 3130
	public string inkName;

	// Token: 0x04000C3B RID: 3131
	public bool correct;
}
