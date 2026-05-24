using System;

// Token: 0x0200015E RID: 350
[Serializable]
public struct WeatherOverride
{
	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06000BC9 RID: 3017 RVA: 0x0005EBD3 File Offset: 0x0005CDD3
	public bool active
	{
		get
		{
			return this.expiryDaysNorm > GameClock.instance.daysNorm;
		}
	}

	// Token: 0x04000DF4 RID: 3572
	public WeatherType weatherType;

	// Token: 0x04000DF5 RID: 3573
	public float expiryDaysNorm;
}
