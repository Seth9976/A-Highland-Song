using System;
using UnityEngine;

// Token: 0x02000078 RID: 120
public class ObstaclePlacement : MonoBehaviour
{
	// Token: 0x0600035D RID: 861 RVA: 0x0001A9A8 File Offset: 0x00018BA8
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawCube(base.transform.position + 1.5f * Vector3.up, new Vector3(0.2f, 3f, 0.2f));
	}

	// Token: 0x040004A5 RID: 1189
	public int halfBeatIdx;
}
