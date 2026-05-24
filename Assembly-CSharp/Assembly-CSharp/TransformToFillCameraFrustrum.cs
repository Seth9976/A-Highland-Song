using System;
using UnityEngine;

// Token: 0x020000FB RID: 251
[ExecuteInEditMode]
public class TransformToFillCameraFrustrum : MonoBehaviour
{
	// Token: 0x06000853 RID: 2131 RVA: 0x00048100 File Offset: 0x00046300
	private void LateUpdate()
	{
		if (this.camera == null)
		{
			return;
		}
		float frustrumHeightAtDistance = this.camera.GetFrustrumHeightAtDistance(this.distance);
		float num = this.camera.ConvertFrustumHeightToFrustumWidth(frustrumHeightAtDistance);
		base.transform.position = this.camera.transform.position + this.camera.transform.forward * this.distance;
		base.transform.rotation = this.camera.transform.rotation;
		base.transform.localScale = new Vector3(num, frustrumHeightAtDistance, 1f);
	}

	// Token: 0x04000A0A RID: 2570
	public Camera camera;

	// Token: 0x04000A0B RID: 2571
	public float distance = 1f;
}
