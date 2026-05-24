using System;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class ChunkGapHint : MonoBehaviour
{
	// Token: 0x0600033D RID: 829 RVA: 0x0001A128 File Offset: 0x00018328
	private void OnDrawGizmosSelected()
	{
		if (this.foundLeftSlope != null && this.foundRightSlope != null)
		{
			Gizmos.color = Color.cyan.WithAlpha(0.5f);
			Gizmos.DrawLine(this.foundLeftSlope.rightPoint, this.foundRightSlope.leftPoint);
		}
		else
		{
			Gizmos.color = Color.red;
		}
		Gizmos.DrawWireSphere(base.transform.position, 0.2f);
	}

	// Token: 0x0400047B RID: 1147
	[NonSerialized]
	public Slope foundLeftSlope;

	// Token: 0x0400047C RID: 1148
	[NonSerialized]
	public Slope foundRightSlope;

	// Token: 0x0400047D RID: 1149
	[NonSerialized]
	public bool leftHintWasRequired;

	// Token: 0x0400047E RID: 1150
	[NonSerialized]
	public bool rightHintWasRequired;
}
