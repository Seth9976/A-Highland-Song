using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000070 RID: 112
public class ChunkAlignSettings : MonoBehaviour
{
	// Token: 0x0600033B RID: 827 RVA: 0x0001A0E8 File Offset: 0x000182E8
	private void Reset()
	{
		this.mode = ((base.GetComponent<Poly>() != null || base.GetComponent<SortingGroup>() != null) ? ChunkAlignMode.All : ChunkAlignMode.Move);
	}

	// Token: 0x04000478 RID: 1144
	public bool alignChildren;

	// Token: 0x04000479 RID: 1145
	public bool alsoAlignSelf;

	// Token: 0x0400047A RID: 1146
	[EnumFlag]
	public ChunkAlignMode mode;
}
