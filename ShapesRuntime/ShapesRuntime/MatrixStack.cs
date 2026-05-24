using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200001B RID: 27
	public readonly struct MatrixStack : IDisposable
	{
		// Token: 0x06000939 RID: 2361 RVA: 0x00021BAF File Offset: 0x0001FDAF
		internal static void Push(Matrix4x4 prevState)
		{
			MatrixStack.matrices.Push(prevState);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x00021BBC File Offset: 0x0001FDBC
		internal static void Pop()
		{
			try
			{
				Draw.Matrix = MatrixStack.matrices.Pop();
			}
			catch (Exception ex)
			{
				Debug.LogError("You are popping more Matrix4x4 stacks than you are pushing. error: " + ex.Message);
			}
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00021C04 File Offset: 0x0001FE04
		internal MatrixStack(Matrix4x4 mtx)
		{
			MatrixStack.matrices.Push(mtx);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00021C11 File Offset: 0x0001FE11
		public void Dispose()
		{
			MatrixStack.Pop();
		}

		// Token: 0x040000CC RID: 204
		private static readonly Stack<Matrix4x4> matrices = new Stack<Matrix4x4>();
	}
}
