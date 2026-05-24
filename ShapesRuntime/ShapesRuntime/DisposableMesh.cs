using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000012 RID: 18
	public class DisposableMesh : IDisposable
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x000062A0 File Offset: 0x000044A0
		public static int ActiveMeshCount
		{
			get
			{
				return DisposableMesh.activeMeshCount;
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000062A7 File Offset: 0x000044A7
		protected void EnsureMeshExists()
		{
			if (!this.hasMesh || this.mesh == null)
			{
				this.mesh = new Mesh
				{
					hideFlags = HideFlags.DontSave
				};
				DisposableMesh.activeMeshCount++;
				this.hasMesh = true;
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000062E8 File Offset: 0x000044E8
		public void Dispose()
		{
			if (this.hasMesh)
			{
				if (this.lastCommandUsedIn != null && !this.lastCommandUsedIn.hasRendered)
				{
					this.lastCommandUsedIn.cachedAssets.Add(this.mesh);
				}
				else
				{
					this.mesh.DestroyBranched();
				}
				DisposableMesh.activeMeshCount--;
				this.hasMesh = false;
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00006348 File Offset: 0x00004548
		protected void ClearMesh()
		{
			if (this.hasMesh)
			{
				this.mesh.Clear();
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000635D File Offset: 0x0000455D
		protected virtual bool ExternallyDirty()
		{
			return false;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00006360 File Offset: 0x00004560
		protected virtual void UpdateMesh()
		{
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00006362 File Offset: 0x00004562
		protected bool EnsureMeshIsReadyToRender(out Mesh outMesh, Action updateMesh)
		{
			if (!this.hasMesh)
			{
				outMesh = null;
				return false;
			}
			if (this.meshDirty)
			{
				updateMesh();
				this.meshDirty = false;
			}
			outMesh = this.mesh;
			return this.hasMesh;
		}

		// Token: 0x04000076 RID: 118
		private static int activeMeshCount;

		// Token: 0x04000077 RID: 119
		protected Mesh mesh;

		// Token: 0x04000078 RID: 120
		protected bool meshDirty;

		// Token: 0x04000079 RID: 121
		private bool hasMesh;

		// Token: 0x0400007A RID: 122
		internal DrawCommand lastCommandUsedIn;
	}
}
