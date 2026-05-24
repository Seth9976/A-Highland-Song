using System;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public class SLayoutAngleProperty : SLayoutProperty<float>
{
	// Token: 0x0600104B RID: 4171 RVA: 0x0007935E File Offset: 0x0007755E
	public override float Lerp(float v0, float v1, float t)
	{
		return Mathf.LerpAngle(v0, v1, t);
	}
}
