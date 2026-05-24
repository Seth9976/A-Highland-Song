using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000031 RID: 49
	public readonly struct StateStack : IDisposable
	{
		// Token: 0x060009D6 RID: 2518 RVA: 0x000237C3 File Offset: 0x000219C3
		internal static void Push(DrawStyle style, Matrix4x4 mtx)
		{
			StyleStack.Push(style);
			MatrixStack.Push(mtx);
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x000237D1 File Offset: 0x000219D1
		internal static void Pop()
		{
			MatrixStack.Pop();
			StyleStack.Pop();
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x000237DD File Offset: 0x000219DD
		internal StateStack(DrawStyle style, Matrix4x4 mtx)
		{
			StateStack.Push(style, mtx);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x000237E6 File Offset: 0x000219E6
		public void Dispose()
		{
			StateStack.Pop();
		}
	}
}
