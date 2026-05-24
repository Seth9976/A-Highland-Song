using System;
using System.Diagnostics;

namespace UnityEngine.Android
{
	// Token: 0x0200001D RID: 29
	public class PermissionCallbacks : AndroidJavaProxy
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060001C1 RID: 449 RVA: 0x00008244 File Offset: 0x00006444
		// (remove) Token: 0x060001C2 RID: 450 RVA: 0x0000827C File Offset: 0x0000647C
		[field: DebuggerBrowsable(0)]
		public event Action<string> PermissionGranted;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060001C3 RID: 451 RVA: 0x000082B4 File Offset: 0x000064B4
		// (remove) Token: 0x060001C4 RID: 452 RVA: 0x000082EC File Offset: 0x000064EC
		[field: DebuggerBrowsable(0)]
		public event Action<string> PermissionDenied;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060001C5 RID: 453 RVA: 0x00008324 File Offset: 0x00006524
		// (remove) Token: 0x060001C6 RID: 454 RVA: 0x0000835C File Offset: 0x0000655C
		[field: DebuggerBrowsable(0)]
		public event Action<string> PermissionDeniedAndDontAskAgain;

		// Token: 0x060001C7 RID: 455 RVA: 0x00008391 File Offset: 0x00006591
		public PermissionCallbacks()
			: base("com.unity3d.player.IPermissionRequestCallbacks")
		{
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000083A0 File Offset: 0x000065A0
		private void onPermissionGranted(string permissionName)
		{
			Action<string> permissionGranted = this.PermissionGranted;
			if (permissionGranted != null)
			{
				permissionGranted.Invoke(permissionName);
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000083B6 File Offset: 0x000065B6
		private void onPermissionDenied(string permissionName)
		{
			Action<string> permissionDenied = this.PermissionDenied;
			if (permissionDenied != null)
			{
				permissionDenied.Invoke(permissionName);
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000083CC File Offset: 0x000065CC
		private void onPermissionDeniedAndDontAskAgain(string permissionName)
		{
			bool flag = this.PermissionDeniedAndDontAskAgain != null;
			if (flag)
			{
				this.PermissionDeniedAndDontAskAgain.Invoke(permissionName);
			}
			else
			{
				Action<string> permissionDenied = this.PermissionDenied;
				if (permissionDenied != null)
				{
					permissionDenied.Invoke(permissionName);
				}
			}
		}
	}
}
