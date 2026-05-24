using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000011 RID: 17
	public readonly struct ColorStack : IDisposable
	{
		// Token: 0x060001DF RID: 479 RVA: 0x00006229 File Offset: 0x00004429
		internal static void Push(Color prevState)
		{
			ColorStack.colors.Push(prevState);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00006238 File Offset: 0x00004438
		internal static void Pop()
		{
			try
			{
				Draw.Color = ColorStack.colors.Pop();
			}
			catch (Exception ex)
			{
				Debug.LogError("You are popping more Color stacks than you are pushing. error: " + ex.Message);
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00006280 File Offset: 0x00004480
		internal ColorStack(Color mtx)
		{
			ColorStack.colors.Push(mtx);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000628D File Offset: 0x0000448D
		public void Dispose()
		{
			ColorStack.Pop();
		}

		// Token: 0x04000075 RID: 117
		private static readonly Stack<Color> colors = new Stack<Color>();
	}
}
