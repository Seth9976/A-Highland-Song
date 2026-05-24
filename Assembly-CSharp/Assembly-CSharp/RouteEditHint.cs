using System;
using UnityEngine;

// Token: 0x0200003C RID: 60
public class RouteEditHint : MonoBehaviour
{
	// Token: 0x060001E6 RID: 486 RVA: 0x000115E4 File Offset: 0x0000F7E4
	private void OnDrawGizmos()
	{
		GizmosX.DrawCircle(base.transform.position, this.radius, 16);
	}

	// Token: 0x040002A7 RID: 679
	public float radius = 20f;

	// Token: 0x040002A8 RID: 680
	public bool fullRoute = true;
}
