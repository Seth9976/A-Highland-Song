using System;
using UnityEngine;

// Token: 0x02000176 RID: 374
public class BackgroundClouds : MonoBehaviour
{
	// Token: 0x06000C56 RID: 3158 RVA: 0x000629C0 File Offset: 0x00060BC0
	private void LateUpdate()
	{
		Vector3 position = base.transform.position;
		Vector3 position2 = GameCamera.instance.transform.position;
		position.x = position2.x;
		position.z = position2.z + this.followDistance;
		base.transform.position = position;
	}

	// Token: 0x04000EC9 RID: 3785
	public float followDistance;
}
