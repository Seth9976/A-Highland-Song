using System;
using UnityEngine;

// Token: 0x020000F9 RID: 249
[ExecuteInEditMode]
public class PostProcessDepthGreyscale : MonoBehaviour
{
	// Token: 0x060007FE RID: 2046 RVA: 0x00046510 File Offset: 0x00044710
	private void Start()
	{
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x00046512 File Offset: 0x00044712
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.camera.depthTextureMode = DepthTextureMode.Depth;
		Graphics.Blit(source, destination, this.mat);
	}

	// Token: 0x040009EB RID: 2539
	public Camera camera;

	// Token: 0x040009EC RID: 2540
	public Material mat;
}
