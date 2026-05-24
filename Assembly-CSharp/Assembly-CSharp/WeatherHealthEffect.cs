using System;

// Token: 0x020000B6 RID: 182
[Flags]
public enum WeatherHealthEffect
{
	// Token: 0x04000696 RID: 1686
	None = 0,
	// Token: 0x04000697 RID: 1687
	Wind = 1,
	// Token: 0x04000698 RID: 1688
	StrongWind = 2,
	// Token: 0x04000699 RID: 1689
	WindMask = 3,
	// Token: 0x0400069A RID: 1690
	Chilly = 4,
	// Token: 0x0400069B RID: 1691
	Cold = 8,
	// Token: 0x0400069C RID: 1692
	Freezing = 16,
	// Token: 0x0400069D RID: 1693
	TemperatureMask = 28,
	// Token: 0x0400069E RID: 1694
	Rain = 32,
	// Token: 0x0400069F RID: 1695
	Storm = 64,
	// Token: 0x040006A0 RID: 1696
	Snow = 128,
	// Token: 0x040006A1 RID: 1697
	WeatherMask = 224,
	// Token: 0x040006A2 RID: 1698
	PreventsHealing = 227,
	// Token: 0x040006A3 RID: 1699
	Night = 256
}
