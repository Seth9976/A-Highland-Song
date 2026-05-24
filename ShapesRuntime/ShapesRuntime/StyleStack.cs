using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000032 RID: 50
	public readonly struct StyleStack : IDisposable
	{
		// Token: 0x060009DA RID: 2522 RVA: 0x000237ED File Offset: 0x000219ED
		internal static void Push(DrawStyle prevState)
		{
			StyleStack.styles.Push(prevState);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x000237FC File Offset: 0x000219FC
		internal static void Pop()
		{
			try
			{
				Draw.style = StyleStack.styles.Pop();
			}
			catch (Exception ex)
			{
				Debug.LogError("You are popping more DrawStyle stacks than you are pushing. error: " + ex.Message);
			}
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00023844 File Offset: 0x00021A44
		internal StyleStack(DrawStyle mtx)
		{
			StyleStack.styles.Push(mtx);
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00023851 File Offset: 0x00021A51
		public void Dispose()
		{
			StyleStack.Pop();
		}

		// Token: 0x04000142 RID: 322
		private static readonly Stack<DrawStyle> styles = new Stack<DrawStyle>();
	}
}
