using System;
using UnityEngine;

// Token: 0x020000AD RID: 173
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class CameraCopier : MonoBehaviour
{
	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06000584 RID: 1412 RVA: 0x0002BDAE File Offset: 0x00029FAE
	private Camera ownCamera
	{
		get
		{
			if (this._ownCamera == null || !Application.isPlaying)
			{
				this._ownCamera = base.GetComponent<Camera>();
			}
			return this._ownCamera;
		}
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x0002BDD7 File Offset: 0x00029FD7
	private void OnDisable()
	{
		this._ownCamera = null;
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x0002BDE0 File Offset: 0x00029FE0
	private void OnPreCull()
	{
		if (this.masterCamera == null)
		{
			return;
		}
		this.ownCamera.aspect = this.masterCamera.aspect;
		this.ownCamera.fieldOfView = this.masterCamera.fieldOfView;
	}

	// Token: 0x04000657 RID: 1623
	public Camera masterCamera;

	// Token: 0x04000658 RID: 1624
	private Camera _ownCamera;
}
