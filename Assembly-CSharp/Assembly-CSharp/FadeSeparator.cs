using System;
using UnityEngine;

// Token: 0x0200016D RID: 365
public class FadeSeparator : MonoBehaviour
{
	// Token: 0x06000C1E RID: 3102 RVA: 0x00060E4B File Offset: 0x0005F04B
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		GizmosX.DrawWireCylinder(base.transform.position, Quaternion.LookRotation(Vector3.up, Vector3.forward), this.radius, this.depth, 12);
	}

	// Token: 0x04000E70 RID: 3696
	public float radius = 10f;

	// Token: 0x04000E71 RID: 3697
	public float depth = 3f;

	// Token: 0x04000E72 RID: 3698
	[Range(0f, 1f)]
	public float opacityScalar = 0.5f;
}
