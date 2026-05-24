using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200001A RID: 26
	public class ImmediateModeShapeDrawer : MonoBehaviour
	{
		// Token: 0x06000934 RID: 2356 RVA: 0x00021B18 File Offset: 0x0001FD18
		public virtual void DrawShapes(Camera cam)
		{
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x00021B1C File Offset: 0x0001FD1C
		private void OnCameraPreRender(Camera cam)
		{
			CameraType cameraType = cam.cameraType;
			if (cameraType == CameraType.Preview || cameraType == CameraType.Reflection)
			{
				return;
			}
			if (this.useCullingMasks && (cam.cullingMask & (1 << base.gameObject.layer)) == 0)
			{
				return;
			}
			this.DrawShapes(cam);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x00021B63 File Offset: 0x0001FD63
		public virtual void OnEnable()
		{
			Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(this.OnCameraPreRender));
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x00021B85 File Offset: 0x0001FD85
		public virtual void OnDisable()
		{
			Camera.onPreRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPreRender, new Camera.CameraCallback(this.OnCameraPreRender));
		}

		// Token: 0x040000CB RID: 203
		[Tooltip("When enabled, shapes will only draw in cameras that can see the layer of this GameObject")]
		public bool useCullingMasks;
	}
}
