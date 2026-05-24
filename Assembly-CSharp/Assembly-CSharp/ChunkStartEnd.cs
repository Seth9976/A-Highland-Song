using System;
using UnityEngine;

// Token: 0x02000072 RID: 114
public class ChunkStartEnd : MonoBehaviour
{
	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x0600033F RID: 831 RVA: 0x0001A1A9 File Offset: 0x000183A9
	public bool pointOnLeftOfSlope
	{
		get
		{
			return this.isStart ^ !this.rightwards;
		}
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000340 RID: 832 RVA: 0x0001A1BC File Offset: 0x000183BC
	public Vector3 point
	{
		get
		{
			Vector3 position = base.transform.position;
			if (this.slope != null)
			{
				return this.slope.SampleAt(position.x, false).point3d;
			}
			return position;
		}
	}

	// Token: 0x0400047F RID: 1151
	public bool isStart = true;

	// Token: 0x04000480 RID: 1152
	public bool rightwards = true;

	// Token: 0x04000481 RID: 1153
	[NonSerialized]
	public Slope slope;
}
