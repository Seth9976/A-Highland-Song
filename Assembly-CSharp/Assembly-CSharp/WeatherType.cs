using System;

// Token: 0x02000160 RID: 352
[Flags]
public enum WeatherType
{
	// Token: 0x04000DFC RID: 3580
	Clear = 0,
	// Token: 0x04000DFD RID: 3581
	Cloudy = 2,
	// Token: 0x04000DFE RID: 3582
	VeryCloudy = 4,
	// Token: 0x04000DFF RID: 3583
	Raining = 8,
	// Token: 0x04000E00 RID: 3584
	Storm = 16,
	// Token: 0x04000E01 RID: 3585
	Snow = 32,
	// Token: 0x04000E02 RID: 3586
	Windy = 64,
	// Token: 0x04000E03 RID: 3587
	Foggy = 128,
	// Token: 0x04000E04 RID: 3588
	AllAny = 254
}
