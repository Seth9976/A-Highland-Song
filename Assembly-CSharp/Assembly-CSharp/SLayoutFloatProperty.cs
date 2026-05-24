using System;

// Token: 0x020001D5 RID: 469
public class SLayoutFloatProperty : SLayoutProperty<float>
{
	// Token: 0x06001049 RID: 4169 RVA: 0x0007934D File Offset: 0x0007754D
	public override float Lerp(float v0, float v1, float t)
	{
		return v0 + t * (v1 - v0);
	}
}
