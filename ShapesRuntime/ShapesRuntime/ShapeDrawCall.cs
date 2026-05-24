using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Shapes
{
	// Token: 0x02000030 RID: 48
	internal struct ShapeDrawCall
	{
		// Token: 0x060009D2 RID: 2514 RVA: 0x000236AD File Offset: 0x000218AD
		public ShapeDrawCall(ShapeDrawState drawState, Matrix4x4 matrix)
		{
			this.count = 1;
			this.drawState = drawState;
			this.matrix = matrix;
			this.instanced = false;
			this.mpb = ObjectPool<MaterialPropertyBlock>.Alloc();
			this.matrices = null;
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x000236DD File Offset: 0x000218DD
		public ShapeDrawCall(ShapeDrawState drawState, int count, Matrix4x4[] matrices)
		{
			this.count = count;
			this.drawState = drawState;
			this.matrices = matrices;
			this.instanced = true;
			this.mpb = ObjectPool<MaterialPropertyBlock>.Alloc();
			this.matrix = default(Matrix4x4);
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x00023714 File Offset: 0x00021914
		public void AddToCommandBuffer(CommandBuffer cmd)
		{
			if (this.instanced)
			{
				cmd.DrawMeshInstanced(this.drawState.mesh, this.drawState.submesh, this.drawState.mat, 0, this.matrices, this.count, this.mpb);
				return;
			}
			cmd.DrawMesh(this.drawState.mesh, this.matrix, this.drawState.mat, this.drawState.submesh, 0, this.mpb);
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x00023798 File Offset: 0x00021998
		public void Cleanup()
		{
			this.mpb.Clear();
			ObjectPool<MaterialPropertyBlock>.Free(this.mpb);
			if (this.instanced)
			{
				ArrayPool<Matrix4x4>.Free(this.matrices);
			}
		}

		// Token: 0x0400013C RID: 316
		public ShapeDrawState drawState;

		// Token: 0x0400013D RID: 317
		public MaterialPropertyBlock mpb;

		// Token: 0x0400013E RID: 318
		public int count;

		// Token: 0x0400013F RID: 319
		public Matrix4x4 matrix;

		// Token: 0x04000140 RID: 320
		public Matrix4x4[] matrices;

		// Token: 0x04000141 RID: 321
		private bool instanced;
	}
}
