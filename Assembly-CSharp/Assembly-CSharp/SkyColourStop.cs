using System;
using UnityEngine;

// Token: 0x02000049 RID: 73
[Serializable]
public struct SkyColourStop
{
	// Token: 0x0600020E RID: 526 RVA: 0x0001205C File Offset: 0x0001025C
	public static SkyColourStop Lerp(SkyColourStop a, SkyColourStop b, float lerp)
	{
		return new SkyColourStop
		{
			top = Color.Lerp(a.top, b.top, lerp),
			mid = Color.Lerp(a.mid, b.mid, lerp),
			bottom = Color.Lerp(a.bottom, b.bottom, lerp),
			scale = Mathf.Lerp(a.scale, b.scale, lerp),
			offset = Mathf.Lerp(a.offset, b.offset, lerp)
		};
	}

	// Token: 0x040002F5 RID: 757
	public Color top;

	// Token: 0x040002F6 RID: 758
	public Color mid;

	// Token: 0x040002F7 RID: 759
	public Color bottom;

	// Token: 0x040002F8 RID: 760
	public float scale;

	// Token: 0x040002F9 RID: 761
	public float offset;
}
