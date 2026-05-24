using System;
using UnityEngine;

// Token: 0x020001D7 RID: 471
public class SLayoutColorProperty : SLayoutProperty<Color>
{
	// Token: 0x0600104D RID: 4173 RVA: 0x00079370 File Offset: 0x00077570
	public override Color Lerp(Color v0, Color v1, float t)
	{
		return Color.Lerp(v0, v1, t);
	}
}
