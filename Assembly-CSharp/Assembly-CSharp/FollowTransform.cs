using System;
using UnityEngine;

// Token: 0x020000B1 RID: 177
public class FollowTransform : MonoBehaviour
{
	// Token: 0x06000592 RID: 1426 RVA: 0x0002C03A File Offset: 0x0002A23A
	private void Update()
	{
		if (this.transformToFollow == null)
		{
			return;
		}
		base.transform.position = this.transformToFollow.position;
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x0002C061 File Offset: 0x0002A261
	private void LateUpdate()
	{
		if (this.transformToFollow == null)
		{
			return;
		}
		base.transform.position = this.transformToFollow.position;
	}

	// Token: 0x04000665 RID: 1637
	public Transform transformToFollow;
}
