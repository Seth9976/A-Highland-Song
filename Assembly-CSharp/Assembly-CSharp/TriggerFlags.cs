using System;

// Token: 0x020001B0 RID: 432
[Flags]
public enum TriggerFlags
{
	// Token: 0x0400113B RID: 4411
	None = 0,
	// Token: 0x0400113C RID: 4412
	DisableSitting = 1,
	// Token: 0x0400113D RID: 4413
	SnowSoftLanding = 2,
	// Token: 0x0400113E RID: 4414
	DisableDropDown = 4,
	// Token: 0x0400113F RID: 4415
	PreventZoomOut = 8,
	// Token: 0x04001140 RID: 4416
	VeryShortZoomOut = 16,
	// Token: 0x04001141 RID: 4417
	ShortZoomOut = 32,
	// Token: 0x04001142 RID: 4418
	MediumZoomOut = 64
}
