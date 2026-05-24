using System;

// Token: 0x020000B9 RID: 185
[Serializable]
public struct HealthEffect
{
	// Token: 0x060005E0 RID: 1504 RVA: 0x0002DFC4 File Offset: 0x0002C1C4
	public static HealthEffect operator +(HealthEffect a, HealthEffect b)
	{
		return new HealthEffect
		{
			healthPerDay = a.healthPerDay + b.healthPerDay,
			maxHealthPerDay = a.maxHealthPerDay + b.maxHealthPerDay
		};
	}

	// Token: 0x040006C0 RID: 1728
	public float healthPerDay;

	// Token: 0x040006C1 RID: 1729
	public float maxHealthPerDay;
}
