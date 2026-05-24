using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
public class CameraPushVolume : MonoBehaviour
{
	// Token: 0x060000AA RID: 170 RVA: 0x00009E3C File Offset: 0x0000803C
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawWireSphere(Vector3.zero, 1f);
		Gizmos.DrawLine(Vector3.zero, 2f * Vector3.up);
		Gizmos.matrix = Matrix4x4.identity;
	}

	// Token: 0x04000102 RID: 258
	[Info("This volume pushes camera upward on *local* Y axis, and has a circular falloff on XZ axis")]
	[SerializeField]
	[Disable]
	private bool _;
}
